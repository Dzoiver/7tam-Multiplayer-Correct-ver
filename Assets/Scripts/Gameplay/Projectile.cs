using UnityEngine;
using Unity.Netcode;

public class Projectile : NetworkBehaviour, IProduct
{
    [SerializeField] private string productName = "PlayerProjectile";
    public string ProductName { get => productName; set => productName = value; }

    private Animator animator;
    private Vector2 normalizeDirection;
    private float speed = 12f;

    public void Initialize(Vector2 direction)
    {
        normalizeDirection = direction;
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !IsOwner && gameObject.CompareTag("Projectile"))
        {
            animator.SetBool("Destroy", true);
            speed = 0f;
        }

        if (collision.gameObject.CompareTag("Player") && !IsOwner && gameObject.CompareTag("EnemyProjectile"))
        {
            animator.SetBool("Destroy", true);
            speed = 0f;
        }


    }

    [ClientRpc]
    private void DestryoProjectileClientRpc()
    {
        animator.SetBool("Destroy", true);
        speed = 0f;
    }

    public void ProjectileDestroy()
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.magnitude > 30.0f)
        {
            Destroy(gameObject);
        }
        transform.Translate(normalizeDirection * speed * Time.deltaTime);
        // transform.position = normalizeDirection * speed * Time.deltaTime;
    }
}
