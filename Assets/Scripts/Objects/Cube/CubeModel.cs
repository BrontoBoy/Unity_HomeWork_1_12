using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class CubeModel : IResettable
{
    private float _minLifetime = 2f;
    private float _maxLifetime = 5f;
    
    private bool _hasTouchedPlatform = false;
    private float _currentLifetime;
    
    public event Action<Vector3> Expired;
    public event Action PlatformTouched;
    public event Action ResetCompleted; 
    
    public float MinLifetime => _minLifetime;
    public float MaxLifetime => _maxLifetime;
    public bool HasTouchedPlatform => _hasTouchedPlatform;
    
    
    public CubeModel(float minLifetime = 2f, float maxLifetime = 5f)
    {
        _minLifetime = minLifetime;
        _maxLifetime = maxLifetime;
        Reset();
    }
    
    public void Reset()
    {
        _hasTouchedPlatform = false;
        ResetCompleted?.Invoke();
    }
    
    public void HandlePlatformCollision(Vector3 position)
    {
        if (_hasTouchedPlatform) 
            return;
        
        _hasTouchedPlatform = true;
        PlatformTouched?.Invoke();
        _currentLifetime = Random.Range(_minLifetime, _maxLifetime);
    }
    
    public void CompleteLifetime(Vector3 position)
    {
        Expired?.Invoke(position);
    }
}