using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Jake Pauls
 * PhaseShift.cs
 * Casting logic and implementation for PhaseShift
 */

[CreateAssetMenu (menuName = "Abilities/Phase Shift")]
public class PhaseShift : TPB_Ability
{   
    private GameObject player;
    private SpriteRenderer affectedSpriteRenderer;
    private bool toggle = false;

    public override void Initialize(GameObject obj) 
    {
        player = obj;
        affectedSpriteRenderer = obj.GetComponent<SpriteRenderer>();
    }

    public override void Cast() 
    {
        toggle = !toggle;
        if (toggle) {
            Debug.Log("PhaseShift was casted.");
            Debug.Log("PhaseShift has a soulCost of " + soulCost);
            player.layer = LayerMask.NameToLayer("PlayerIgnoreWall");
            affectedSpriteRenderer.color = new Color(1,1,1,.5f);   
        } else {
            player.layer = LayerMask.NameToLayer("Default");
            affectedSpriteRenderer.color = new Color(1,1,1,1);
        }
    }
}
