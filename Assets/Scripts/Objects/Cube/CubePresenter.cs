using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(CubeView))]
[RequireComponent(typeof(CubeMovement))]
public class CubePresenter : MonoBehaviour, IResettable
{
    private const float LifetimeVariation = 0.1f;
    private const float UpwardsModifier = 1f;
    
    private CubeModel _model;
    private CubeView _view;
    private CubeMovement _movement;
    private Coroutine _lifetimeCoroutine;
    
    public event Action<CubePresenter, Vector3> CubeExpired;
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Platform>(out _))
        {
            _model.HandlePlatformCollision(_view.Position);
        }
    }
    
    private void OnDestroy()
    {
        if (_model != null)
        {
            _model.PlatformTouched -= HandlePlatformTouched;
            _model.Expired -= HandleExpired;
            _model.ResetCompleted -= HandleResetCompleted;
        }
    }
    
    public void Initialize(CubeModel model)
    {
        _model = model;
        _view = GetComponent<CubeView>();
        _movement = GetComponent<CubeMovement>();
        
        _model.PlatformTouched += HandlePlatformTouched;
        _model.Expired += HandleExpired;
        _model.ResetCompleted += HandleResetCompleted;
        
        Reset();
    }
    
    public void Reset()
    {
        _model?.Reset();
    }
    
    private void HandlePlatformTouched()
    {
        _view.SetTouchedColor();
        
        if (_lifetimeCoroutine != null)
        {
            StopCoroutine(_lifetimeCoroutine);
        }
        
        _lifetimeCoroutine = StartCoroutine(LifetimeCoroutine());
    }
    
    private void HandleExpired(Vector3 position)
    {
        CubeExpired?.Invoke(this, position);
    }
    
    private void HandleResetCompleted()
    {
        _view.SetInitialColor();
        _movement.ResetMovement();
        
        if (_lifetimeCoroutine != null)
        {
            StopCoroutine(_lifetimeCoroutine);
            _lifetimeCoroutine = null;
        }
    }
    
    private IEnumerator LifetimeCoroutine()
    {
        float minLifetime = _model.MinLifetime * (UpwardsModifier - LifetimeVariation);
        float maxLifetime = _model.MaxLifetime * (UpwardsModifier + LifetimeVariation);
        float waitTime = Random.Range(minLifetime, maxLifetime);
        
        yield return new WaitForSeconds(waitTime);
        
        _model.CompleteLifetime(_view.Position);
    }
}