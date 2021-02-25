﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameplayUI : MonoBehaviour
{
    public static GameplayUI instance;

    [Header("Player")]
    [SerializeField] private TPB_Character player;
    [SerializeField] private Slider playerHealthBar;
    [SerializeField] private Slider playerEssenceBar;
    [SerializeField] private TextMeshProUGUI keyAmountTextUI;

    private bool isPlayerInitialized;
    //[SerializeField] private PlayerComboUI playerComboUI;

    [Header("Boss")]
    [SerializeField] private TPB_Character boss;
    [SerializeField] private TextMeshProUGUI bossNameTextUI;
    [SerializeField] private Slider bossHealthBar;

    // Private Properties
    private const float barAdjustmentSpeed = 2;

    private void Start()
    {
        instance = this;

        if (player != null) { SetPlayer(player); }
        if (boss == null) { bossHealthBar.gameObject.SetActive(false); }

        //boss.onDeath?.AddListener();
    }

    #region Player Methods
    public void SetPlayer(TPB_Character player)
    {
        instance.player = player;

        if (!instance.isPlayerInitialized)
        {
            instance.player.onHealthChange?.AddListener(UpdatePlayerHealthBar);
            instance.player.onEssenceChange?.AddListener(UpdatePlayerHealthBar);
            //player.onComboUpdate?.AddListener(UpdatePlayerHealthBar());
            instance.isPlayerInitialized = true;
        }

        instance.playerHealthBar.maxValue = player.maxHealth;
        instance.playerHealthBar.value = player.currentHealth;

        instance.playerEssenceBar.maxValue = player.maxEssence;
        instance.playerEssenceBar.value = player.currentEssence;
    }

    // The reason I use similar methods is for the StopCoroutine. It only stops the first one which can cause issues.
    // If I use StopAllCoroutines it will stop other bars from working.

    public void UpdatePlayerHealthBar()
    {
        if (instance.playerHealthBar == null)
        {
            Debug.LogWarning("Please attach a slider for the player's health");
        }
        else
        {
            StopCoroutine(UpdatePlayerHealthBarRoutine());
            StartCoroutine(UpdatePlayerHealthBarRoutine());
        }
    }

    private IEnumerator UpdatePlayerHealthBarRoutine()
    {
        float characterHealth = player.currentHealth; // Need to replace this with the Character.Health
        float currentHealth = playerHealthBar.value;
        float progress = 0;

        while (progress < 1)
        {
            // Changing the displayed
            playerHealthBar.value = Mathf.Lerp(currentHealth, characterHealth, progress);
            progress += Time.deltaTime * barAdjustmentSpeed;
            yield return null;
        }

        playerHealthBar.value = characterHealth;
        yield return null;
    }

    public void UpdatePlayerEssenceBar()
    {
        if (instance.playerEssenceBar == null)
        {
            Debug.LogWarning("Please attach a slider for the player's health");
        }
        else
        {
            StopCoroutine(UpdatePlayerEssenceBarRoutine());
            StartCoroutine(UpdatePlayerEssenceBarRoutine());
        }
    }

    private IEnumerator UpdatePlayerEssenceBarRoutine()
    {
        float characterEssence = player.currentEssence; // Need to replace this with the Character.Health
        float currentEssence = playerEssenceBar.value;
        float progress = 0;

        while (progress < 1)
        {
            // Changing the displayed
            playerEssenceBar.value = Mathf.Lerp(currentEssence, characterEssence, progress);
            progress += Time.deltaTime * barAdjustmentSpeed;
            yield return null;
        }

        playerEssenceBar.value = characterEssence;
        yield return null;
    }

    public void UpdateKeyUI()
    {
        // Get save data
        keyAmountTextUI.text = "0";
    }

    #endregion

    #region Boss Methods
    public void SetBoss(TPB_Character boss)
    {
        instance.boss = boss;

        if (!instance.isPlayerInitialized)
        {
            instance.boss.onHealthChange?.AddListener(UpdateBossHealthBar);
            //player.onComboUpdate?.AddListener(UpdatePlayerHealthBar());
            instance.isPlayerInitialized = true;
        }

        if (bossNameTextUI != null) { bossNameTextUI.text = boss.name; }

        instance.bossHealthBar.maxValue = player.maxHealth;
        instance.bossHealthBar.value = player.currentHealth;
    }

    public void UpdateBossHealthBar()
    {
        if (instance.playerHealthBar == null)
        {
            Debug.LogWarning("Please attach a slider for the player's health");
        }
        else
        {
            StopCoroutine(UpdateBossHealthBarRoutine());
            StartCoroutine(UpdateBossHealthBarRoutine());
        }
    }

    private IEnumerator UpdateBossHealthBarRoutine()
    {
        float characterHealth = boss.currentHealth; // Need to replace this with the Character.Health
        float currentHealth = bossHealthBar.value;
        float progress = 0;

        while (progress < 1)
        {
            // Changing the displayed
            bossHealthBar.value = Mathf.Lerp(currentHealth, characterHealth, progress);
            progress += Time.deltaTime * barAdjustmentSpeed;
            yield return null;
        }

        bossHealthBar.value = characterHealth;
        yield return null;
    }

    #endregion
}