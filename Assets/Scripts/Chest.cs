using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Needs a pool of items
// Character will be responible for opening chests
// Add the ability to have keys unlock the chests
public class Chest : MonoBehaviour
{
    public enum DropType
    {
        DropOneItemAtRandom,
        DropAllItems,
    }

    [SerializeField] private List<GameObject> items = new List<GameObject>();

    [Header("Conditions")]
    [SerializeField] private bool doesRequiresKey;

    [Header("Item Drop Settings")]
    [SerializeField] private DropType dropType;
    [SerializeField] private float dropStartDelay = 1;
    [SerializeField] private float dropDelayBetweenItems = 0.5f;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Open(bool isKeyAvailable = false)
    {
        if (doesRequiresKey && !isKeyAvailable) { return; }

        animator.SetTrigger("Open");

        switch (dropType)
        {
            case DropType.DropOneItemAtRandom:

                break;
            case DropType.DropAllItems:

                break;
            default:
                break;
        }
    }

    private void Drop()
    {
        
    }

    public void DropAll()
    {

    }
}
