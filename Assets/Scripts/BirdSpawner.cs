using UnityEngine;

public class BirdSpawner : EnemySpawner
{
    protected override GameObject InstantiateEnemy()
    {
        var enemy = Instantiate(EnemyPrefab, Game.Instance.Playism.transform);
        var direction = Util.RandomDirection;

        enemy.transform.position = direction * SPAWN_DISTANCE_FROM_CENTER_DEFAULT;
        enemy.GetComponent<Bird>().Direction = - direction;

        return enemy;
    }
}
