using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Sprite[] _sprites;

    private int _spriteRow;
    private Vector2 _moveAxes;
    private Vector2 _velocity;
    private Transform _bulletSpawnTransform;
    private Rigidbody2D _body;
    private SpriteRenderer _spriteRenderer;
    private AudioSource _audioSource;
    private PlayerSprite _playerSprite;
    private bool _isInvincible;

    public Transform AimTransform;
    public Health Health;
    public bool IsStopped;

    [SerializeField]
    private float _speed = 0.25F;

    [SerializeField]
    private float _deceleration = 2.0F;

    protected void Start()
    {
        _sprites = Game.AssetBundle.LoadAssetWithSubAssets<Sprite>("Corinne");
        AimTransform = transform.Find("AimTransform");
        _bulletSpawnTransform = AimTransform?.Find("BulletSpawnTransform");
        _audioSource = GetComponent<AudioSource>();
        _body = GetComponent<Rigidbody2D>();
        Health = GetComponent<Health>();
        Health.Damaged.AddListener(OnDamaged);

        Transform sprite = transform.Find("Sprite");

        _spriteRenderer = sprite?.GetComponent<SpriteRenderer>();
        _playerSprite = sprite?.GetComponent<PlayerSprite>();

        var hurtBox = transform.Find("HurtBox");

        _body = hurtBox?.GetComponent<Rigidbody2D>();

        var collider2d = hurtBox?.GetComponent<Collider2D>();
        var collider2dGameObj = hurtBox?.GetComponent<Collider2DGameObject>();

        collider2dGameObj?.TriggerEntered2D.AddListener((Collider2D other) =>
        {
            if (!_isInvincible)
            {
                Health.Damage(Game.CalculateDamage(other.gameObject, gameObject));
                Game.Instance.AudioSource.PlayOneShot(Resources.Audio.PlayerHurt);
            }
        });
    }

    protected void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backslash))
        {
            Health.Heal(100);
        }

        if (!IsStopped)
        {
            Vector2 moveVector = _moveAxes.normalized * _speed;

            _moveAxes = new(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            _velocity += moveVector;
        }

        transform.Translate(_velocity * Time.deltaTime);

        if (Mathf.Abs(transform.position.x) > 5F)
        {
            var newPosition = transform.position;

            newPosition.x = -transform.position.x;
            transform.position = newPosition;
        }

        if (Mathf.Abs(transform.position.y) > 4F)
        {
            var newPosition = transform.position;

            newPosition.y = -transform.position.y;
            transform.position = newPosition;
        }

        _velocity = Vector2.Lerp(_velocity, Vector2.zero, _deceleration * Time.deltaTime);

        if (!IsStopped)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            mousePos.z = 0;
            AimTransform.up = mousePos - transform.position;
        }

        float a = Vector3.SignedAngle(Vector3.up, AimTransform.up, Vector3.forward);

        a += 180.0F;
        a -= 45.0F;
        a /= 90.0F;

        var aa = (int)a;

        if (aa == 1)
        {
            _spriteRow = 2;
            _spriteRenderer.flipX = false;
        }
        else if (aa == 0)
        {
            _spriteRow = 1;
            _spriteRenderer.flipX = false;
        }
        else if (aa == 3)
        {
            _spriteRow = 0;
            _spriteRenderer.flipX = false;
        }
        else if (aa == 2)
        {
            _spriteRow = 1;
            _spriteRenderer.flipX = true;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            var bulletGameObj = Instantiate(Resources.Prefabs.PlayerBullet, Game.Instance.Playism.transform);
            var bulletComponent = bulletGameObj.GetComponent<Bullet>();

            bulletGameObj.transform.position = _bulletSpawnTransform.position;
            bulletComponent.Direction = AimTransform.up;
            _audioSource.PlayOneShot(Resources.Audio.PlayerShoot);
        }

        _spriteRenderer.sprite = _sprites[_spriteRow * 3 + _playerSprite.Frame];
    }

    private void OnDamaged()
    {
        StartCoroutine(HandlePostDamaged());
    }

    private IEnumerator HandlePostDamaged()
    {
        _isInvincible = true;
        _spriteRenderer.color = new Color(1.0F, 1.0F, 1.0F, 0.5F);
        yield return new WaitForSeconds(3.0F);
        _isInvincible = false;
        _spriteRenderer.color = new Color(1.0F, 1.0F, 1.0F, 1.0F);
    }
}
