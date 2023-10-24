using System.Collections.Generic;
using Fusion;
using TMPro;
using UnityEngine;

public class NetworkPlayer : NetworkBehaviour, IPlayerLeft
{
    [Header("UI")]
    public TextMeshProUGUI playerNickNameTM;

    [Header("Sprite")]
    public SpriteRenderer playerSpriteRenderer;

    public enum PlayerState
    {
        pendingConnect,
        connected,
        playing,
        dead
    }

    [Networked(OnChanged = nameof(OnNickNameChanged))]
    public NetworkString<_16> NetNickName { get; set; }

    [Networked(OnChanged = nameof(OnPlayerStateChanged))]
    public PlayerState NetPlayerState { get; set; }

    [Networked(OnChanged = nameof(OnColorChanged))]
    public Color NetSpriteColor { get; set; }

    [Networked(OnChanged = nameof(OnFoodEatenChanged))]
    public ushort NetFoodEaten { get; set; }

    [Networked(OnChanged = nameof(OnHealthChanged))]
    public float NetHealth { get; set; }

    [Networked(OnChanged = nameof(OnMaxHealthChanged))]
    public float NetMaxHealth { get; set; }

    [Networked(OnChanged = nameof(OnStaminaChanged))]
    public float NetStamina { get; set; }

    [Networked(OnChanged = nameof(OnMaxStaminaChanged))]
    public float NetMaxStamina { get; set; }

    public static NetworkPlayer Local { get; set; }

    // Other components
    SettingsHandler settingsHandler;
    InGameUIHandler inGameUIHandler;
    MovementHandler movementHandler;

