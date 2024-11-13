using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    private float _health;

    [SerializeField]
    public float InitialHealth = 100.0F;

    public UnityEvent Damaged;
    public UnityEvent Healed;

    protected void Start()
    {
        _health = InitialHealth;
    }

    public float GetHealth()
    {
        return _health;
    }

    public void SetHealth(float value)
    {
        _health = value;
    }

    public void Heal(float amount)
    {
        _health += amount;
        _health = Mathf.Min(_health, InitialHealth);
        Healed.Invoke();
    }

    public void Damage(float amount)
    {
        _health -= amount;
        Damaged.Invoke();
    }
}
