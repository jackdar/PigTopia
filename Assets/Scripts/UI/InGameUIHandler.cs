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

<<<<<<< Updated upstream
    [Header("Canvas")]
    [SerializeField]
    Canvas joinGameCanvas;
    [SerializeField]
    Canvas cameraCanvas;
    [SerializeField]
    Canvas pauseGameCanvas;

    public Color pigColor;
=======
    [Header("Text")]
    [SerializeField]
    TextMeshProUGUI gameText;
    [SerializeField]
    TextMeshProUGUI healthText;
    [SerializeField]
    TextMeshProUGUI staminaText;
    [SerializeField]
    TextMeshProUGUI scoreboardText;

    [Header("Canvases")]
    [SerializeField]
    Canvas joinGameCanvas;
    [SerializeField]
    Canvas pauseGameCanvas;
    [SerializeField] 
    Canvas scoreboardCanvas;

    [Header("Player List Handler")]
    [SerializeField]
    GameObject networkPlayerListHandler;

    public NetworkPlayerListHandler playerListHandler;

    public Color pigColor = Color.white;
>>>>>>> Stashed changes

    void Start()
    {
        playerListHandler = networkPlayerListHandler.GetComponent<NetworkPlayerListHandler>();

        SetJoinButtonState(false);
        SetPauseMenuState(false);
<<<<<<< Updated upstream
=======
        SetScoreboardState(false);
        SetGameTextState(false);
>>>>>>> Stashed changes
    }

    public void OnJoinGame()
    {
        Utils.DebugLog("OnJoinGame clicked");

        NetworkPlayer.Local.JoinGame(nameInputField.text);

        pigColor = Color.white;

        HandleScoreboard();

        SetGameTextState(true);

        //Hide the join game canvas
        gameObject.SetActive(false);
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

    public void OnPauseGame() {
        SetPauseMenuState(!cameraCanvas.gameObject.activeSelf);
    }

    public bool GetPauseMenuState()
    {
        return cameraCanvas.gameObject.activeSelf;
    }
    
    public void SetPauseMenuState(bool isEnabled)
    {
<<<<<<< Updated upstream
        cameraCanvas.gameObject.SetActive(isEnabled);
    }
=======
        SetGameTextState(!isEnabled);
        Utils.DebugLog($"Setting pauseGameCanvas active to {isEnabled}!");
        pauseGameCanvas.gameObject.SetActive(isEnabled);
    }

    public void SetScoreboardState(bool isEnabled)
    {
        scoreboardCanvas.gameObject.SetActive(isEnabled);
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
        healthText.text = "Health: " + health + "/" + maxHealth;
    }

    public void SetStamina(ushort stamina, ushort maxStamina)
    {
        staminaText.text = "Stamina: " + stamina + "/" + maxStamina;
    }

>>>>>>> Stashed changes
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

<<<<<<< Updated upstream
=======
    void ClearGameText()
    {
        gameText.text = "";
    }
>>>>>>> Stashed changes
}
