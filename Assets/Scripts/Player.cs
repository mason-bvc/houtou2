using UnityEngine;

public class Player : MonoBehaviour
{
    private Sprite[] _sprites;
    private int _spriteRow;
    private Vector2 _moveAxes;
    private Vector2 _velocity;
    private Transform _aimTransform;
    private Transform _bulletSpawnTransform;
    private Rigidbody2D _body;
    private SpriteRenderer _spriteRenderer;
    private PlayerSprite _playerSprite;
    private GameObject _bulletPrefab;

    [SerializeField]
    private float _speed = 0.25F;

    [SerializeField]
    private float _deceleration = 2.0F;

    protected void Start()
    {
        _sprites = Game.AssetBundle.LoadAssetWithSubAssets<Sprite>("Corinne");
        _aimTransform = transform.Find("AimTransform");
        _bulletSpawnTransform = _aimTransform?.Find("BulletSpawnTransform");

        Transform sprite = transform.Find("Sprite");

        _spriteRenderer = sprite?.GetComponent<SpriteRenderer>();
        _playerSprite = sprite?.GetComponent<PlayerSprite>();
        _bulletPrefab = Game.AssetBundle.LoadAsset<GameObject>("PlayerBullet");

        Transform hurtBox = transform.Find("HurtBox");
        _body = hurtBox?.GetComponent<Rigidbody2D>();
    }

    protected void Update()
    {
        Vector2 moveVector = _moveAxes.normalized * _speed;

        _moveAxes = new(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _velocity += moveVector;
        transform.Translate(_velocity * Time.deltaTime);
        _velocity = Vector2.Lerp(_velocity, Vector2.zero, _deceleration * Time.deltaTime);

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        mousePos.z = 0;
        _aimTransform.up = mousePos - transform.position;

        float a = Vector3.SignedAngle(Vector3.up, _aimTransform.up, Vector3.forward);

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
            var bulletGameObj = Instantiate(_bulletPrefab);
            var bulletComponent = bulletGameObj.GetComponent<Bullet>();

            bulletGameObj.transform.position = _bulletSpawnTransform.position;
            bulletComponent.Direction = _aimTransform.up;
        }

        _spriteRenderer.sprite = _sprites[_spriteRow * 3 + _playerSprite.Frame];
    }
}
