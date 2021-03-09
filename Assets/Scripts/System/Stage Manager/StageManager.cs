using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [System.Serializable]
    public class StartingPosition
    {
        public Transform playerPosition;
        public Transform cameraPosition;
    }

    //[System.Serializable]
    //public class StageItem
    //{
    //    [Tooltip("This will be used to cross reference the savefile if the item has been collected already.")]
    //    [SerializeField] private string stageItemID;
    //    public string StageItemID { get { return stageItemID; } }

    //    [SerializeField] private GameObject pickupItem;
    //    public GameObject PickupItem { get { return pickupItem; } }

    //    [SerializeField] private Transform spawnTransform;
    //    public Vector2 spawnPosition
    //    {
    //        get
    //        {
    //            Vector2 position = Vector2.zero;
    //            if (spawnTransform != null) { position = spawnTransform.position; }
    //            return position;
    //        }
    //    }
    //}

    [SerializeField] private StartingPosition defaultStartingPosition;

    [Space]

    [Tooltip("These positions will be used to place the player. It will grab a number called 'StartPosition' from the Game Manager.")]
    [SerializeField] private List<StartingPosition> startingPositions;
    private int selectedStartingPosition;

    [Space]

    [SerializeField] private List<StageEnemySpawner> enemySpawners = new List<StageEnemySpawner>();

    [Header("Player Settings")]
    [SerializeField] private GameObject player;
    private GameObject instantiatedPlayer;
    [SerializeField] private GameplayCamera gameplayCamera;
    [SerializeField] private GameplayUI gameplayUI;

    [Space]

    // Starting Information
    private StartingPosition startingPosition;
    private Vector2 playerSpawnLocation;
    private Vector2 cameraSpawnLocation;

    private void Awake()
    {

        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        if (instantiatedPlayer != null)
        {
            Debug.LogWarning("Player has already been created.");
            return;
        }
        else if (player == null)
        {
            Debug.LogError("There is no Player prefab connected to the Stage Manager.");
            return;
        }
        else
        {
            playerSpawnLocation = Vector2.zero;
            cameraSpawnLocation = Vector2.zero;

            // Choosing the Starting Position but checking if there is a list and if there's more than 0 entries.
            if (startingPositions != null && startingPositions.Count > 0)
            {
                // Setting default position
                startingPosition = defaultStartingPosition;

                if (GameManager.instance)
                {
                    // Getting data from the game manager
                    selectedStartingPosition = GameManager.instance.StartPosition;

                    // Setting start position
                    startingPosition = startingPositions[selectedStartingPosition];
                }

                playerSpawnLocation = startingPosition.playerPosition.position;
                cameraSpawnLocation = startingPosition.cameraPosition.position;
            }

            // Creating the player and placing them into the world.
            instantiatedPlayer = Instantiate(player, playerSpawnLocation, Quaternion.identity);
            instantiatedPlayer.name = "Player";

            // Moving Camera to position
            gameplayCamera.transform.position = cameraSpawnLocation;
            gameplayCamera.target = instantiatedPlayer.transform;

            // Set the UI to track the player's data
            gameplayUI.SetPlayer(instantiatedPlayer.GetComponent<TPB_Player>());
        }
    }

    public void SpawnEnemies()
    {

    }

    public void Save()
    {
        
    }
}
