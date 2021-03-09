using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 1. Ensure the layer is set to Item.
/// 2. 'storedItem' cannot be null.
/// 3. Use Collect() in characters that can pick up items. It will return 'storedItem'.
/// </summary>
public class PickupItem : MonoBehaviour
{
    [SerializeField] protected ItemContainer storedItem;

    public UnityEvent onCollect;

    public void SetStoredItem(ItemContainer item)
    {
        storedItem = item;
    }

    public virtual Item Collect()
    {
        onCollect?.Invoke();

        Item newItem = new Item(); // ScriptableObject.CreateInstance<Item>();
        //newItem.name = storedItem.name;
        newItem.SetValues(storedItem.Item.Name,
            storedItem.Item.Id,
            storedItem.Item.Type,
            storedItem.Item.Description,
            storedItem.Item.CurrentStackAmount,
            storedItem.Item.MaximumStackAmount,
            //storedItem.DisplayImage,
            //storedItem.Prefab,
            storedItem.Item.Attributes);

        return newItem;
    }
}
