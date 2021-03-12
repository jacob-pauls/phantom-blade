using System.Collections;
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
    [SerializeField] private bool findPlayer = true;
    [SerializeField] private Slider playerHealthBar;
    [SerializeField] private Slider playerEssenceBar;
    [SerializeField] private TextMeshProUGUI keyAmountTextUI;

    private bool isPlayerInitialized;
    private bool isBossInitialized;
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

        if (findPlayer)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<TPB_Character>();
        }

        if (player != null) { SetPlayer(player); }
        if (boss != null) { SetBoss(boss); }
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
            instance.player.onEssenceChange?.AddListener(UpdatePlayerEssenceBar);
            //player.onComboUpdate?.AddListener(UpdatePlayerHealthBar());
            instance.player.onItemCollected?.AddListener(UpdateKeyUI);
            instance.player.onItemUsed?.AddListener(UpdateKeyUI);
            instance.isPlayerInitialized = true;
        }

        instance.playerHealthBar.maxValue = player.maxHealth;
        instance.playerHealthBar.value = player.currentHealth;

        instance.playerEssenceBar.maxValue = player.maxEssence;
        instance.playerEssenceBar.value = player.currentEssence;

        UpdatePlayerHealthBar();
        UpdateKeyUI();
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
        Item key = GameManager.Load().Inventory.Get("new_opportunities"); //player.inventory.Get("new_opportunities");
        keyAmountTextUI.text = key != null ? key.CurrentStackAmount.ToString() : "0";
    }

    #endregion

    #region Boss Methods
    public void SetBoss(TPB_Character boss)
    {
        instance.boss = boss;

        if (!instance.isBossInitialized)
        {
            instance.boss.onHealthChange?.AddListener(UpdateBossHealthBar);
            //player.onComboUpdate?.AddListener(UpdatePlayerHealthBar());
            instance.isBossInitialized = true;
        }

        if (bossNameTextUI != null) { bossNameTextUI.text = boss.characterName; }

        instance.bossHealthBar.maxValue = boss.maxHealth;
        instance.bossHealthBar.value = boss.currentHealth;
    }

    public void UpdateBossHealthBar()
    {
        if (instance.bossHealthBar == null)
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
        Debug.Log("UpdateBossHealth -> " + boss.currentHealth);
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
