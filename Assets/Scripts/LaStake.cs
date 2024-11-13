using UnityEngine;

public class LaStake : MonoBehaviour
{
    protected void Update()
    {
        var newPositon = transform.localPosition;

        newPositon.y = Mathf.Sin(Time.time * 2.0F) / 2.0F;
        transform.localPosition = newPositon;
    }
}
