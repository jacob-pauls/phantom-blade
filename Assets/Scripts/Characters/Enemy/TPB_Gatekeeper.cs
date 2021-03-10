using UnityEngine;

/**
 * Jake Pauls
 * TPB_Enemy.cs
 * Gatekeeper boss fight logic in TPB
 */

public class TPB_Gatekeeper : TPB_Melee_Enemy
{
    [Header ("Guardian Ability Parameteters")]
    [Space]
    [SerializeField] private float chargeSpeed;
    [SerializeField] private float chargeDistance;
    private float chargeTime;

    [HideInInspector] public bool isCharging;    

    public void Charge(bool startCharging) 
    {
        if (startCharging || isCharging) {
            if (chargeTime <= 0) {
                isCharging = false;
                base.anim.SetBool("isCharging", false);

                chargeTime = chargeDistance;
                base.rb2D.velocity = Vector2.zero;
            } else {
                isCharging = true;
                base.anim.SetBool("isCharging", true);
                
                chargeTime -= Time.deltaTime;
                
                if (base.rb2D.velocity.x > 0) {
                    base.rb2D.AddRelativeForce(Vector2.right * chargeSpeed);
                } else if (base.rb2D.velocity.x < 0) {
                    base.rb2D.AddRelativeForce(Vector2.left * chargeSpeed);
                } else if (base.rb2D.velocity.x == 0) {
                    if (base.isFacingRight) {
                        base.rb2D.AddRelativeForce(Vector2.right * chargeSpeed);
                    } else {
                        base.rb2D.AddRelativeForce(Vector2.left * chargeSpeed);
                    }
                }
            }
        }
    }
}