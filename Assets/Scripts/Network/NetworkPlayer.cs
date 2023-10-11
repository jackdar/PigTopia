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
    public NetworkString<_16> nickName { get; set; }

    [Networked(OnChanged = nameof(OnPlayerStateChanged))]
    public PlayerState playerState { get; set; }

    public static NetworkPlayer Local { get; set; }

    // Other components
    InGameUIHandler inGameUIHandler;
    MovementHandler movementHandler;
    
    void Awake()
    {
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

    void ResetPlayer()
    {
        Vector3 newPosition = Utils.GetRandomSpawnPosition();

        // Teleport player to new position
        GetComponent<NetworkRigidbody2D>().TeleportToPosition(newPosition);

        playerState = PlayerState.playing;
    }

    public override void Spawned()
    {
        Utils.DebugLog($"Player spawned, has input auth {Object.HasInputAuthority}");

        if (Object.HasInputAuthority)
        {
            Local = this;

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
            Runner.Despawn(Object);
    }

    static void OnNickNameChanged(Changed<NetworkPlayer> changed)
    {
        changed.Behaviour.OnNickNameChanged();
    }

    void OnNickNameChanged()
    {
        Utils.DebugLog($"Nickname changed for player to {nickName} for player {gameObject.name}");

        playerNickNameTM.text = nickName.ToString();
    }

    static void OnPlayerStateChanged(Changed<NetworkPlayer> changed)
    {
        Utils.DebugLog($"OnPlayerStateChanged to {changed.Behaviour.playerState}");

        changed.Behaviour.OnPlayerStateChanged();
    }

    void OnPlayerStateChanged()
    {
        if (playerState == PlayerState.playing)
        {
            // Focus camera on the new position
            if (Object.HasInputAuthority)
                Camera.main.transform.position = transform.position;

            playerSpriteRenderer.gameObject.SetActive(true);
            movementHandler.enabled = true;
        }
        else
        {
            playerSpriteRenderer.gameObject.SetActive(false);
            movementHandler.enabled = false;
        }
    }

    public void JoinGame(string nickName)
    {
        RPC_JoinGame(nickName);
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    void RPC_JoinGame(string nickName, RpcInfo info = default)
    {
        Utils.DebugLog($"[RPC] RPC_JoinGame {nickName}");

        this.nickName = nickName;

        ResetPlayer();
    }
}
