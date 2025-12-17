using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BombModel : IResettable
{
    private const float AlphaChangeThreshold = 0.01f;
    private const float FullAlpha = 1f;
    private const float ZeroAlpha = 0f;
    private const float ZeroLifetime = 0f;
    
    private float _minLifetime = 2f;
    private float _maxLifetime = 5f;
    private float _explosionRadius = 5f;
    private float _explosionForce = 500f;
    private float _currentLifetime;
    private float _currentAlpha = FullAlpha;
    private float _totalLifetime;
    private bool _hasExploded = false;
    
    public event Action Exploded;
    public event Action<float> AlphaChanged;
    public event Action ResetCompleted;
    
    public float ExplosionRadius => _explosionRadius;
    public float ExplosionForce => _explosionForce;
    
    public BombModel(float minLifetime = 2f, float maxLifetime = 5f, float explosionRadius = 5f, 
        float explosionForce = 500f)
    {
        _minLifetime = minLifetime;
        _maxLifetime = maxLifetime;
        _explosionRadius = explosionRadius;
        _explosionForce = explosionForce;
        Reset();
    }
    
    public void Reset()
    {
        _currentAlpha = FullAlpha;
        _hasExploded = false;
        ResetCompleted?.Invoke();
    }
    
    public void StartCountdown()
    {
        _hasExploded = false;
        _totalLifetime = Random.Range(_minLifetime, _maxLifetime);
        _currentLifetime = _totalLifetime;
    }
    
    public void Update(float deltaTime)
    {
        if (_hasExploded)
            return;

        if (_currentLifetime > ZeroLifetime)
        {
            _currentLifetime -= deltaTime;
            
            float progress = Mathf.Clamp01(_currentLifetime / _totalLifetime);
            float newAlpha = progress;

            if (Mathf.Abs(newAlpha - _currentAlpha) > AlphaChangeThreshold)
            {
                _currentAlpha = newAlpha;
                AlphaChanged?.Invoke(_currentAlpha);
            }
            
            if (_currentLifetime <= ZeroLifetime)
            {
                _hasExploded = true;
                
                if (_currentAlpha > ZeroAlpha)
                {
                    _currentAlpha = ZeroAlpha;
                    AlphaChanged?.Invoke(_currentAlpha);
                }
                Exploded?.Invoke();
            }
        }
        else if (_hasExploded == false)
        {
            _hasExploded = true;
            Exploded?.Invoke();
        }
    }
    
    public void TriggerExplosion()
    {
        if (_hasExploded == false)
        {
            _hasExploded = true;
            Exploded?.Invoke();
        }
    }
}