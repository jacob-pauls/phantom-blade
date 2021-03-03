using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData : GameSaveFile
{
    //----------------------------------
    // DO NOT REMOVE OR CHANGE VARIABLE NAMES!
    //----------------------------------

    #region Variables

    // Inventory -- Do not change or add variables to the Inventory class
    private Inventory inventory;
    public Inventory Inventory
    {
        get
        {
            if (inventory == null) { inventory = new Inventory(); }
            return inventory;
        }
    }

    // Stages -- Only add new stages, do not remove stages
    public string lastStageName;
    #endregion

    public PlayerData()
    {
        Initialize();
    }

    public void Initialize()
    {
        // Add an inventory to the new PlayerData()
        inventory = new Inventory();

        UpdateBuildVersion();
    }

    public override void UpdateBuildVersion()
    {
        // Checking and updating build version
        int latestBuildVersion = 0;
        while (currentBuildVersion < latestBuildVersion)
        {
            switch (currentBuildVersion)
            {
                case 0:

                    break;

            }

            currentBuildVersion++;
        }
    }

    //public StageInfo GetStageInfo(string id)
    //{
    //    StageInfo stage = null;

    //    if (stages == null) { stages = new List<StageInfo>(); }

    //    for (int i = 0; i < stages.Count; i++)
    //    {

    //        if (id == stages[i].stageID)
    //        {
    //            stage = stages[i];
    //            break;
    //        }
    //    }

    //    if (stage == null)
    //    {
    //        Debug.LogWarning("Stage ID: " + id + " does not exist.");
    //    }

    //    return stage;
    //}

    //public void SetStageInfo(string id, float highscore)
    //{
    //    bool isSet = false;
    //    for (int i = 0; i < stages.Count; i++)
    //    {
    //        if (id == stages[i].stageID)
    //        {
    //            stages[i].highscore = highscore;
    //            isSet = true;
    //            break;
    //        }
    //    }

    //    if (!isSet)
    //    {
    //        Debug.LogWarning("Stage: " + id + " does not exist.");
    //    }
    //}
}
