using UnityEngine;

public abstract class Mob : MonoBehaviour
{
    protected Health Health;
    protected Collider2DGameObject HurtBoxCollider2DGameObject;

    [SerializeField]
    protected GameObject HurtBox;

    protected void Start()
    {
        Health = GetComponent<Health>();
        Health.Damaged.AddListener(OnDamaged);
        HurtBoxCollider2DGameObject = HurtBox.GetComponent<Collider2DGameObject>();
        HurtBoxCollider2DGameObject.TriggerEntered2D.AddListener(OnTriggerEntered2D);
    }

    protected virtual void OnDamaged()
    {
        if (Health.GetHealth() <= 0.0F)
        {
            Game.Instance.AudioSource.PlayOneShot(Resources.Audio.Explosion1);
            Destroy(gameObject);
        }
    }

    protected virtual void OnTriggerEntered2D(Collider2D other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            Health.Damage(Game.CalculateDamage(other.gameObject, gameObject));
        }
    }
}
