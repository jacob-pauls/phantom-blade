using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private List<Item> items = new List<Item>();

    public void Add(Item item, int amount = 1)
    {
        bool isInList = false;

        for (int i = 0; i < items.Count; i++)
        {
            if (item.Id == items[i].Id)
            {
                items[i].ChangeStackAmount(amount);
                isInList = true;
                return;
            }
        }

        if (!isInList)
        {
            item.SetStackAmount(amount);

            // Add it to the list
            items.Add(item);
        }
    }

    public void Remove(string id, int amount = 1)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (id == items[i].Id)
            {

                break;
            }
        }
    }
}
