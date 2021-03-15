using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * Jake Pauls
 * PlayerStatManager.cs
 * Defines prefs to bring health and essence between scenes
 */

public class PlayerStatManager : MonoBehaviour
{
    [SerializeField] private TPB_Player player;
    [SerializeField] private string firstSceneName;
    private LevelControl[] levelControl;
    private CountDownTimer cutsceneTimer;
    private Scene currentScene;
    private int healthPref;
    private int essencePref;

    void Awake()
    {
        levelControl = Object.FindObjectsOfType<LevelControl>();

        // Listen for scene loads and changes to apply health and essence
        if (levelControl != null) {
            foreach(LevelControl sceneCollider in levelControl) {
                sceneCollider.onSceneChange.AddListener(SavePlayerStatsOnSceneChange);
                sceneCollider.onSceneLoad.AddListener(GetPlayerStatsOnSceneLoad);
            } 
        }
    }

    public void SavePlayerStatsOnSceneChange() 
    {
        PlayerPrefs.SetInt("currentHealth", player.currentHealth);
        PlayerPrefs.SetInt("currentEssence", player.currentEssence);
        
        if (player.abilities.IsAbilityUnlocked(TPB_Ability_Controller.AbilityTypes.DoubleJump))
            PlayerPrefs.SetInt("DoubleJump", 1);
    }

    public void GetPlayerStatsOnSceneLoad()
    {
        currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == firstSceneName) {
            player.currentHealth = player.maxHealth;
            player.currentEssence = player.maxEssence;
            PlayerPrefs.SetInt("DoubleJump", 0);
        } else {
            Debug.Log("Getting player stats...");
            player.currentHealth = PlayerPrefs.GetInt("currentHealth", player.maxHealth);
            player.currentEssence = PlayerPrefs.GetInt("currentEssence", player.maxEssence);
            
            if (PlayerPrefs.GetInt("DoubleJump") == 1)
                player.abilities.UnlockAbility(TPB_Ability_Controller.AbilityTypes.DoubleJump);
        }
    }
}