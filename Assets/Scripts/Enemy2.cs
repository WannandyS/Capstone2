using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    [Header ("Movement")]
    public float speed = 3f;
    private bool facingRight;
    public float chaseSpeed = 2;
    private Animator animator;

    [Header ("HP")]
    public int maxHealth = 3;

    [Header ("Point")]
    public Transform detectPoint;
    public float distance = 1.5f;
    public LayerMask targetLayer;
    public Transform player;
    public float attackRange = 5f;
    public float retrieveDistance = 2.5f;
    public Transform attackPoint;
    public float attackRadius = 1f;
    public LayerMask layerPlayer;

    public GameObject explosionPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        facingRight = true;
        animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (maxHealth <= 0)
        {
            Died();
        }

        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            if (player.position.x < transform.position.x && facingRight == true)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                facingRight = false;
            } else if (player.position.x > transform.position.x && facingRight == false)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                facingRight = true;
            }

            if (Vector2.Distance(transform.position, player.position) > retrieveDistance)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
                animator.SetBool("Attack", false);
            } else
            {
                animator.SetBool("Attack", true);
            }
        } else
        {
            transform.Translate(new Vector2(1, 0) * speed * Time.deltaTime);
        }


        RaycastHit2D hit = Physics2D.Raycast(detectPoint.position, Vector2.down, distance, targetLayer);

        if (hit == false)
        {
            if (facingRight == true)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                facingRight = false;
            } else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                facingRight = true;
            }
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
        if (maxHealth <= 0)
        {
            return;
        }
        maxHealth -= 1;
        animator.SetTrigger("Hurt");
        Camera.instance.Shake(1.5f, 0.2f);
    }

    void Died()
    {
        Camera.instance.Shake(2.5f, 0.5f);
        Debug.Log("Enemy Died");
        GameObject temp = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(temp, 16);
        Destroy(this.gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        if (detectPoint == null) {
            return;
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(detectPoint.position, Vector2.down * distance);

        if (attackPoint == null)
        {
            return;
        }
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}
