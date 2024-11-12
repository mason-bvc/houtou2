using UnityEngine;

public class Killable : MonoBehaviour
{
    private Collider2D _hurtBox;
    private Collider2DGameObject _colliderGameObj;

    protected void Start()
    {
        _hurtBox = GetComponent<Collider2D>();
        _colliderGameObj = GetComponent<Collider2DGameObject>();
        _colliderGameObj.TriggerEntered2D.AddListener((Collider2D collider) => Game.HandleTriggerEnter2D(_hurtBox, collider));
    }
}
