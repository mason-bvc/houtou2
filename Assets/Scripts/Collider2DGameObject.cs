using UnityEngine;
using UnityEngine.Events;

public class Collider2DGameObject : MonoBehaviour
{
    public readonly UnityEvent<Collision2D> Collision2DEntered = new();
    public readonly UnityEvent<Collision2D> Collision2DExited = new();

    protected void OnCollisionEnter2D(Collision2D collision) => Collision2DEntered.Invoke(collision);
    protected void OnCollisionExit2D(Collision2D collision) => Collision2DExited.Invoke(collision);
}
