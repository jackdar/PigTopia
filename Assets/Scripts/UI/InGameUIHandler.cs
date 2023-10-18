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

    [Header("Input")]
    [SerializeField] 
    TMP_InputField nameInputField;

    [Header("Canvas")]
    [SerializeField]
    Canvas joinGameCanvas;
    [SerializeField]
    Canvas cameraCanvas;
    [SerializeField]
    Canvas pauseGameCanvas;

    void Start()
    {
        SetJoinButtonState(false);
        SetPauseMenuState(false);
    }

    public void OnJoinGame()
    {
        Utils.DebugLog("OnJoinGame clicked");

        NetworkPlayer.Local.JoinGame(nameInputField.text);

        //Hide the join game canvas
        gameObject.SetActive(false);
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
        cameraCanvas.gameObject.SetActive(isEnabled);
    }
    public void OnExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
