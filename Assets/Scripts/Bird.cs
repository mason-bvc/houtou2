using UnityEngine;

public class Bird : Mob, IDirectional2D
{
    public Vector2 Direction { get; set; }

    protected void Update()
    {
        transform.Translate(Direction * Time.deltaTime, Space.World);
    }
}
