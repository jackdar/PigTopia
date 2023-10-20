using System.Collections;
using System.Collections.Generic;
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

    [Header("Canvases")]
    [SerializeField]
    Canvas joinGameCanvas;
    [SerializeField]
    Canvas pauseGameCanvas;
    [SerializeField]
    Canvas scoreboardCanvas;

    public List<NetworkPlayer> players;

    public Color pigColor = Color.white;

    void Start()
    {
        SetJoinButtonState(false);
        SetPauseMenuState(false);
        SetScoreboardState(false);
    }

    public void OnJoinGame()
    {
        Utils.DebugLog("OnJoinGame clicked");

        NetworkPlayer.Local.JoinGame(nameInputField.text);

        //Hide the join game canvas
        joinGameCanvas.gameObject.SetActive(false);
    }

    public void SetJoinButtonState(bool isEnabled)
    {
        joinGameButton.enabled = isEnabled;

        if (isEnabled)
            joinGameButton.GetComponentInChildren<TextMeshProUGUI>().text = "Join Game";
        else joinGameButton.GetComponentInChildren<TextMeshProUGUI>().text = "Connecting to server";
    }

    public void OnPauseGame() {
        SetPauseMenuState(!pauseGameCanvas.gameObject.activeSelf);
    }

    public void OnRestartGame()
    {
        SetPauseMenuState(!pauseGameCanvas.gameObject.activeSelf);
        NetworkPlayer.Local.ResetPlayer();
    }

    public bool GetPauseMenuState()
    {
        return pauseGameCanvas.gameObject.activeSelf;
    }
    
    public void SetPauseMenuState(bool isEnabled)
    {
        pauseGameCanvas.gameObject.SetActive(isEnabled);
    }

    public void SetScoreboardState(bool isEnabled)
    {
        scoreboardCanvas.gameObject.SetActive(isEnabled);
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

    public void HandleScoreboard()
    {
        scoreboardCanvas.GetComponentInChildren<TextMeshProUGUI>().text = "";

        foreach (NetworkPlayer np in players)
        {
            scoreboardCanvas.GetComponentInChildren<TextMeshProUGUI>().text += np.nickName + ": " + np.foodEaten + "\n";
        }
    }
}
