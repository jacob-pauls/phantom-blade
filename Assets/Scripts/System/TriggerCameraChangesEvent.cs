using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCameraChangesEvent : MonoBehaviour
{
    [SerializeField] private float minX = -10, maxX = 10;
    [SerializeField] private float minY = 0, maxY = 10;

    private void Start()
    {
        if (GetComponent<Collider2D>() == null)
        {
            Debug.LogWarning(name + " does NOT have a Collider2D. Please add one to the gameobject.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameplayCamera.SetCameraBoundaries(minX, maxX, minY, maxY);
        }
    }
}
