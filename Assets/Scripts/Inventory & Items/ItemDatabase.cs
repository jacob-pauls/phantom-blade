using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase instance;

    [SerializeField] private List<Item> items = new List<Item>();

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

    public static Item GetItem(string id)
    {
        Item item = null;

        for (int i = 0; i < instance.items.Count; i++)
        {
            if (id == instance.items[i].Id)
            {
                item = instance.items[i];
                break;
            }
        }

        return item;
    }
}
