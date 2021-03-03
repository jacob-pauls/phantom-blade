using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string startSceneName;
    [SerializeField] private Button continueButton;

    private void Awake()
    {
        PlayerData data = GameManager.Load();

        if (data.lastStageName == null || data.lastStageName == string.Empty)
        {
            continueButton.gameObject.SetActive(false);
        }
    }

    public void NewGame()
    {
        // Resets the temp files to a brand new one.
        GameManager.ResetLoadedData();

        SceneManager.LoadScene(startSceneName);
    }

    public void Continue()
    {
        PlayerData data = GameManager.Load();

        SceneManager.LoadScene(data.lastStageName);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
