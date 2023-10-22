using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    [Header("Canvas")]
    [SerializeField]
    Canvas joinGameCanvas;
    [SerializeField]
    Canvas cameraCanvas;
    [SerializeField]
    Canvas pauseGameCanvas;

    public Color pigColor;

    public static bool isPauseMenuVisible = false;
    public GameObject pauseMenuUI;


    void Start()
    {
        SetJoinButtonState(false);
        pauseGameCanvas.gameObject.SetActive(false);
        //SetPauseMenuState(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPauseMenuVisible)
            {
                Resume();
            }
            else
            {
                Pause();
            }

            isPauseMenuVisible = !isPauseMenuVisible;
            pauseGameCanvas.gameObject.SetActive(!pauseGameCanvas.gameObject.activeSelf);     
        }
    }

    void Resume()
    {
        pauseMenuUI.SetActive(false);
        isPauseMenuVisible = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        isPauseMenuVisible = true;
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

        SetPauseMenuState(!GetPauseMenuState());  
    }

    public bool GetPauseMenuState()
    {
        return cameraCanvas.gameObject.activeSelf;
    }
    
    public void SetPauseMenuState(bool isEnabled)
    {
        pauseGameCanvas.gameObject.SetActive(isEnabled);
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

}
