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
    Button resumeGameButton;
    [SerializeField]
    Button settingsGameButton;
    [SerializeField]
    Button quitGameButton;

    //[Header("Texts")]
    //[SerializeField]
    //TextMeshProUGUI statusText;
    //
    //[Header("Stats")]
    //[SerializeField]
    //TextMeshProUGUI connectionTypeText;
    //
    //[SerializeField]
    //TextMeshProUGUI rttText;

    [Header("Input")]
    [SerializeField] 
    TMP_InputField nameInputField;

    [Header("Join Canvas")]
    [SerializeField] 
    Canvas joinGameCanvas;
    [SerializeField]
    Canvas pauseGameCanvas;

    void Start()
    {
        //statusText.gameObject.SetActive(false);

        SetJoinButtonState(false);
        pauseGameCanvas.gameObject.SetActive(false);

    }

    //public void SetConnectionType(string type)
    //{
    //    connectionTypeText.text = $"Connection type: {type}"; 
    //}
    //
    //public void SetRtt(string rtt)
    //{
    //    rttText.text = rtt;
    //}


    public void OnJoinGame()
    {
        Utils.DebugLog("OnJoinGame clicked");

        NetworkPlayer.Local.JoinGame(nameInputField.text);

        //Hide the join and pause game canvas
        joinGameCanvas.gameObject.SetActive(false);
        

        //Show the status text
        //statusText.gameObject.SetActive(true);
    }

    public void SetJoinButtonState(bool isEnabled)
    {
        joinGameButton.enabled = isEnabled;

        if (isEnabled)
            joinGameButton.GetComponentInChildren<TextMeshProUGUI>().text = "Join Game";
        else joinGameButton.GetComponentInChildren<TextMeshProUGUI>().text = "Connecting to server";
    }

    public void OnGamePaused()
    {
        if (pauseGameCanvas.gameObject.activeSelf)
        {
            SetPauseMenuState(false);
        }
        else
        {
            SetPauseMenuState(true);
        }
    }

    public void SetPauseMenuState(bool isEnabled)
    {
        pauseGameCanvas.gameObject.SetActive(isEnabled);
    }
}
