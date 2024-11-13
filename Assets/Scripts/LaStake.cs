using System;
using System.Collections;
using UnityEngine;

public class LaStake : MonoBehaviour
{
    private enum EState
    {
        Normal,
        LotsOfBullets,
        Teleporting,
    }

    private static Array _stateValues;
    private Transform _bulletSpawnerTransform;
    private EState _state;

    public bool IsStopped;

    protected void Start()
    {
        _stateValues = Enum.GetValues(typeof(EState));
        _bulletSpawnerTransform = transform.Find("BulletSpawnerTransform");
    }

    protected void Update()
    {
        var newPositon = transform.localPosition;

        newPositon.y = Mathf.Sin(Time.time * 2.0F) / 2.0F;
        transform.localPosition = newPositon;

        if (IsStopped)
        {
            return;
        }

        if (_state == EState.LotsOfBullets)
        {
            _bulletSpawnerTransform.Rotate(Vector3.forward, 9.0F, Space.World);

            if ((int)(Time.time * 1000.0F) % 4 == 0)
            {
                var bullet = Instantiate(Resources.Prefabs.LaStakeBullet, Game.Instance.Playism.transform);
                var bulletComponent = bullet.GetComponent<LaStakeBullet>();

                bullet.transform.position = transform.position;
                bulletComponent.Direction = _bulletSpawnerTransform.transform.up;
            }
        }
    }

    public void StartBeingMean()
    {
        IsStopped = false;
        StartCoroutine(MeanCoroutine());
    }

    public void StopBeingMean()
    {
        IsStopped = true;
        StopCoroutine(MeanCoroutine());
    }

    private IEnumerator MeanCoroutine()
    {
        var shouldTeleport = _state != EState.Teleporting && !IsStopped;

        _state = EState.Normal;

        yield return new WaitForSeconds(1);

        _state = (EState)UnityEngine.Random.Range((int)EState.LotsOfBullets, _stateValues.Length);

        if (_state == EState.Teleporting && shouldTeleport)
        {
            transform.position = Util.RandomDirection * 4;
            Game.Instance.AudioSource.PlayOneShot(Resources.Audio.Teleport);
            yield return null;
            StartCoroutine(MeanCoroutine());
        }

        if (_state != EState.Teleporting)
        {
            yield return new WaitForSeconds(9);
        }

        StartCoroutine(MeanCoroutine());
    }
}