    void Awake()
    {
        settingsHandler = FindObjectOfType<SettingsHandler>();
        inGameUIHandler = FindObjectOfType<InGameUIHandler>();
        movementHandler = GetComponent<MovementHandler>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public override void FixedUpdateNetwork()
    {
        if (Object.HasInputAuthority && inGameUIHandler != null)
        {
            // Get some stats
            //inGameUIHandler.SetConnectionType(Runner.CurrentConnectionType.ToString());
            //inGameUIHandler.SetRtt($"RTT {Mathf.RoundToInt((float)Runner.GetPlayerRtt(Object.InputAuthority) * 100)} ms");
        }
    }

    public void ResetPlayer()
    {
        Vector3 newPosition = Utils.GetRandomSpawnPosition();

        if (Object.HasInputAuthority)
            inGameUIHandler.SetGameplayUIState(true);

        NetMaxHealth = 20f;
        NetMaxStamina = 20f;

        NetHealth = NetMaxHealth;
        NetStamina = NetMaxStamina;
        NetFoodEaten = 0;

        movementHandler.Reset();

        // Teleport player to new position
        GetComponent<NetworkRigidbody2D>().TeleportToPosition(newPosition);

        NetPlayerState = PlayerState.playing;
    }

    public override void Spawned()
    {
        Utils.DebugLog($"Player spawned, has input auth {Object.HasInputAuthority}");
        
        if (Object.HasInputAuthority)
        {
            Local = this;
            tag = "LocalPlayer";
            inGameUIHandler.SetJoinButtonState(true);
        }

        // Set the Player as player object
        Runner.SetPlayerObject(Object.InputAuthority, Object);

        // Make it easier to tell which player is which
        transform.name = $"P_{Object.Id}";
    }

    public void PlayerLeft(PlayerRef player)
    {
        if (player == Object.InputAuthority)
        {
            inGameUIHandler.mainCamera.GetComponent<AudioListener>().enabled = true;
            inGameUIHandler.playerListHandler.Players.Remove(this);
            Runner.Despawn(Object);
        }
    }

    // Player nickname OnChanged
    static void OnNickNameChanged(Changed<NetworkPlayer> changed)
    {
        changed.Behaviour.OnNickNameChanged();
    }

    void OnNickNameChanged()
    {
        Utils.DebugLog($"Nickname changed for player to {NetNickName} for player {gameObject.name}");

        playerNickNameTM.text = NetNickName.ToString();
    }

    // Player State OnChanged
    static void OnPlayerStateChanged(Changed<NetworkPlayer> changed)
    {
        Utils.DebugLog($"OnPlayerStateChanged to {changed.Behaviour.NetPlayerState}");

        changed.Behaviour.OnPlayerStateChanged();
    }

    void OnPlayerStateChanged()
    {
        if (NetPlayerState == PlayerState.playing)
        {
            // Focus camera on the new position
            if (Object.HasInputAuthority)
            {
                Camera.main.transform.position = transform.position;
                inGameUIHandler.SetGameplayUIState(true);
            }
            
            playerSpriteRenderer.gameObject.SetActive(true);
            movementHandler.enabled = true;
        }
        else
        {
            playerSpriteRenderer.gameObject.SetActive(false);
            movementHandler.enabled = false;
        }
    }

    public static void OnColorChanged(Changed<NetworkPlayer> changed)
    {
        changed.Behaviour.playerSpriteRenderer.color = changed.Behaviour.NetSpriteColor;
    }

    // Food Eaten OnChanged
    static void OnFoodEatenChanged(Changed<NetworkPlayer> changed)
    {
        Utils.DebugLog($"Food eaten!");

        changed.Behaviour.OnFoodEatenChanged();
    }

    void OnFoodEatenChanged()
    {
        if (NetHealth < NetMaxHealth) NetHealth += 50;

        if (NetFoodEaten == 100 && Object.HasInputAuthority)
        {
            RPC_SendSizeMessage(playerNickNameTM.text);
        }

        inGameUIHandler.HandleScoreboard();
    }

    // Player health OnChanged
    static void OnHealthChanged(Changed<NetworkPlayer> changed)
    {
        changed.Behaviour.OnHealthChanged();
    }

    static void OnMaxHealthChanged(Changed<NetworkPlayer> changed)
    {
        changed.Behaviour.OnHealthChanged();
    }

    void OnHealthChanged()
    {
        inGameUIHandler.SetHealth(NetHealth, NetMaxHealth);
    }

    // Player stamina OnChanged
    static void OnStaminaChanged(Changed<NetworkPlayer> changed)
    {
        changed.Behaviour.OnStaminaChanged();
    }

    static void OnMaxStaminaChanged(Changed<NetworkPlayer> changed)
    {
        changed.Behaviour.OnStaminaChanged();
    }

    void OnStaminaChanged()
    {
        inGameUIHandler.SetStamina(NetStamina, NetMaxStamina);
    }

    public void JoinGame(string nickName)
    {
        RPC_JoinGame(nickName);
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    void RPC_JoinGame(string nickName, RpcInfo info = default)
    {
        Utils.DebugLog($"[RPC] RPC_JoinGame {nickName}");

        this.NetNickName = nickName;
        NetSpriteColor = inGameUIHandler.pigColor;
        inGameUIHandler.playerListHandler.Players.Add(this);

        ResetPlayer();
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    public void RPC_SendSizeMessage(string playerName, RpcInfo info = default)
    {
        Utils.DebugLog($"[RPC] RPC_SendSizeMessage {playerName}");

        if (info.IsInvokeLocal)
            inGameUIHandler.SetGameText($"You have reached the maximum size!", 10f);
        else
            inGameUIHandler.SetGameText($"{playerName} has reached maximum size!", 10f);
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    public void RPC_PlayRunSound(NetworkPlayer player, RpcInfo info = default)
    {
        Utils.DebugLog($"[RPC] RPC_PlayRunSound");

        if (info.IsInvokeLocal)
            GetComponent<AudioSource>().Play();
        else
            player.GetComponent<AudioSource>().Play();
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    public void RPC_StopPlayingRunSound(NetworkPlayer player, RpcInfo info = default)
    {
        Utils.DebugLog($"[RPC] RPC_StopPlayingRunSound");

        if (info.IsInvokeLocal)
            GetComponent<AudioSource>().Stop();
        else
            player.GetComponent<AudioSource>().Stop();
    }
}
