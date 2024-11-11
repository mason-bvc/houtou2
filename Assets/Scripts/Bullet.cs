using UnityEngine;

public class Bullet : MonoBehaviour, IDirectional2D
{
    [SerializeField]
    protected float Speed = 1.0F;

    public Vector2 Direction { get; set; } = Vector2.up;

    protected void Update()
    {
        transform.Translate(Direction * Speed * Time.deltaTime, Space.World);
    }
}
