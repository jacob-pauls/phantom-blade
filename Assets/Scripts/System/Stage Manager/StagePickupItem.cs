using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StagePickupItem : PickupItem
{
    [SerializeField] private string stageItemId;

    private void Start()
    {
        if (GameManager.instance != null)
        {
            Debug.Log("Checking item");
            // If the item is already pickup up, turn this item off
            if (GameManager.Load().IsStageItemPickedUp(stageItemId))
            {
                Debug.Log("Turn off item");
                gameObject.SetActive(false);
            }
        }
    }

    public override Item Collect()
    {
        if (storedItem.Item == null)
        {
            Debug.LogWarning("There is no Item attached to the storedItem.");
            return null;
        }

        GameManager.Load().AddStageItem(stageItemId);

        GameManager.Save();

        return base.Collect();
    }
}
