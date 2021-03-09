using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public enum ItemType
    {
        InstantConsumable,
        Consumable,
        Quest,
    }

    [System.Serializable]
    public class Attribute
    {
        [SerializeField] private string name;
        public string Name { get { return name; } }

        [SerializeField] private string value;
        public string GetValueAsString
        {
            get { return value; }
        }
        public int GetValueAsInt
        {
            get
            {
                int newValue = 0;
                bool canBeAnInt = int.TryParse(value, out newValue);

                if (!canBeAnInt) { Debug.Log("Cannot convert '" + value + "' into a int. Setting value to 0."); }

                return newValue;
            }
        }
        public float GetValueAsFloat
        {
            get
            {
                float newValue = 0;
                bool canBeAFloat = float.TryParse(value, out newValue);

                if (!canBeAFloat) { Debug.Log("Cannot convert '" + value + "' into a float. Setting value to 0."); }

                return newValue;
            }
        }
    }

    [SerializeField] private string name;
    public string Name { get { return name; } }

    [SerializeField] private string id;
    public string Id { get { return id; } }

    [SerializeField] private ItemType type;
    public ItemType Type { get { return type; } }

    [SerializeField] [TextArea] private string description;
    public string Description { get { return description; } }

    [Space]

    [SerializeField] private int currentStackAmount = 1;
    public int CurrentStackAmount
    {
        get
        {
            currentStackAmount = Mathf.Clamp(currentStackAmount, 0, MaximumStackAmount);
            return currentStackAmount;
        }
    }

    [SerializeField] private int maximumStackAmount = 1;
    public int MaximumStackAmount { get { return maximumStackAmount; } }

    [Space]

    [SerializeField] private List<Attribute> attributes = new List<Attribute>();
    public List<Attribute> Attributes { get { return attributes; } }

    public void ChangeStackAmount(int amount)
    {
        if (currentStackAmount == maximumStackAmount) { return; }
        currentStackAmount = Mathf.Clamp(currentStackAmount + amount, 0 , maximumStackAmount);
    }

    public void SetStackAmount(int amount)
    {
        currentStackAmount = Mathf.Clamp(amount, 0, maximumStackAmount);
    }

    public Sprite GetDisplayImage()
    {
        if (!ItemDatabase.instance)
        {
            Debug.LogWarning("There is no ItemDatabase, please make sure it's in the scene.");
            return null;
        }

        Sprite sprite = null;

        for (int i = 0; i < ItemDatabase.Items.Count; i++)
        {
            if (ItemDatabase.Items[i].Item.Id == id)
            {
                sprite = ItemDatabase.Items[i].DisplayImage;
                break;
            }
        }

        return sprite;
    }

    public GameObject GetPrefab()
    {
        if (!ItemDatabase.instance)
        {
            Debug.LogWarning("There is no ItemDatabase, please make sure it's in the scene.");
            return null;
        }

        GameObject prefab = null;

        for (int i = 0; i < ItemDatabase.Items.Count; i++)
        {
            if (ItemDatabase.Items[i].Item.Id == id)
            {
                prefab = ItemDatabase.Items[i].Prefab;
                break;
            }
        }

        return prefab;
    }

    public Attribute GetAttribute(string name, bool showWarning = true)
    {
        Attribute attribute = null;

        for (int i = 0; i < attributes.Count; i++)
        {
            if (attributes[i].Name == name)
            {
                attribute = attributes[i];
                break;
            }
        }

        if (showWarning && attribute == null)
        {
            Debug.LogWarning(name + " does not exist as an attribute. Check for spelling or see if it needs to be added.");
        }

        return attribute;
    }

    public void SetValues(string name, string id, ItemType type, string description, int currentStackAmount, int maximumStackAmount, /*Sprite displayImage, GameObject prefab,*/ List<Attribute> attributes)
    {
        this.name = name;
        this.id = id;
        this.type = type;
        this.description = description;
        this.currentStackAmount = currentStackAmount;
        this.maximumStackAmount = maximumStackAmount;
        //this.displayImage = displayImage;
        //this.prefab = prefab;
        this.attributes = attributes;
    }

    public Item() { }

    public Item(Item item)
    {
        name = item.name;
        id = item.id;
        type = item.type;
        description = item.description;
        currentStackAmount = item.currentStackAmount;
        maximumStackAmount = item.maximumStackAmount;
        //displayImage = item.displayImage;
        //prefab = item.prefab;
        attributes = item.attributes;
    }


}
