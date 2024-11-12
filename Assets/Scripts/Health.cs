using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    private float _health;

    [SerializeField]
    private float _initialHealth = 100.0F;

    public UnityEvent Damaged;

    protected void Start()
    {
        _health = _initialHealth;
    }

    public float GetHealth()
    {
        return _health;
    }

    public void Damage(float amount)
    {
        _health -= amount;
        Damaged.Invoke();
    }
}
