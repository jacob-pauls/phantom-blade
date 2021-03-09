using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Create New Item", order = 0)]
public class ItemContainer : ScriptableObject
{
    [SerializeField] private Item item;
    public Item Item { get { return item; } }

    [SerializeField] private Sprite displayImage;
    public Sprite DisplayImage { get { return displayImage; } }

    [SerializeField] private GameObject prefab;
    public GameObject Prefab { get { return prefab; } }
}
