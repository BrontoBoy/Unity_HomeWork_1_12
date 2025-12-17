using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class CubeView : MonoBehaviour
{
    [SerializeField] private Color _initialColor = Color.white;
    [SerializeField] private Color _touchedColor = Color.red;
    
    private Renderer _renderer;
    
    public Vector3 Position
    {
        get => transform.position;
        set => transform.position = value;
    }
    
    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        SetInitialColor();
    }

    public void SetInitialColor()
    {
        _renderer.material.color = _initialColor;
    }
    
    public void SetTouchedColor()
    {
        _renderer.material.color = _touchedColor;
    }
}