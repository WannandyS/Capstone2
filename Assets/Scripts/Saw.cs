using UnityEngine;

public class Saw : MonoBehaviour
{
    public float speed = 2.5f;
    private int index = 0;
    public Transform[] points;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, points[index].position) < 0.1f)
        {
            index += 1;
        }

        if (index == points.Length)
        {
            index = 0;
        }

        transform.position = Vector2.MoveTowards(transform.position, points[index].position, speed * Time.deltaTime);
    }
}
