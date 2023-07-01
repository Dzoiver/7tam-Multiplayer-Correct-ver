using UnityEngine;

public interface IProduct
{
    public string ProductName { get; set; }

    public void Initialize(Vector2 direction);
}

public abstract class Factory : MonoBehaviour
{
    public abstract IProduct Create(Vector3 position);
}