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

    [Header("Loot Settings")]
    public GameObject heartPrefab;
    [Range(0, 100)] public float dropChance = 25f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        enemyCollider = GetComponent<Collider>();
    }

    public void TakeDamage(int damageAmount)
    {
        if (isDead) return;

        HP -= damageAmount;

        if (HP <= 0)
        {
            isDead = true;

            if (animator != null)
            {
                int randomValue = UnityEngine.Random.Range(0, 2);
                if (randomValue == 0) animator.SetTrigger("DIE1");
                else animator.SetTrigger("DIE2");
            }

            if (enemyCollider != null) enemyCollider.enabled = false;

            if (monsterHand != null && monsterHand.GetComponent<Collider>() != null)
            {
                monsterHand.GetComponent<Collider>().enabled = false;
            }

            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.monsterChannel2.PlayOneShot(SoundManager.Instance.monsterDeath);
            }

            SpawnLoot();
        }
        else
        {
            if (animator != null)
            {
                animator.SetTrigger("DAMAGE");
                animator.SetBool("isChasing", true);
            }

            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.monsterChannel2.PlayOneShot(SoundManager.Instance.monsterHurt);
            }
        }
    }

    private void SpawnLoot()
    {
        if (heartPrefab == null) return;

        float roll = UnityEngine.Random.Range(0f, 100f);
        if (roll <= dropChance)
        {
            Instantiate(heartPrefab, transform.position + Vector3.up, Quaternion.identity);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2.5f);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 18f);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 21f);
    }
}