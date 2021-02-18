using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerComboUI : MonoBehaviour
{
    [System.Serializable]
    public class ComboMilestones
    {
        [SerializeField] private string comboName;
        public string ComboName { get { return comboName; } }
        [SerializeField] private int comboHitsRequired;
        public int ComboHitsRequired { get { return comboHitsRequired; } }
    }

    public TPB_Character player;
    private int currentCombo; // Should be grabbed from the player character
    [Space]
    [SerializeField] private TextMeshProUGUI comboNameTextUI;
    [SerializeField] private TextMeshProUGUI comboHitsRequiredTextUI;
    [Space]
    [SerializeField] private List<ComboMilestones> comboMilestones = new List<ComboMilestones>();

    private Animator animator;
    private int numberOfHits;

    private bool isInitialized;

    public void Initialize()
    {
        if (!isInitialized)
        {
            animator = GetComponent<Animator>();

            //player.onComboHit?.AddListener(Combo)

            isInitialized = true;
        }
    }

    private void Start()
    {
        Initialize();
    }

    [ContextMenu("Update Combo UI")]
    public void UpdateComboUI()
    {
        // Deciding if there should be a combo name appearing.
        comboNameTextUI.gameObject.SetActive(currentCombo < LowestCombo ? false : true);

        // Updating combo values to UI
        comboNameTextUI.text = GetCurrentComboMilestone.ComboName;
        comboHitsRequiredTextUI.text = currentCombo + " Hits";

        animator.SetTrigger("Play");
    }

    [ContextMenu("Test Combo By Increasing 1")]
    private void TestUpdateComboIncrease()
    {
        currentCombo++;
        UpdateComboUI();
    }

    private int LowestCombo
    {
        get
        {
            int lowest = 0;
            for (int i = 0; i < comboMilestones.Count; i++)
            {
                if (i == 0 || lowest > comboMilestones[0].ComboHitsRequired)
                {
                    lowest = comboMilestones[i].ComboHitsRequired;
                    Debug.Log("Hits " + comboMilestones[i].ComboHitsRequired);
                }
            }
            Debug.Log("Lowest Required Combo: " + lowest);
            return lowest;
        }
    }

    private ComboMilestones GetCurrentComboMilestone
    {
        get
        {
            ComboMilestones combo = null;

            for (int i = 0; i < comboMilestones.Count; i++)
            {
                if (currentCombo < comboMilestones[i].ComboHitsRequired)
                {
                    combo = comboMilestones[i];
                    break;
                }
            }

            return combo;
        }
    }
}
