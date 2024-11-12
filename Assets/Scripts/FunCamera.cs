using System.Collections;
using UnityEngine;

public class FunCamera : MonoBehaviour
{
    private static Vector3 _cameraZero = new Vector3(0, 0, -10);
    private bool _isShaking;

    protected void Update()
    {
        if (_isShaking)
        {
            transform.position = _cameraZero + Vector3.right * Random.value / 10.0F;
        }
    }

    public void Shake()
    {
        StartCoroutine(DoShakeStuff());
    }

    private IEnumerator DoShakeStuff()
    {
        _isShaking = true;
        yield return new WaitForSeconds(0.25F);
        _isShaking = false;
        transform.position = _cameraZero;
    }
}
