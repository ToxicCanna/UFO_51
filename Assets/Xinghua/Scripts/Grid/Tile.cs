using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color _baseColor, _offsetColor;
    public Color targetColor;
    [SerializeField] private SpriteRenderer _renderer;
    private void Awake()
    {
        if (_renderer == null)
        {
            _renderer = GetComponent<SpriteRenderer>();
        }
    }
    public void SetColorAlpha()
    {
        _baseColor.a = 1.0f;
        _offsetColor.a = 1.0f;
    }
    public void InitialGridColor(bool isOffset)
    {
        //Debug.Log("grid color");
        _renderer.color = isOffset ? _offsetColor : _baseColor;
    }
}
