using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int maxHealth = 5;

    private float movement;
    public float speed = 5f;
    private bool facingRight = true;
    private Rigidbody2D rb;
    public float jumpHeight = 20f;
    private bool isGround;
    private Animator animator;

    public Transform attackPoint;
    public float attackRadius = 1;
    public LayerMask attackLayer;

    public TMP_Text hpText;
    private int currentDiamond;
    public TMP_Text currentDiamondText;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentDiamond = 0;
    }

    // Update is called once per frame
    void Update()
    {
        movement = Input.GetAxis("Horizontal");
        transform.position += new Vector3(movement, 0, 0) * Time.deltaTime * speed;

        Flip();

        if (maxHealth <= 0)
        {
            Died();
        }

        hpText.text = maxHealth.ToString();
        currentDiamondText.text = currentDiamond.ToString();

        if (Input.GetKey(KeyCode.Space) && isGround == true)
        {
            Jump();
            isGround = false;
            animator.SetBool("Jump", true);
        }

        //lari
        if (Mathf.Abs(movement) > .1f)
        {
            animator.SetFloat("Run", 1f);
        }
        else if (movement < .1f)
        {
            animator.SetFloat("Run", 0f);
        }

        //serang
        if (Input.GetKeyDown(KeyCode.J))
        {
            AttackAnim();
        }
    }

    void AttackAnim()
    {
        int random = Random.Range(0, 3);

        if (random == 0)
        {
            animator.SetTrigger("Attack1");
        }
        else if (random == 1)
        {
            animator.SetTrigger("Attack2");
        }
        else if (random == 2)
        {
            animator.SetTrigger("Attack3");
        }
    }

    void Flip()
    {
        if (movement < 0 && facingRight == true)
        {
            transform.eulerAngles = new Vector3(0, -180, 0);
            facingRight = false;
        }
        else if (movement > 0 && facingRight == false)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            facingRight = true;
        }
    }

    void Jump()
    {
        rb.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
    }

    public void Attack()
    {
        Collider2D hit = Physics2D.OverlapCircle(attackPoint.position, attackRadius, attackLayer);

        if (hit == true)
        {
            FindAnyObjectByType<Enemy2>().TakeDamage();
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

    public void Died()
    {
        Debug.Log("Player Dead");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Heart")
        {
            maxHealth += 1;
            collision.gameObject.GetComponent<Animator>().SetTrigger("CollectHeal");
            Destroy(collision.gameObject, 0.3f);
        }

        if (collision.gameObject.tag == "Item")
        {
            collision.gameObject.GetComponent<Animator>().SetTrigger("CollectDiamond");
            Destroy(collision.gameObject, 0.3f);
            currentDiamondText.text = currentDiamond.ToString();
            currentDiamond += 1;
            Destroy(collision.gameObject, 0.3f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Stage")
        {
            isGround = true;
            animator.SetBool("Jump", false);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}
