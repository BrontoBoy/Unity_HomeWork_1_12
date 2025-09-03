using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;


[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(Rigidbody))]
public class Cube : MonoBehaviour
{
    private const float MinLifetime = 2f;
    private const float MaxLifetime = 5f;

    [SerializeField] private Color _initialColor = Color.white;
    [SerializeField] private Color _touchedColor = Color.red;
    
    public event Action<Cube> Expired;
    
    private bool _hasTouchedPlatform = false;
    
    private Renderer _cubeRenderer;
    private Rigidbody _cubeRigidbody;
    private Coroutine _lifetimeCoroutine;
    
    private void Awake()
    {
        _cubeRenderer = GetComponent<Renderer>();
        _cubeRigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        Reset();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_hasTouchedPlatform == false && collision.gameObject.TryGetComponent(out Platform _))
        {
            HandlePlatformCollision();
        }
    }

    public void Reset()
    {
        _hasTouchedPlatform = false;
        _cubeRenderer.material.color = _initialColor;
        _cubeRigidbody.linearVelocity = Vector3.zero;
        _cubeRigidbody.angularVelocity = Vector3.zero;
        
        if (_lifetimeCoroutine != null)
        {
            StopCoroutine(_lifetimeCoroutine);
            _lifetimeCoroutine = null;
        }
    }
    
    private void HandlePlatformCollision()
    {
        _hasTouchedPlatform = true;
        _cubeRenderer.material.color = _touchedColor;
        float lifetime = Random.Range(MinLifetime, MaxLifetime);
        _lifetimeCoroutine = StartCoroutine(LifetimeCountdown(lifetime));
    }

    private IEnumerator LifetimeCountdown(float lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        
        Expired?.Invoke(this);
    }
}
