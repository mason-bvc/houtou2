using UnityEngine;

public class Fairy : Mob
{
    protected override void OnDamaged()
    {
        if (Health.GetHealth() <= 0.0F)
        {
            for (var i = 0; i < 3; i++)
            {
                var bird = Instantiate(Resources.Prefabs.Bird, GameObject.Find("Playism").transform);
                var birdComponent = bird.GetComponent<Bird>();
                var direction = Util.RandomDirection;

                bird.transform.position = transform.position + (Vector3)direction / 4.0F;
                birdComponent.Direction = direction * 2.0F;
            }
        }

        base.OnDamaged();
    }
}
