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

    [Header("Audio Local (Nuevo)")]
    public AudioSource myAudioSource;

    public AudioClip monsterWalking;
    public AudioClip monsterChase;
    public AudioClip monsterAttack;
    public AudioClip monsterHurt;
    public AudioClip monsterDeath;
    private void Start()
    {
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        enemyCollider = GetComponent<Collider>();

        monsterHand = GetComponentInChildren<MonsterHand>();

        myAudioSource = GetComponent<AudioSource>();
    }

    public void TakeDamage(int damageAmount)
    {
        if (isDead) return;

        HP -= damageAmount;

        if (HP <= 0)
        {
            isDead = true;

            Player player = FindObjectOfType<Player>();
            if (player != null)
            {
                player.AddBerserkCharge();
            }

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

            if (myAudioSource != null)
            {
                myAudioSource.PlayOneShot(monsterDeath);
            }

            SpawnLoot();

            Destroy(gameObject, 5f);
        }
        else
        {
            if (animator != null)
            {
                animator.SetTrigger("DAMAGE");
                animator.SetBool("isChasing", true);
            }

            if (myAudioSource != null)
            {
                myAudioSource.PlayOneShot(monsterHurt);
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