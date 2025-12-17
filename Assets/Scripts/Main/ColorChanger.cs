using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;
    
    private Color _startValue;
    private Color _targetValue;
    private bool _isChanged = false;
    
    public bool IsChanged => _isChanged;

    private void Awake()
    {
        if (_renderer == null)
            _renderer = GetComponent<Renderer>();
            
        _startValue = _renderer.material.color; 
    }
    
    public void ChangeColor(Color newColor)
    {
        if (_isChanged) 
            return;
        
        _targetValue = newColor;
        _renderer.material.color = _targetValue;
        _isChanged = true;
    }
    
    public void ResetColor()
    {
        _renderer.material.color = _startValue;
        _isChanged = false;
    }
}