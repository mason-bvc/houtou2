using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private const float SPAWN_DISTANCE_FROM_CENTER_DEFAULT = 10.0F;
    private const float SPAWN_COOLDOWN_SECONDS_DEFAULT = 0.4F;

    private int _fairyCount;

    public IEnumerator SpawnWaiter()
    {
        yield return new WaitForSeconds(SPAWN_COOLDOWN_SECONDS_DEFAULT);

        var fairy = Instantiate(Resources.Prefabs.Fairy);

        fairy.transform.position = Util.RandomDirection * SPAWN_DISTANCE_FROM_CENTER_DEFAULT;

        // if (++_fairyCount < 3)
        // {
            StartCoroutine(SpawnWaiter());
        // }
    }
}
