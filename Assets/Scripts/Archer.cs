using UnityEngine;

public class Archer : MonoBehaviour
{
    [Header("Check Player")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform checkPoint;
    [SerializeField] private float size_X = 4f;
    [SerializeField] private float size_Y = 2f;
    [SerializeField] private LayerMask whatIsPlayer;

    [Header("Spawn Arrow")]
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float x_Velocity = 10f;

    [Header("Health")]
    [SerializeField] private int maxHealth = 2;
    public GameObject explosionPrefab;

    private bool facingLeft;
    private Animator animator;
    private AudioManager audio;

    void Start()
    {
        facingLeft = true;
        animator = this.gameObject.GetComponent<Animator>();
        audio = FindAnyObjectByType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D collInfo = Physics2D.OverlapBox(checkPoint.position, new Vector2(size_X, size_Y), 0f, whatIsPlayer);

        if (maxHealth <= 0)
        {
            Die();
        }

        if (FindAnyObjectByType<Manager>().isGameActive == false)
        {
            return;
        }

        if (collInfo)
        {
            if (facingLeft && player.position.x > transform.position.x)
            {
                transform.eulerAngles = new Vector3(0f, -180f, 0f);
                facingLeft = false;
            }
            else if (!facingLeft && player.position.x < transform.position.x)
            {
                transform.eulerAngles = new Vector3(0f, 0f, 0f);
                facingLeft = true;
            }
            Attack();
            animator.SetBool("Attack", true);
        }
        else
        {
            animator.SetBool("Attack", false);
        }
    }

    void Attack()
    {
        Debug.Log("Attack arrow");
    }

    public void SpawnArrow()
    {
        GameObject tempArrowPrefab = Instantiate(arrowPrefab, spawnPoint.position, spawnPoint.rotation);
        tempArrowPrefab.gameObject.GetComponent<Rigidbody2D>().linearVelocity = x_Velocity * (-spawnPoint.right);

        FindAnyObjectByType<AudioManager>().PlayShootSound();
    }

    public void TakeDamage()
    {
        if (maxHealth <= 0) return;

        maxHealth -= 1;
        animator.SetTrigger("Hurt");
        Camera.instance.Shake(1.5f, 0.2f);
        audio.enemyHurtAudio.Play();
    }

    private void OnDrawGizmosSelected()
    {
        if (checkPoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(checkPoint.position, new Vector2(size_X, size_Y));
        }
    }

    void Die()
    {
        Camera.instance.Shake(2.5f, 0.5f);
        Debug.Log("Enemy Died");

        GameObject temp = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(temp, 0.7f);

        Destroy(gameObject);
        FindAnyObjectByType<AudioManager>().PlayEnemyDied2Sound();
    }
}
