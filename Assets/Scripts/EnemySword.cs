using UnityEngine;
//kodenya sama persis dengan enemy2
public class EnemySword : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 3f;
    private bool facingRight;
    public float chaseSpeed = 2;
    private Animator animator;

    [Header("HP")]
    public int maxHealth = 3;

    [Header("Point")]
    public Transform detectPoint;
    public float distance = 1.5f;
    public LayerMask targetLayer;
    public Transform player;
    public float attackRange = 5f;
    public float retrieveDistance = 2.5f;
    public Transform attackPoint;
    public float attackRadius = 1f;
    public LayerMask layerPlayer;

    [Header("Attack System")]
    public float attackCooldown = 2f;
    private float lastAttackTime;

    public GameObject explosionPrefab;
    private AudioManager audio;

    void Start()
    {
        facingRight = true;
        animator = GetComponent<Animator>();
        audio = FindAnyObjectByType<AudioManager>();
    }

    void Update()
    {
        if (maxHealth <= 0)
        {
            Died();
        }

        if (FindAnyObjectByType<Manager>().isGameActive == false)
        {
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            if (player.position.x < transform.position.x && facingRight)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                facingRight = false;
            }
            else if (player.position.x > transform.position.x && !facingRight)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                facingRight = true;
            }

            if (distanceToPlayer > retrieveDistance)
            {
                transform.position = Vector2.MoveTowards(
                    transform.position,
                    player.position,
                    chaseSpeed * Time.deltaTime
                );
            }
            else
            {
                if (Time.time >= lastAttackTime + attackCooldown)
                {
                    AttackAnim();
                    lastAttackTime = Time.time;
                }
            }
        }
        else
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }

        RaycastHit2D hit = Physics2D.Raycast(detectPoint.position, Vector2.down, distance, targetLayer);

        if (hit == false)
        {
            if (facingRight)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                facingRight = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                facingRight = true;
            }
        }
    }

    void AttackAnim()
    {
        int random = Random.Range(0, 2);

        if (random == 0)
        {
            animator.SetTrigger("Attack1");
            audio.enemyAttackAudio.Play();
        }
        else
        {
            animator.SetTrigger("Attack2");
            audio.enemyAttackAudio.Play();
        }
    }

    public void Attack()
    {
        Collider2D hit = Physics2D.OverlapCircle(attackPoint.position, attackRadius, layerPlayer);

        if (hit)
        {
            FindAnyObjectByType<Player>().TakeDamage();
        }
    }

    public void TakeDamage()
    {
        if (maxHealth <= 0) return;

        maxHealth -= 1;
        animator.SetTrigger("Hurt");
        Camera.instance.Shake(1.5f, 0.2f);
        audio.enemyHurtAudio.Play();
    }

    void Died()
    {
        Camera.instance.Shake(2.5f, 0.5f);
        Debug.Log("Enemy Died");

        GameObject temp = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(temp, 0.7f);

        Destroy(gameObject);
        audio.enemyDied1Audio.Play();
    }

    private void OnDrawGizmosSelected()
    {
        if (detectPoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(detectPoint.position, Vector2.down * distance);
        }

        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
        }
    }
}