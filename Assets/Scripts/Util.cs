using UnityEngine;

public static class Util
{
    public static Vector2 RandomDirection => (Quaternion.AngleAxis(Random.value * Mathf.PI * 2 * Mathf.Rad2Deg, Vector3.forward) * Vector2.up).normalized;
}
