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

        // This is a trivial branch, but if I wanted to be a micro-optimizer,
        // I could set up a big circle trigger in the main scene that, upon
        // detecting that a bullet has exited it, suggests to the bullet that
        // it should be deleted.
        if (transform.position.sqrMagnitude >= DestroyDistanceSquared)
        {
            Destroy(gameObject);
        }
    }
}
