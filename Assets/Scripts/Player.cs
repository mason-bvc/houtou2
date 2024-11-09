using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector2 _moveAxes;
    private Vector2 _velocity;
    private Transform _aimTransform;
    private Transform _bulletSpawnTransform;
    private Collider2D _collider;
    private GameObject _bulletPrefab;

    [SerializeField]
    private float _speed = 0.25F;

    [SerializeField]
    private float _deceleration = 2.0F;

    protected void Start()
    {
        _aimTransform = transform.Find("AimTransform");
        _bulletSpawnTransform = _aimTransform?.Find("BulletSpawnTransform");
        _collider = transform.Find("Body")?.GetComponent<Collider2D>();
        _bulletPrefab = Game.AssetBundle.LoadAsset<GameObject>("PlayerBullet");
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

        if (Input.GetButtonDown("Fire1"))
        {
            var bulletGameObj = Instantiate(_bulletPrefab);
            var bulletComponent = bulletGameObj.GetComponent<Bullet>();

            bulletGameObj.transform.position = _bulletSpawnTransform.position;
            bulletComponent.Direction = _aimTransform.up;
        }
    }
}
