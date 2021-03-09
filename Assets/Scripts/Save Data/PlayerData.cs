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

    // Stage Items -- Do not add stages from the start. Stage Managers will be in charge of transferring information.
    private List<string> stageItemIDs = new List<string>();

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

    public bool IsStageItemPickedUp(string id)
    {
            bool isPickup = false;

            if (stageItemIDs.Count > 0)
            {
                for (int i = 0; i<stageItemIDs.Count; i++)
                {
                    if (id == stageItemIDs[i])
                    {
                        isPickup = true;
                        break;
                    }
                }
            }

            return isPickup;
    }

    public void AddStageItem(string stageItemID)
    {
        if (stageItemIDs == null) { stageItemIDs = new List<string>(); }
        stageItemIDs.Add(stageItemID);
        Debug.Log(stageItemID + " has been added to the stageItemIDs list.");
    }
}
