using UnityEngine;
using UnityEngine.Events;

/**
 * Jake Pauls
 * TPB_Player_Attack_Manager.cs
 * Player attack logic
 */

public class TPB_Player_Attack_Manager : MonoBehaviour
{
    [Header ("Attack Detection")]
    [Space]
    [SerializeField] float attackHitBoxHeight;
    [SerializeField] float attackHitBoxWidth;
    [SerializeField] Transform attackCollider;
    [SerializeField] LayerMask enemyLayer;

    [Header ("Attack Timing")]
    [Space]
    private float delayBetweenAttacks = 0f;
    [SerializeField] private float attackDelay = 0.3f;

    private Animator anim;
    private TPB_Player player;

    private bool isAttackKeyPressed = false;

    private int attackNumber;

    public UnityEvent onEnemyHit;

    void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
        player = gameObject.GetComponent<TPB_Player>();
    }

    public void MeleeAttack(bool isAttackKeyPressed) {
        this.isAttackKeyPressed = isAttackKeyPressed;
        if(delayBetweenAttacks <= 0) {
            if (isAttackKeyPressed) {   
                attackNumber = Random.Range(1,4);
                anim.SetInteger("attack", attackNumber);
                attackNumber = 0;             
                Collider2D[] enemyColliders = Physics2D.OverlapBoxAll(attackCollider.position, new Vector2(attackHitBoxWidth, attackHitBoxHeight), enemyLayer);
                for (int i = 0; i < enemyColliders.Length; i++) {
                    TPB_Enemy enemy = enemyColliders[i].GetComponent<TPB_Enemy>();
                    if (enemy && !enemy.isDead) {
                        // TODO: The logic is here to hit ONE enemy, modify this to multiple?
                        enemy.ChangeHealthAmount(-player.attackDamage);
                        onEnemyHit?.Invoke();
                        // gameObject.SetActive(false);
                        break;
                    }
                }
                delayBetweenAttacks = attackDelay;
            }
        } else {
            delayBetweenAttacks -= Time.deltaTime;
        }
    }

    public void RangedAttack(bool isAttack)
    {
        // TODO: Abstract Imput, Implement Ranged Attack Logic
        if (Input.GetButton("Range"))
            Debug.Log("Ranged Attack!");
    }

    public void ResetMeleeAttack() 
    {
        anim.SetInteger("attack", attackNumber);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackCollider.position, new Vector3(attackHitBoxWidth, attackHitBoxHeight, 1));
    }

}
