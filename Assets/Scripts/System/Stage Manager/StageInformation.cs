using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StageInformation
{
    [System.Serializable]
    public class StageInformationItem
    {
        [Tooltip("Do not confuse this as the item's ID. This is the Stage's item ID. Meaning this ID will be used to track this specific stage item.")]
        [SerializeField] private string stageItemID;
        public string StageItemID { get { return stageItemID; } }
        [HideInInspector] public bool isAlreadyPickedUp;
    }

    [Tooltip("This ID will be used to store and find information to the savefile.")]
    [SerializeField] private string stageID;
    public string StageID { get { return stageID; } }

    [Tooltip("Used to figure out if the item needs to be spawned.")]
    public List<StageInformationItem> stageItems = new List<StageInformationItem>();

    public StageInformationItem GetStageItem(string id)
    {
        StageInformationItem stageItem = null;

        for (int i = 0; i < stageItems.Count; i++)
        {
            if (id == stageItems[i].StageItemID)
            {
                stageItem = stageItems[i];
                break;
            }
        }

        return stageItem;
    }

    public StageInformation(string id, List<StageInformationItem> items)
    {
        stageID = id;
        stageItems = items;
    }

    public StageInformation(StageInformation information)
    {
        stageID = information.stageID;
        stageItems = information.stageItems;
    }

    public StageInformation() { }
}
