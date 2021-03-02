using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private static PlayerData loadedData;

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

    public static void Save(PlayerData data = null)
    {
        if (data != null)
        {
            SaveManager.Save(data, "PlayerData");
            return;
        }

        SaveManager.Save(loadedData, "PlayerData");
    }

    public static PlayerData Load()
    {
        if (loadedData == null)
        {
            PlayerData file = (PlayerData)SaveManager.Load("PlayerData");

            if (file == null)
            {
                file = new PlayerData();
            }

            file.UpdateBuildVersion();

            loadedData = file;
        }

        return loadedData;
    }
}
