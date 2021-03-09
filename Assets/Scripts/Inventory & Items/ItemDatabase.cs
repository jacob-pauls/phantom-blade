using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase instance;

    [SerializeField] private List<ItemContainer> items = new List<ItemContainer>();
    public static List<ItemContainer> Items
    {
        get
        {
            return instance.items;
        }
    }

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

    public static ItemContainer GetItem(string id)
    {
        ItemContainer item = null;

        for (int i = 0; i < instance.items.Count; i++)
        {
            if (id == instance.items[i].Item.Id)
            {
                item = instance.items[i];
                break;
            }
        }

        return item;
    }
}
