using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayCamera : MonoBehaviour
{
    private enum UpdateMode
    {
        Update,
        FixedUpdate,
    }

    public static GameplayCamera instance;

    public Transform target;
    [Space]
    [SerializeField] private Transform pivot;
    [SerializeField] private new Transform camera;
    [Space]
    [SerializeField] private UpdateMode updateMode = UpdateMode.FixedUpdate;
    [SerializeField] private float distanceInFrontOfTarget = 2;
    [SerializeField] private float zOffset = -10;
    [SerializeField] private float cameraMoveSpeed = 3;
    [Space]
    [SerializeField] private bool useBoundaries;
    [SerializeField] private float minX = -10, maxX = 10;
    [SerializeField] private float minY = 0, maxY = 10;

    private static bool isShaking;
    private float duration;
    private float distance;

    private void Awake()
    {
        instance = this;
    }

    private void OnDestroy()
    {
        instance = null;
    }

    private void FixedUpdate()
    {
        if (updateMode == UpdateMode.FixedUpdate)
        {
            MoveToTarget();
        }
    }

    private void Update()
    {
        if (updateMode == UpdateMode.Update)
        {
            MoveToTarget(UpdateMode.Update);
        }
    }

    private void MoveToTarget(UpdateMode mode = UpdateMode.FixedUpdate)
    {
        if (target)
        {
            Vector3 position = target.position;
            position.z = zOffset;

            if (target.localScale.x > 0) { position.x = target.position.x + distanceInFrontOfTarget; }
            if (target.localScale.x < 0) { position.x = target.position.x - distanceInFrontOfTarget; }

            float time = 0;
            switch (updateMode)
            {
                case UpdateMode.Update:
                    time = Time.deltaTime;
                    break;
                case UpdateMode.FixedUpdate:
                    time = Time.fixedDeltaTime;
                    break;
            }

            if (useBoundaries)
            {
                position.x = Mathf.Clamp(position.x, minX, maxX);
                position.y = Mathf.Clamp(position.y, minY, maxY);
            }

            transform.position = Vector3.Lerp(transform.position, position, time * cameraMoveSpeed);

        }
    }

    public static void SetCameraBoundaries(float minX, float maxX, float minY, float maxY)
    {
        instance.minX = minX;
        instance.maxX = maxX;
        instance.minY = minY;
        instance.maxY = maxY;
    }

    [ExecuteInEditMode]
    private void OnDrawGizmos()
    {
        if (useBoundaries)
        {
            Gizmos.color = Color.green;
            Vector3 center = new Vector3((minX + maxX) / 2, (minY + maxY) / 2);
            Vector3 size = new Vector3(Mathf.Abs(minX - maxX), Mathf.Abs(minY - maxY));
            Gizmos.DrawWireCube(center, size);
        }
    }

    // Duration is how long the shake will last
    //public static void Shake(float duration, float distance, bool overrideCurrentShake = false)
    //{
    //    if (instance == null) { return; }
    //    if (isShaking && !overrideCurrentShake) { return; }

    //    instance.duration = duration;

    //    instance.StopCoroutine(StartShakingCamera());
    //    instance.StartCoroutine(StartShakingCamera());
    //}

}
