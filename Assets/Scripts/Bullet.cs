using UnityEngine;

public class Bullet : MonoBehaviour, IDirectional2D
{
    protected Collider2DGameObject Collider2DGameObject;

    [SerializeField]
    protected float Speed = 1.0F;

    public Vector2 Direction { get; set; } = Vector2.up;

    protected void Start()
    {
        Collider2DGameObject = transform.Find("HitBox")?.GetComponent<Collider2DGameObject>();
        Collider2DGameObject.TriggerEntered2D.AddListener(other =>
        {
            if (other.CompareTag("Enemy"))
            {
                Destroy(gameObject);
            }
        });
    }

    protected void Update()
    {
        transform.Translate(Direction * Speed * Time.deltaTime, Space.World);
    }
}
