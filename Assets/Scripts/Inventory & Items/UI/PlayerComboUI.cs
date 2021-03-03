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

        [SerializeField] private Sprite comboDisplayImage;
        public Sprite ComboDisplayImage { get { return comboDisplayImage; } }
    }

    public TPB_Player player;
    private int testCurrentCombo; // Should be grabbed from the player character
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

            //player.onEnemyHit?.AddListener(UpdateComboUI);

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
        comboNameTextUI.gameObject.SetActive(testCurrentCombo < LowestCombo ? false : true);

        // Updating combo values to UI
        comboNameTextUI.text = GetCurrentComboMilestone?.ComboName;
        comboHitsRequiredTextUI.text = testCurrentCombo + " Hits";

        animator.SetTrigger("Play");
    }

    [ContextMenu("Test Combo By Increasing 1")]
    private void TestUpdateComboIncrease()
    {
        testCurrentCombo++;
        UpdateComboUI();
    }

    private int LowestCombo
    {
        get
        {
            int lowest = 0;

            for (int i = 0; i < comboMilestones.Count; i++)
            {
                if (i == 0 || comboMilestones[0].ComboHitsRequired < lowest)
                {
                    lowest = comboMilestones[i].ComboHitsRequired;
                }
            }

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

                if (testCurrentCombo >= comboMilestones[i].ComboHitsRequired)
                {
                    Debug.Log(testCurrentCombo);
                    combo = comboMilestones[i];
                }

            }

            return combo;
        }
    }
}
