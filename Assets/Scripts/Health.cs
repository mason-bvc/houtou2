using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    private float _health;

    [SerializeField]
    public float InitialHealth = 100.0F;

    public UnityEvent Damaged;

    protected void Start()
    {
        _health = InitialHealth;
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
