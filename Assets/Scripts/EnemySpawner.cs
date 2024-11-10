using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private const float SPAWN_DISTANCE_FROM_CENTER_DEFAULT = 10.0F;
    private const float SPAWN_COOLDOWN_SECONDS_DEFAULT = 1.0F;

    private GameObject _fairyPrefab;
    private int _fairyCount;

    protected void Start()
    {
        _fairyPrefab = Game.AssetBundle.LoadAsset<GameObject>("Fairy");
        StartCoroutine(SpawnWaiter());
    }

    private IEnumerator SpawnWaiter()
    {
        yield return new WaitForSeconds(SPAWN_COOLDOWN_SECONDS_DEFAULT);

        var fairy = Instantiate(_fairyPrefab);

        fairy.transform.position = new Vector3(Util.ValueFromMinusOneToPositiveOne, Util.ValueFromMinusOneToPositiveOne, Util.ValueFromMinusOneToPositiveOne).normalized * SPAWN_DISTANCE_FROM_CENTER_DEFAULT;

        // if (++_fairyCount < 3)
        // {
            StartCoroutine(SpawnWaiter());
        // }
    }
}
