using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [System.Serializable]
    public class StartingPosition
    {
        public Transform position;
        public Transform cameraPosition;
    }

    public List<StartingPosition> startingPositions;
}
