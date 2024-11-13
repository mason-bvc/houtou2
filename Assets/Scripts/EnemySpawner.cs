using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    protected const float SPAWN_DISTANCE_FROM_CENTER_DEFAULT = 10.0F;
    protected const float SPAWN_COOLDOWN_SECONDS_DEFAULT = 0.4F;

    private List<GameObject> _enemies = new();
    public int CurrentMaxEnemies = 5;
    public bool IsStopped;

    [SerializeField]
    public GameObject EnemyPrefab;

    protected virtual GameObject InstantiateEnemy()
    {
        var enemy = Instantiate(EnemyPrefab);
        enemy.transform.position = Util.RandomDirection * SPAWN_DISTANCE_FROM_CENTER_DEFAULT;
        return enemy;
    }

    public IEnumerator BeginSpawn()
    {
        if (IsStopped)
        {
            yield return null;
        }

        if (_enemies.Count >= CurrentMaxEnemies)
        {
            _enemies.RemoveAll(go => go == null);
        }

        yield return new WaitForSeconds(SPAWN_COOLDOWN_SECONDS_DEFAULT);

        if (_enemies.Count < CurrentMaxEnemies)
        {
            _enemies.Add(InstantiateEnemy());
        }

        StartCoroutine(BeginSpawn());
    }
}
