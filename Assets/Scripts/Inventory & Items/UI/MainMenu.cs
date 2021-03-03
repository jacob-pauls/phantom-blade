using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string startSceneName;


    public void NewGame()
    {
        // Resets the temp files to a brand new one.
        GameManager.ResetLoadedData();

        SceneManager.LoadScene(startSceneName);
    }

    public void Continue()
    {
        PlayerData data = GameManager.Load();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
