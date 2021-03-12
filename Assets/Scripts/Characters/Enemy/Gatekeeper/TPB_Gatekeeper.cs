using UnityEngine;

/**
 * Jake Pauls
 * TPB_Enemy.cs
 * Gatekeeper boss fight logic in TPB
 */

public class TPB_Gatekeeper : TPB_Melee_Enemy
{
    [Header ("Charge Parameteters")]
    [Space]
    [SerializeField] private float chargeSpeed;
    [SerializeField] private float chargeDistance;
    private float chargeTime;

    [Header ("Slash Projectile Parameteters")]
    [Space]
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private float projectileDelay;
    [SerializeField] private float projectileSpeed;
    private float timeBetweenProjectiles;

    [HideInInspector] public bool isCharging;  
    [HideInInspector] public bool isSlashing; 

    public void Charge(bool startCharging) 
    {
        if (startCharging || isCharging) {
            if (chargeTime <= 0) {
                isCharging = false;
                isAttackingDisabled = false;
                base.anim.SetBool("isCharging", false);

                chargeTime = chargeDistance;
                base.rb2D.velocity = Vector2.zero;
            } else {
                isCharging = true;
                base.isAttackingDisabled = true;
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

    private void FireProjectile(Vector2 direction, float force) 
    {
        GameObject slashProjectile = (GameObject) Instantiate(projectile, projectileSpawnPoint.position, Quaternion.identity);
        
        if (base.isFacingRight) {
            Vector3 newScale = slashProjectile.transform.localScale;
            newScale.x = -Mathf.Abs(newScale.x);
            slashProjectile.transform.localScale = newScale;
        } else {
            Vector3 newScale = slashProjectile.transform.localScale;
            newScale.x = Mathf.Abs(newScale.x);
            slashProjectile.transform.localScale = newScale;
        }

        Rigidbody2D projectileRigidBody = slashProjectile.GetComponent<Rigidbody2D>();
        projectileRigidBody.AddForce(direction * force);

        // Get the only animation length from the projectile, destroy projectile once exceeds duration
        float projectileDuration = slashProjectile.GetComponent<Animator>().runtimeAnimatorController.animationClips[0].length;
        Destroy(slashProjectile, projectileDuration);
    }

    public void SlashProjectile(bool startSlash) 
    {
        if (timeBetweenProjectiles <= 0) {
            if (startSlash) {
                isSlashing = true;
                base.anim.SetBool("firstAttack", true);
                base.isAttackingDisabled = true;

                if (base.isFacingRight) {
                    FireProjectile(Vector2.right, projectileSpeed);
                } else {
                    FireProjectile(Vector2.left, projectileSpeed);
                }

                timeBetweenProjectiles = projectileDelay;
            } else {
                isSlashing = false;
            }
        } else {
            base.isAttackingDisabled = false;
            timeBetweenProjectiles -= Time.deltaTime;
        }
    }

    [ContextMenu ("Test Damage")]
    void TestDamage() 
    {
        ChangeHealthAmount(-10);
    }

    [ContextMenu ("Test Heal")]
    void TestHeal()
    {
        ChangeHealthAmount(10);
    }
}