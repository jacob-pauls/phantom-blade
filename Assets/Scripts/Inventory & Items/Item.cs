using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Create New Item", order = 0)]
public class Item : ScriptableObject
{
    [SerializeField] private new string name;
    public string Name { get { return name; } }

    [SerializeField] [TextArea] private string description;
    public string Description { get { return description; } }

    [SerializeField] private Sprite displayImage;
    public Sprite DisplayImage { get { return displayImage; } }

    [SerializeField] private GameObject prefab;
    public GameObject Prefab { get { return prefab; } }

}
