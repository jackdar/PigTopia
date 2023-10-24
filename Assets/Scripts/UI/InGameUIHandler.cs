using System.Collections;
using System.Collections.Generic;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIHandler : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField]
    Button joinGameButton;
    [SerializeField]
    Button resumeButton;
    [SerializeField]
    Button settingsButton;
    [SerializeField]
    Button exitButton;

    [Header("Colour Buttons")]
    [SerializeField]
    Button pinkButton;
    [SerializeField]
    Button redButton;
    [SerializeField]
    Button blueButton;
    [SerializeField]
    Button yellowButton;

    [Header("Input")]
    [SerializeField] 
    TMP_InputField nameInputField;

    [Header("Text")]
    [SerializeField]
    TextMeshProUGUI gameText;
    [SerializeField]
    TextMeshProUGUI scoreboardText;

    [Header("UI Bars")]
    [SerializeField]
    Canvas healthBar;
    [SerializeField]
    Canvas staminaBar;

    [Header("Canvases")]
    [SerializeField]
    Canvas joinGameCanvas;
    [SerializeField]
    Canvas pauseGameCanvas;
    [SerializeField]
    Canvas pauseButtonCanvas;
    [SerializeField]
    Canvas pauseSettingsCanvas;
    [SerializeField] 
    Canvas gameplayCanvas;

    [Header("Player List Handler")]
    [SerializeField]
    GameObject networkPlayerListHandler;

    [Header("Camera")]
    [SerializeField]
    Camera mainCamera;

    public NetworkPlayerListHandler playerListHandler;

    bool muteToggled = false;

    public Color pigColor = Color.white;

    private string _nickName = null;
    
    void Start()
    {
        playerListHandler = networkPlayerListHandler.GetComponent<NetworkPlayerListHandler>();

        SetJoinButtonState(false);
        SetPauseMenuState(false);
        SetGameplayUIState(false);
        SetGameTextState(false);
    }

    public void OnJoinGame()
    {
        Utils.DebugLog("OnJoinGame clicked");

        NetworkPlayer.Local.JoinGame(nameInputField.text);

        mainCamera.GetComponent<AudioListener>().enabled = false;
        NetworkPlayer.Local.GetComponent<AudioListener>().enabled = true;

        pigColor = Color.white;

        HandleScoreboard();

        SetGameTextState(true);

        SetNickName(nameInputField.text);
        
        //Hide the join game canvas
        joinGameCanvas.gameObject.SetActive(false);
    }

    public void HandleScoreboard()
    {
        scoreboardText.text = "";

        foreach (NetworkPlayer np in playerListHandler.Players)
        {
            scoreboardText.text += np.NetNickName + ": " + np.NetFoodEaten + "\n";
        }
    }

    public void SetJoinButtonState(bool isEnabled)
    {
        joinGameButton.enabled = isEnabled;

        if (isEnabled)
            joinGameButton.GetComponentInChildren<TextMeshProUGUI>().text = "Join Game";
        else joinGameButton.GetComponentInChildren<TextMeshProUGUI>().text = "Connecting to server";
    }

    public void OnRestartGame()
    {
        if (playerListHandler.Players.Contains(NetworkPlayer.Local))
        {
            playerListHandler.Players.Get(playerListHandler.Players.IndexOf(NetworkPlayer.Local)).ResetPlayer();
            OnPauseGame();
        }
    }

    public void OnPauseGame() {
        SetPauseMenuState(!pauseGameCanvas.gameObject.activeSelf);
    }

    public bool GetPauseMenuState()
    {
        return pauseGameCanvas.gameObject.activeSelf;
    }
    
    public void SetPauseMenuState(bool isEnabled)
    {
        SetGameTextState(!isEnabled);
        Utils.DebugLog($"Setting pauseGameCanvas active to {isEnabled}!");
        pauseButtonCanvas.gameObject.SetActive(isEnabled);
        pauseSettingsCanvas.gameObject.SetActive(false);
        pauseGameCanvas.gameObject.SetActive(isEnabled);
    }

    public void OnPauseSettingsMenu()
    {
        pauseButtonCanvas.gameObject.SetActive(!pauseButtonCanvas.gameObject.activeSelf);
        pauseSettingsCanvas.gameObject.SetActive(!pauseSettingsCanvas.gameObject.activeSelf);
    }

    public void SetGameplayUIState(bool isEnabled)
    {
        gameplayCanvas.gameObject.SetActive(isEnabled);
    }

    public bool GetGameTextState()
    {
        return gameText.gameObject.activeSelf;
    }

    public void SetGameTextState(bool isEnabled)
    {
        gameText.gameObject.SetActive(isEnabled);
    }

    public void SetGameText(string gameTextString, float time)
    {
        gameText.text = gameTextString;
        Invoke("ClearGameText", time);
    }

    public void SetHealth(ushort health, ushort maxHealth)
    {
        // TODO
    }

    public void SetStamina(ushort stamina, ushort maxStamina)
    {
        // TODO
    }

    public void OnExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void HandleColour(string color)
    {
        if (color == "pink") pigColor = new Color(1f, 1f, 1f, 1f);
        if (color == "red") pigColor = new Color(1f, 0.35f, 0.35f, 1f);
        if (color == "blue") pigColor = new Color(0.35f, 1f, 1f, 1f);
        if (color == "yellow") pigColor = new Color(0.75f, 1f, 0.15f, 1f);
    }
    
    void ClearGameText()
    {
        gameText.text = "";
    }

    public void OnMuteToggled()
    {
        muteToggled = !muteToggled;
        if (muteToggled)
        {
            AudioListener.volume = 0f;
        }
        else
        {
            AudioListener.volume = 1f;
        }
    }

    public void SetNickName(string nickName)
    {
        _nickName = nickName;
    }
    
    public string GetNickName()
    {
        if (string.IsNullOrWhiteSpace(_nickName))
        {
            _nickName = "Test Data";
        }

        return _nickName;
    }
}
