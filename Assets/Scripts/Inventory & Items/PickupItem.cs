using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PickupItem : MonoBehaviour
{
    public UnityEvent onCollect;

    public void Collect()
    {
        onCollect?.Invoke();
        Destroy(gameObject);
    }
}
