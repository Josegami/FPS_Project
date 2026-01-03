using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int HP = 100;
    private Animator animator;

    private NavMeshAgent navAgent;
    private Collider enemyCollider;

    public bool isDead;

    private void Start()
    {
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        enemyCollider = GetComponent<Collider>();
    }

    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;

        if (HP <= 0)
        {
            isDead = true;

            int randomValue = Random.Range(0, 2);

            if (randomValue == 0)
            {
                animator.SetTrigger("DIE1");
            }
            else
            {
                animator.SetTrigger("DIE2");
            }

            if (enemyCollider != null)
            {
                enemyCollider.enabled = false;
            }

            //Dead Sound
            SoundManager.Instance.monsterChannel2.PlayOneShot(SoundManager.Instance.monsterDeath);
                
        }
        else
        {
            animator.SetTrigger("DAMAGE");

            //Hurt sound
            SoundManager.Instance.monsterChannel2.PlayOneShot(SoundManager.Instance.monsterHurt);

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
