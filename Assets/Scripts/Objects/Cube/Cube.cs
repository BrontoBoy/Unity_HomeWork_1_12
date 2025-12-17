using UnityEngine;

[RequireComponent(typeof(ColorChanger))]
[RequireComponent(typeof(Timer))]
[RequireComponent(typeof(CollisionHandler))]
[RequireComponent(typeof(Rigidbody))]
public class Cube : MonoBehaviour, IResettable
{
    [SerializeField] private Color _touchedColor = Color.red;
    
    private ColorChanger _colorChanger;
    private Timer _timer;
    private CollisionHandler _collisionHandler;
    
    public event System.Action<Cube, Vector3> CubeExpired;
    
    private void Awake()
    {
        _colorChanger = GetComponent<ColorChanger>();
        _timer = GetComponent<Timer>();
        _collisionHandler = GetComponent<CollisionHandler>();
        
        _collisionHandler.Touched += HandleTouch;
        _timer.TimeOver += HandleTimeOver;
    }
    
    private void OnEnable()
    {
        Reset();
    }
    
    private void OnDestroy()
    {
        if (_collisionHandler != null)
            _collisionHandler.Touched -= HandleTouch;
            
        if (_timer != null)
            _timer.TimeOver -= HandleTimeOver;
    }
    
    public void Reset()
    {
        _colorChanger.ResetColor();
        _timer.ResetTimer();
        _collisionHandler.ResetState();
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        
        if (rigidbody != null)
        {
            rigidbody.linearVelocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
        }
    }
    
    private void HandleTouch()
    {
        _colorChanger.ChangeColor(_touchedColor);
        _timer.StartTimer();
    }
    
    private void HandleTimeOver()
    {
        CubeExpired?.Invoke(this, transform.position);
    }
}