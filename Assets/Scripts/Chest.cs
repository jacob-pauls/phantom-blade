using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    [SerializeField] private bool isReopenable;

    [Header("Item Drop Settings")]
    [SerializeField] private Transform dropPosition;
    [Space]
    [SerializeField] private DropType dropType;
    [SerializeField] private float dropStartDelay = 1;
    [SerializeField] private float dropDelayBetweenItems = 0.5f;
    [Space]
    [SerializeField] private float xShootRange = 5;
    [SerializeField] private float yShootAmount = 10;
    [Space]
    public UnityEvent onOpen;
    public UnityEvent onDrop;
    public UnityEvent onLocked;

    private bool isOpenedOnce;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    [ContextMenu("Open Chest")]
    public void Open()
    {
        bool isKeyAvailable = false;
        if (doesRequiresKey && !isKeyAvailable)
        {
            onLocked?.Invoke();
            return;
        }

        if (isOpenedOnce && !isReopenable) { return; }

        animator.SetTrigger("Open");

        switch (dropType)
        {
            case DropType.DropOneItemAtRandom:
                int randomIndex = Random.Range(0, items.Count);
                Drop(randomIndex);
                break;
            case DropType.DropAllItems:
                DropAll();
                break;
        }

        onOpen?.Invoke();
        isOpenedOnce = true;
    }

    /// <summary>
    /// This allows you to drop 1 item from the list.
    /// The 'index' is the position of the item in the list. It starts on 0.
    /// </summary>
    private void Drop(int index = 0)
    {
        if (index > items.Count)
        {
            Debug.LogWarning("The index (" + index + ") is bigger than the list count (" + items.Count + "). Stopping Drop() method.");
            return;
        }
        else if (dropPosition == null)
        {
            Debug.LogWarning("The index (" + index + ") does not have a dropPosition attached. Stopping Drop() method.");
            return;
        }
        else if (items[index] == null)
        {
            Debug.LogWarning("The index (" + index + ") does not have a gameObject attached. Stopping Drop() method.");
            return;
        }
        else if (items[index] == GetComponent<PickupItem>())
        {
            Debug.LogWarning("The index (" + index + ") does not have a PickupItem component. Stopping Drop() method.");
            return;
        }
        else if (items[index] == GetComponent<Rigidbody2D>())
        {
            Debug.LogWarning("The index (" + index + ") does not have a Rigidbody2D component. Stopping Drop() method.");
            return;
        }
        else
        {
            GameObject pickup = Instantiate(items[index], dropPosition.position, Quaternion.identity);

            float x = Random.Range(-xShootRange, xShootRange);
            float y = yShootAmount;
            Vector2 force = new Vector2(x, y);

            pickup.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);

            items.RemoveAt(index);
        }
    }

    public void DropAll()
    {
        StartCoroutine(DropAllRoutine());
    }

    private IEnumerator DropAllRoutine()
    {
        while (items.Count > 0)
        {
            Drop();
            yield return new WaitForSeconds(dropDelayBetweenItems);
        }
    }
}
