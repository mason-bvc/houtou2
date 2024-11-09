using UnityEngine;

public class Follower : MonoBehaviour
{
    [SerializeField]
    private float _speed = 1.0F;

    protected void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, Game.Player.transform.position, _speed * Time.deltaTime);
    }
}
