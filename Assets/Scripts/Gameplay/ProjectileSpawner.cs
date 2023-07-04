using UnityEngine;
using Unity.Netcode;

public class ProjectileSpawner : Factory
{
    [SerializeField] FloatingJoystick shootingJoystick;

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

        /*
        if (IsClient)
        {
            SpawnProjectileServerRpc(position, normalizeDirection);
            // Ask server to spawn
        }
        */
        // each product contains its own logic
        newProduct.Initialize(normalizeDirection);

        SpawnProjectileClientRpc(position, normalizeDirection); // Only host can call. All recieve
        SpawnProjectileServerRpc(position, normalizeDirection); // All can call. Only server recieve

        return newProduct;
    }

    /*
     * Server shoot and spawn projectile
     */

    [ServerRpc]
    private void SpawnProjectileServerRpc(Vector3 position, Vector2 direction)
    {
        if (IsOwner)
            return;
        GameObject instance = Instantiate(productPrefab.gameObject, position, Quaternion.identity);
        Projectile newProduct = instance.GetComponent<Projectile>();
        newProduct.Initialize(direction);
    }

    [ClientRpc]
    private void SpawnProjectileClientRpc(Vector3 position, Vector2 direction)
    {
        if (IsOwner)
            return;
        GameObject instance = Instantiate(productPrefab.gameObject, position, Quaternion.identity);
        Projectile newProduct = instance.GetComponent<Projectile>();
        newProduct.Initialize(direction);
    }
}
