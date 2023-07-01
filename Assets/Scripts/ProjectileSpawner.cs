using UnityEngine;
using Zenject;

public class ProjectileSpawner : Factory
{
    [Inject]
    FloatingJoystick shootingJoystick;

    [SerializeField] private Projectile productPrefab;

    public override IProduct Create(Vector3 position)
    {
        // create a Prefab instance and get the product component
        GameObject instance = Instantiate(productPrefab.gameObject, position, Quaternion.identity);
        Projectile newProduct = instance.GetComponent<Projectile>();

        Vector2 normalizeDirection;
        normalizeDirection.x = shootingJoystick.Horizontal;
        normalizeDirection.y = shootingJoystick.Vertical;

        normalizeDirection.Normalize();
        // each product contains its own logic
        newProduct.Initialize(normalizeDirection);

        return newProduct;
    }
}
