using UnityEngine;

public class Background : MonoBehaviour
{
    private Color _currentColor = Color.blue / 4.0F;
    private Color _targetColor;
    private Renderer _renderer;

    public Color Color
    {
        get => _targetColor;
        set => _targetColor = value;
    }

    protected void Start()
    {
        _renderer = GetComponent<Renderer>();
        _targetColor = _currentColor;
    }

    protected void Update()
    {
        _currentColor.r = Mathf.Lerp(_currentColor.r, _targetColor.r, Time.deltaTime);
        _currentColor.g = Mathf.Lerp(_currentColor.g, _targetColor.g, Time.deltaTime);
        _currentColor.b = Mathf.Lerp(_currentColor.b, _targetColor.b, Time.deltaTime);
        _renderer.sharedMaterial.SetColor("_MainColor", _currentColor);
    }
}
