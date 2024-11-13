using UnityEngine;
using UnityEngine.UI;

public class ScreenFlash : MonoBehaviour
{
    private RawImage _image;
    private Color _currentColor = new Color(0, 0, 0, 0);
    private Color _targetColor;

    public Color Color
    {
        get => _targetColor;
        set => _targetColor = value;
    }

    protected void Update()
    {
        _currentColor.r = Mathf.Lerp(_currentColor.r, _targetColor.r, Time.deltaTime);
        _currentColor.g = Mathf.Lerp(_currentColor.g, _targetColor.g, Time.deltaTime);
        _currentColor.b = Mathf.Lerp(_currentColor.b, _targetColor.b, Time.deltaTime);
        _currentColor.a = Mathf.Lerp(_currentColor.a, _targetColor.a, Time.deltaTime);

        _image.color = _currentColor;
    }

    protected void Start()
    {
        _image = GetComponent<RawImage>();
        _targetColor = _currentColor;
    }
}
