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
    [SerializeField] private Item storedItem;

    public UnityEvent onCollect;

    public Item Collect()
    {
        onCollect?.Invoke();
        Destroy(gameObject);
        return storedItem;
    }
}
