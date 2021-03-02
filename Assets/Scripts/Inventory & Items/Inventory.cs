using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    [SerializeField] private List<Item> items = new List<Item>();

    public int ItemCount
    {
        get
        {
            int count = items.Count;
            return count;
        }
    }

    public void Add(Item item, int amount = 1)
    {
        bool isInList = false;

        for (int i = 0; i < items.Count; i++)
        {
            if (item.Id == items[i].Id)
            {
                items[i].ChangeStackAmount(Mathf.Abs(amount));
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
                items[i].ChangeStackAmount(-Mathf.Abs(amount));
                break;
            }
        }
    }

    public void RemoveAt(int index)
    {
        items.RemoveAt(index);
    }

    public void Drop(string id, Vector2 position, int amount = 1)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (id == items[i].Id)
            {
                GameObject drop = Object.Instantiate(items[i].Prefab, position, Quaternion.identity);
                drop.GetComponent<PickupItem>().SetStoredItem(items[i]);
                break;
            }
        }
    }

    public Item Get(string id)
    {
        Item item = null;

        for (int i = 0; i < items.Count; i++)
        {
            if (id == items[i].Id)
            {
                item = items[i];
                break;
            }
        }

        return item;
    }

    public Item Get(int index)
    {
        Item item = items[index];
        return item;
    }
}
