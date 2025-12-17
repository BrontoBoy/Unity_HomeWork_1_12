using UnityEngine;

[RequireComponent(typeof(FadeEffect))]
[RequireComponent(typeof(Explosion))]
[RequireComponent(typeof(Timer))]
[RequireComponent(typeof(Rigidbody))]
public class Bomb : MonoBehaviour, IResettable
{
    private const float MinLifetime = 2f;
    private const float MaxLifetime = 5f;
    
    [SerializeField] private Color _color = Color.black;
    [SerializeField] private float _minLifetime = MinLifetime;
    [SerializeField] private float _maxLifetime = MaxLifetime;
    
    private FadeEffect _fadeEffect;
    private Explosion _explosion;
    private Timer _timer;
    private Renderer _renderer;
    
    public event System.Action<Bomb> Exploded;
    
    private void Awake()
    {
        _fadeEffect = GetComponent<FadeEffect>();
        _explosion = GetComponent<Explosion>();
        _timer = GetComponent<Timer>();
        _renderer = GetComponent<Renderer>();
        _renderer.material.color = _color;
        
        _timer.TimeOver += HandleTimeOver; 
    }
    
    private void OnEnable()
    {
        Reset();
        StartCountdown();
    }
    
    private void OnDestroy()
    {
        if (_timer != null)
            _timer.TimeOver -= HandleTimeOver;
    }
    
    public void Reset()
    {
        _fadeEffect.ResetVisuals();
        _timer.ResetTimer();
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        
        if (rigidbody != null)
        {
            rigidbody.linearVelocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
        }
        
        _renderer.material.color = _color;
    }
    
    public void ExplodeNow()
    {
        TriggerExplosion();
    }
    
    private void StartCountdown()
    {
        _timer.StartTimer();
        float lifetime = Random.Range(_minLifetime, _maxLifetime);
        _fadeEffect.StartFade(lifetime);
    }
    
    private void HandleTimeOver()
    {
        TriggerExplosion();
    }
    
    private void TriggerExplosion()
    {
        _explosion.Explode();
        
        Exploded?.Invoke(this);
    }
}