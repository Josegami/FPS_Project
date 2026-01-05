using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int HP = 100;
    private Animator animator;

    private NavMeshAgent navAgent;
    private Collider enemyCollider;

    private MonsterHand monsterHand;

    public bool isDead;

    private void Start()
    {
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        enemyCollider = GetComponent<Collider>();
    }

    public void TakeDamage(int damageAmount)
    {
        if (isDead) return; // Exit if already dead

        HP -= damageAmount;

        if (HP <= 0)
        {
            isDead = true;

            // --- Death Logic --- //
            if (animator != null)
            {
                int randomValue = UnityEngine.Random.Range(0, 2);
                if (randomValue == 0)
                {
                    animator.SetTrigger("DIE1");
                }
                else
                {
                    animator.SetTrigger("DIE2");
                }
            }

            if (enemyCollider != null)
            {
                enemyCollider.enabled = false;
            }

            // Safety check for monsterHand
            if (monsterHand != null && monsterHand.GetComponent<Collider>() != null)
            {
                monsterHand.GetComponent<Collider>().enabled = false;
            }

            // Dead Sound
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.monsterChannel2.PlayOneShot(SoundManager.Instance.monsterDeath);
            }
        }
        else
        {
            // --- Hurt Logic --- //
            if (animator != null)
            {
                animator.SetTrigger("DAMAGE");

                // Trigger Chase state when damaged
                animator.SetBool("isChasing", true);
            }

            // Hurt sound
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.monsterChannel2.PlayOneShot(SoundManager.Instance.monsterHurt);
            }
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2.5f); // Attacking // Stop Attacking

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 18f); // Attacking // Stop Attacking

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 21f); // Stop Chassing
    }
}
