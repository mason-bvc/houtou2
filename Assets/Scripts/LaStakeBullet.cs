using UnityEngine;

public class LaStakeBullet : MonoBehaviour, IDirectional2D
{
    public Vector2 Direction { get; set; }

    protected void Update()
    {
        transform.Translate(Direction * Time.deltaTime, Space.World);
    }
}
