using UnityEngine;

public class Arrow : MonoBehaviour
{
    void Start()
    {
        Destroy(this.gameObject, 2f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.GetComponent<Player>() != null)
            {
                collision.gameObject.GetComponent<Player>().TakeDamage();
                Destroy(this.gameObject);
            }
        }
    }
}