using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Animator animator;
    private Vector2 target;
    private Vector2 startPosition;
    private float speed = 5f;

    private void Awake()
    {
        gameObject.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Shoot(Vector2 playerPosition, Vector2 t)
    {
        gameObject.SetActive(true);
        startPosition = playerPosition;
        transform.position = playerPosition;
        target = t;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            animator.SetBool("Destroy", true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // transform.position = Vector2.MoveTowards(startPosition, target, speed * Time.deltaTime);
    }
}
