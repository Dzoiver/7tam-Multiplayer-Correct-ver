using UnityEngine;
using Unity.Netcode;

public interface IProduct
{
    public string ProductName { get; set; }

    public void Initialize(Vector2 direction);
}

public abstract class Factory : NetworkBehaviour
{
    public abstract IProduct Create(Vector3 position);
}