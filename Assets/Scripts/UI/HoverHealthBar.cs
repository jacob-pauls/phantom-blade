using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HoverHealthBar : MonoBehaviour
{
    [SerializeField] private TPB_Character target;
    [SerializeField] private Slider slider;

    private void Start()
    {
        target.onHealthChange?.AddListener(UpdateBar);

        slider.maxValue = target.maxHealth;
        slider.value = target.currentHealth;
    }

    [ContextMenu("Update Health Bar Test")]
    public void UpdateBar()
    {
        if (slider == null)
        {
            Debug.Log("Please attach a slider component to " + name);
        }
        //else if() // If character script is not attached
        //{

        //}
        else
        {
            StopCoroutine(UpdateBarRoutine());
            StartCoroutine(UpdateBarRoutine());
        }
    }

    private IEnumerator UpdateBarRoutine()
    {
        float characterHealth = target.currentHealth; // Need to replace this with the Character.Health
        float currentHealth = slider.value;
        float progress = 0;

        float speed = 2; // Change this value to effect the bar speed

        while (progress < 1)
        {
            // Changing the displayed
            slider.value = Mathf.Lerp(currentHealth, characterHealth, progress);
            progress += Time.deltaTime * speed;
            yield return null;
        }

        slider.value = characterHealth;

    }
}
