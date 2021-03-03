using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private static PlayerData loadedData;

    // Load data
    private int startPosition;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void ResetLoadedData()
    {
        loadedData = new PlayerData();
    }

    public static void Save(PlayerData data = null)
    {
        if (data != null)
        {
            SaveManager.Save(data, "PlayerData");
            return;
        }

        SaveManager.Save(loadedData, "PlayerData");
    }

    public static PlayerData Load(bool getStoredFiles = false)
    {
        if (loadedData == null || getStoredFiles)
        {
            PlayerData file = (PlayerData)SaveManager.Load("PlayerData");

            if (file == null) { file = new PlayerData(); }

            file.UpdateBuildVersion();

            loadedData = file;
        }

        return loadedData;
    }

    // Selects stage by name, then checks 
    public static void LoadScene(string sceneName, int startPosition)
    {
        instance.startPosition = startPosition;
        SceneManager.LoadScene(sceneName);

        // Eventually may need to add a loading screen
    }
}
