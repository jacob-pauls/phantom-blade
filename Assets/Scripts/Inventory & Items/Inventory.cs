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
        if (!ItemDatabase.instance)
        {
            Debug.LogError("There is no ItemDatabase. Add it to the scene.");
            return;
        }

        for (int i = 0; i < items.Count; i++)
        {
            if (id == items[i].Id)
            {
                // There is an item in the inventory

                ItemContainer container = ItemDatabase.GetItem(id);

                if (container == null)
                {
                    Debug.LogError(id + " is not in the ItemDatabase. Check for spelling or add it to the database.");
                    return;
                }

                GameObject drop = Object.Instantiate(container.Prefab, position, Quaternion.identity);
                drop.GetComponent<PickupItem>().SetStoredItem(container);
                break;
            }
        }
    }

    public void DropAll(Vector2 position, int amount = 1)
    {
        if (!ItemDatabase.instance)
        {
            Debug.LogError("There is no ItemDatabase. Add it to the scene.");
            return;
        }

        for (int i = 0; i < items.Count; i++)
        {
            // There is an item in the inventory

            ItemContainer container = ItemDatabase.GetItem(items[i].Id);

            if (container == null)
            {
                Debug.LogError("There is an item not in the ItemDatabase. Check for spelling or add it to the database.");
                continue;
            }

            GameObject drop = Object.Instantiate(container.Prefab, position, Quaternion.identity);
            drop.GetComponent<PickupItem>().SetStoredItem(container);

            float x = Random.Range(-3, 3);
            float y = 5;
            Vector2 force = new Vector2(x, y);
            
            // Drop items in vicinity of enemy
            Rigidbody2D rb2d = drop.GetComponent<Rigidbody2D>();
            if (rb2d != null) { rb2d.AddForce(force, ForceMode2D.Impulse); }
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
