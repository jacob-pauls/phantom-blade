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
    private Scene currentScene;
    private int healthPref;
    private int essencePref;

    void Awake()
    {
        levelControl = Object.FindObjectsOfType<LevelControl>();
        
        // Listen for scene loads and changes to apply health and essence
        foreach(LevelControl sceneCollider in levelControl) {
            sceneCollider.onSceneChange.AddListener(SetHealthAndEssenceOnSceneChange);
            sceneCollider.onSceneLoad.AddListener(GetHealthAndEssenceOnSceneLoad);
        } 
    }

    public void SetHealthAndEssenceOnSceneChange() 
    {
        PlayerPrefs.SetInt("currentHealth", player.currentHealth);
        PlayerPrefs.SetInt("currentEssence", player.currentEssence);
    }

    public void GetHealthAndEssenceOnSceneLoad()
    {
        currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == firstSceneName) {
            Debug.Log("Max");
            player.currentHealth = player.maxHealth;
            player.currentEssence = player.maxEssence;
        } else {
            player.currentHealth = PlayerPrefs.GetInt("currentHealth", player.maxHealth);
            player.currentEssence = PlayerPrefs.GetInt("currentEssence", player.maxEssence);
        }
    }
}