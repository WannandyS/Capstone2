using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

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

    //public Transform spawnPosition;
    public GameObject gameOverUI;
    public GameObject winUI;

    private AudioManager audio;
    public CanvasGroup fadePanel;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentDiamond = 0;

        audio = FindAnyObjectByType<AudioManager>();

        StartCoroutine(FadeIn());

        IEnumerator FadeIn()
        {
            fadePanel.alpha = 1;

            for (float i = 1; i >= 0; i -= Time.deltaTime)
            {
                fadePanel.alpha = i;
                yield return null;
            }
        }
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
        if (Mathf.Abs(movement) > .1f && isGround)
        {
            animator.SetFloat("Run", 1f);

            if (isGround && audio != null && !audio.moveAudio.isPlaying)
            {
                audio.PlayWalkSound();
            }
        }
        else
        {
            animator.SetFloat("Run", 0f);

            if (audio != null)
            {
                audio.moveAudio.Stop();
            }
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

        AudioManager audio = FindAnyObjectByType<AudioManager>();

        if (random == 0)
        {
            animator.SetTrigger("Attack1");
            audio.PlayAttack1Sound();
        }
        else if (random == 1)
        {
            animator.SetTrigger("Attack2");
            audio.PlayAttack2Sound();
        }
        else if (random == 2)
        {
            animator.SetTrigger("Attack3");
            audio.PlayAttack3Sound();
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
        FindAnyObjectByType<AudioManager>().PlayJumpSound();
    }

    public void Attack()
    {
        Collider2D hit = Physics2D.OverlapCircle(attackPoint.position, attackRadius, attackLayer);

        if (hit != null)
        {
            if (hit.GetComponent<Enemy2>() != null)
            {
                hit.GetComponent<Enemy2>().TakeDamage();
            }

            if (hit.GetComponent<EnemySword>() != null)
            {
                hit.GetComponent<EnemySword>().TakeDamage();
            }

            if (hit.GetComponent<Archer>() != null)
            {
                hit.GetComponent<Archer>().TakeDamage();
            }
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
        audio.PlayHurtSound();

        //transform.position = spawnPosition.position; ini gak ini dipake
    }

    public void Died()
    {
        Camera.instance.Shake(2f, 0.2f);
        gameOverUI.SetActive(true);
        FindAnyObjectByType<Manager>().isGameActive = false;
        Destroy(this.gameObject);
        audio.PlayHurtSound();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Heart")
        {
            maxHealth += 1;
            collision.gameObject.GetComponent<Animator>().SetTrigger("CollectHeal");
            Destroy(collision.gameObject, 0.3f);
            audio.PlayHeartSound();
        }

        if (collision.gameObject.tag == "Item")
        {
            collision.gameObject.GetComponent<Animator>().SetTrigger("CollectDiamond");
            Destroy(collision.gameObject, 0.3f);
            currentDiamondText.text = currentDiamond.ToString();
            currentDiamond += 1;
            Destroy(collision.gameObject, 0.3f);
            audio.PlayCollectGemSound();
        }

        if (collision.gameObject.tag == "Key")
        {
            collision.gameObject.GetComponent<Animator>().SetTrigger("CollectKey");
            Destroy(collision.gameObject, 0.3f);
            StartCoroutine(NextStage());
            audio.PlayKeySound();
        }

        if (collision.gameObject.tag == "TreasureKey")
        {
            winUI.SetActive(true);
            FindAnyObjectByType<Manager>().isGameActive = false;
        }

        if (collision.gameObject.tag == "Traps")
        {
            TakeDamage();
        }
    }

    IEnumerator NextStage()
    {
        yield return new WaitForSeconds(0.2f);

        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            fadePanel.alpha = i;
            yield return null;
        }

        SceneManager.LoadScene("Stage2");
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
