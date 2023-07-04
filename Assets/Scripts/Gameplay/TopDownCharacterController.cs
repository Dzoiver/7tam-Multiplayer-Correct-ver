using UnityEngine;
using Zenject;
using Unity.Netcode;

namespace Cainos.PixelArtTopDown_Basic
{
    public class TopDownCharacterController : NetworkBehaviour
    {
        [SerializeField] FloatingJoystick movementJoystick;
        [SerializeField] FloatingJoystick shootingJoystick;
        [SerializeField] GameObject projectilePrefab;
        [SerializeField] ProjectileSpawner projectileSpawner;

        [SerializeField] GameObject projectile;

        private float speed = 4;
        private Animator animator;
        private Rigidbody2D rb;
        private float shootInterval = 0.15f;
        private float timeSinceLastProjectile = 0f;

        Vector2 dir = Vector2.zero;

        private void Start()
        {
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();

            if (!IsOwner)
            {
                gameObject.tag = "Enemy";
                Destroy(movementJoystick.gameObject);
                Destroy(shootingJoystick.gameObject);
            }
        }
        private void Update()
        {
            if (!IsOwner)
                return;

            dir.Normalize();
            animator.SetBool("IsMoving", dir.magnitude > 0);

            Vector3 direction = Vector3.forward * movementJoystick.Vertical + Vector3.right * movementJoystick.Horizontal;

            direction.Normalize();

            if ((shootingJoystick.Horizontal != 0 || shootingJoystick.Vertical != 0)
                && timeSinceLastProjectile > shootInterval)
            {
                timeSinceLastProjectile = 0f;

                Vector2 normalizeDirection;
                normalizeDirection.x = shootingJoystick.Horizontal;
                normalizeDirection.y = shootingJoystick.Vertical;

                normalizeDirection.Normalize();
                normalizeDirection *= 0.2f; // Make projectiles spawn a bit infornt of player so they don't stack inside walls on spawn
                normalizeDirection.x += transform.position.x;
                normalizeDirection.y += transform.position.y;
                projectileSpawner.Create(normalizeDirection);
            }
            timeSinceLastProjectile += Time.deltaTime;
        }
        private void FixedUpdate()
        {
            if (!IsOwner)
                return;

            Vector2 newVector = new Vector2();

            newVector.x = rb.position.x + movementJoystick.Horizontal * speed * Time.deltaTime;
            newVector.y = rb.position.y + movementJoystick.Vertical * speed * Time.deltaTime;


            if (movementJoystick.Horizontal > 0)
            {
                animator.SetFloat("Direction", 2);
            }
            else
            {
                animator.SetFloat("Direction", 3);
            }

            if (movementJoystick.Vertical > 0)
            {
                animator.SetFloat("Direction", 1);
            }
            else
            {
                animator.SetFloat("Direction", 0);
            }

            rb.MovePosition(newVector);
        }
    }
}
