using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro; 

public class tuichu2 : MonoBehaviour
{
    public Button exitButton;
    public GameObject confirmationPanel; 
    public Button confirmButton;
    public Button cancelButton;

    void Start()
    {
        exitButton.onClick.AddListener(ShowConfirmation);
        confirmButton.onClick.AddListener(ConfirmExit);
        cancelButton.onClick.AddListener(CancelExit);
        confirmationPanel.SetActive(false); 
    }

    void ShowConfirmation()
    {
        confirmationPanel.SetActive(true);
        Time.timeScale = 0f; 
    }

    void ConfirmExit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    void CancelExit()
    {
        confirmationPanel.SetActive(false);
        Time.timeScale = 1f; 
    }
}