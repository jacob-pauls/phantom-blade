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

    [SerializeField] private Inventory inventory;

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

    public void Open(bool isKeyAvailable = false)
    {
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
                int randomIndex = Random.Range(0, inventory.ItemCount);
                StartCoroutine(DropRoutine(randomIndex));
                break;
            case DropType.DropAllItems:
                StartCoroutine(DropAllRoutine());
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
        if (index > inventory.ItemCount)
        {
            Debug.LogWarning("The index (" + index + ") is bigger than the list count (" + inventory.ItemCount + "). Stopping Drop() method.");
            return;
        }
        else if (dropPosition == null)
        {
            Debug.LogWarning("The index (" + index + ") does not have a dropPosition attached. Stopping Drop() method.");
            return;
        }
        else
        {
            GameObject pickup = Instantiate(inventory.Get(index).GetPrefab(), dropPosition.position, Quaternion.identity);

            float x = Random.Range(-xShootRange, xShootRange);
            float y = yShootAmount;
            Vector2 force = new Vector2(x, y);

            pickup.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);

            inventory.RemoveAt(index);

            animator.SetTrigger("Drop");
        }
    }

    private IEnumerator DropRoutine(int index)
    {
        yield return new WaitForSeconds(dropStartDelay);
        Drop(index);
    }

    private IEnumerator DropAllRoutine()
    {
        yield return new WaitForSeconds(dropStartDelay);

        while (inventory.ItemCount > 0)
        {
            Drop();
            yield return new WaitForSeconds(dropDelayBetweenItems);
        }
    }
}
