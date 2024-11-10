using UnityEngine;

public class Fairy : MonoBehaviour
{
    private Collider2D _hurtBox;
    private Collider2DGameObject _colliderGameObj;

    protected void Start()
    {
        _hurtBox = transform.Find("HurtBox")?.GetComponent<Collider2D>();
        _colliderGameObj = _hurtBox?.GetComponent<Collider2DGameObject>();
        _colliderGameObj.TriggerEntered2D.AddListener((Collider2D collider) => Game.HandleTriggerEnter2D(_hurtBox, collider));
    }
}
