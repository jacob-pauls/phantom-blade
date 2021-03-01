using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressKeyEvent : MonoBehaviour
{
    [SerializeField] private KeyCode keyCode;

    public UnityEvent onPressed;

    private void Update()
    {
        if (Input.GetKeyDown(keyCode))
        {
            onPressed?.Invoke();
        }
    }
}
