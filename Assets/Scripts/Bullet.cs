using UnityEngine;

public class Bullet : MonoBehaviour
{
    public const float DESTROY_DISTANCE_SQUARED_DEFAULT = 100.0F;

    [SerializeField]
    protected float Speed = 1.0F;

    public float DestroyDistanceSquared = DESTROY_DISTANCE_SQUARED_DEFAULT;
    public Vector2 Direction = Vector2.up;

    protected void Update()
    {
        transform.Translate(Direction * Speed * Time.deltaTime, Space.World);

        if (transform.position.sqrMagnitude >= DestroyDistanceSquared)
        {
            Destroy(gameObject);
        }
    }
}
