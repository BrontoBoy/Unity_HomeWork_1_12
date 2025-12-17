using System;
using UnityEngine;

[RequireComponent(typeof(BombView))]
[RequireComponent(typeof(BombMovement))]
public class BombPresenter : MonoBehaviour, IResettable
{
    private const float UpwardsModifier = 1f;
    private const float FullAlpha = 1f;
    private const float MaxLifetimeBeforeForceExplode = 10f;
    private const float ResetTimerValue = 0f;
    
    private float _timeAlive = ResetTimerValue;
    private BombModel _model;
    private BombView _view;
    private BombMovement _movement;
    private bool _isExploded = false;
    
    public event Action<BombPresenter> BombExploded;
    
    private void Update()
    {
        _timeAlive += Time.deltaTime;
        
        if (_timeAlive >= MaxLifetimeBeforeForceExplode)
        {
            HandleExploded();
        }
    }
    
    private void FixedUpdate()
    {
        if (_model != null && _isExploded == false)
        {
            _model.Update(Time.fixedDeltaTime);
        }
    }
    
    private void OnDestroy()
    {
        UnsubscribeFromModelEvents();
    }
    
    private void UnsubscribeFromModelEvents()
    {
        if (_model != null)
        {
            _model.Exploded -= HandleExploded;
            _model.AlphaChanged -= HandleAlphaChanged;
            _model.ResetCompleted -= HandleResetCompleted;
        }
    }
    
    public void Initialize(BombModel model)
    {
        _isExploded = false;
        _timeAlive = ResetTimerValue;
        
        UnsubscribeFromModelEvents();
        
        _model = model;
        _view = GetComponent<BombView>();
        _movement = GetComponent<BombMovement>();
        
        _model.Exploded += HandleExploded;
        _model.AlphaChanged += HandleAlphaChanged;
        _model.ResetCompleted += HandleResetCompleted;
        
        _model.StartCountdown();
    }
    
    public void Reset()
    {
        _isExploded = false;
        _timeAlive = ResetTimerValue;
        _model?.Reset();
    }
    
    private void HandleExploded()
    {
        if (_isExploded) 
            return;
        
        _isExploded = true;
        ApplyExplosionForce();
        BombExploded?.Invoke(this);
        UnsubscribeFromModelEvents();
    }
    
    private void HandleAlphaChanged(float alpha)
    {
        _view.SetAlpha(alpha);
    }
    
    private void HandleResetCompleted()
    {
        _view.SetAlpha(FullAlpha);
        _movement.ResetMovement();
    }
    
    private void ApplyExplosionForce()
    {
        Collider[] colliders = Physics.OverlapSphere(_view.Position, _model.ExplosionRadius);
        
        foreach (Collider hit in colliders)
        {
            Rigidbody rigidbody = hit.attachedRigidbody;
            
            if (rigidbody == null) 
                continue;

            if (rigidbody == _movement.Rigidbody) 
                continue;
            
            rigidbody.AddExplosionForce(_model.ExplosionForce, _view.Position, _model.ExplosionRadius, 
                UpwardsModifier, ForceMode.Impulse);
        }
    }
}