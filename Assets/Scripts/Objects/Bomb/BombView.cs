using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class BombView : MonoBehaviour
{
    private const float DefaultAlpha = 1f;
    
    [SerializeField] private Color _bombColor = Color.black;
    
    private Renderer _renderer;
    private Material _material;
    
    public Vector3 Position
    {
        get => transform.position;
        set => transform.position = value;
    }
    
    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _material = new Material(_renderer.material);
        _renderer.material = _material;
        SetAlpha(DefaultAlpha);
    }
    
    public void SetAlpha(float alpha)
    {
        Color color = _bombColor;
        color.a = Mathf.Clamp01(alpha);
        _material.color = color;
    }
}