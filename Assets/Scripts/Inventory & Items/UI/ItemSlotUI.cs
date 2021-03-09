using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    [SerializeField] private string itemID;
    [SerializeField] private Image displayImage;
    [SerializeField] private TextMeshProUGUI amountTextUI;

    public void UpdateItemSlot()
    {
        if (GameManager.instance == null && ItemDatabase.instance == null)
        {
            Debug.LogWarning("GameManager.instance or ItemDatabase.instance is null.");
            return;
        }

        // Find item in inventory
        Item item = GameManager.Load().Inventory.Get(itemID);

        // If it's not in inventory, find it in the Database
        if (item == null)
        {
            item = ItemDatabase.GetItem(itemID).Item;
            item.SetStackAmount(0);
        }

        displayImage.sprite = item.GetDisplayImage();
        amountTextUI.text = item.CurrentStackAmount.ToString();
    }
}
