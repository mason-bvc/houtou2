using UnityEngine;
using UnityEngine.Events;

public class Collider2DGameObject : MonoBehaviour
{
    public readonly UnityEvent<Collision2D> CollisionEntered2D = new();
    public readonly UnityEvent<Collision2D> CollisionExited2D = new();
    public readonly UnityEvent<Collider2D> TriggerEntered2D = new();
    public readonly UnityEvent<Collider2D> TriggerExited2D = new();

    protected void OnCollisionEnter2D(Collision2D collision) => CollisionEntered2D.Invoke(collision);
    protected void OnCollisionExit2D(Collision2D collision) => CollisionExited2D.Invoke(collision);
    protected void OnTriggerEnter2D(Collider2D collider) => TriggerEntered2D.Invoke(collider);
    protected void OnTriggerExit2D(Collider2D collider) => TriggerExited2D.Invoke(collider);
}
