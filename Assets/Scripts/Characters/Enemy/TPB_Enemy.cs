using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Jake Pauls
 * TPB_Enemy.cs
 * Enemy definition in TPB
 */

public class TPB_Enemy : TPB_Character
{
    [Header ("Enemy Movement")]
    public float aggroRange;
    public Transform target;

    protected override void Awake()
    {
        base.Awake();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }
}