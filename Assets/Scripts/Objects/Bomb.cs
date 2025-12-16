using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Bomb : MonoBehaviour, IResettable
{
    [SerializeField] private float _minLifetime = 2f;
    [SerializeField] private float _maxLifetime = 5f;
    [SerializeField] private float _explosionRadius = 5f;
    [SerializeField] private float _explosionForce = 500f;
    [SerializeField] private Color _bombColor = Color.black;

    private Renderer _bombRenderer;
    private Rigidbody _bombRigidbody;
    private Coroutine _explodeCoroutine;
    
    public event Action<Bomb> BombExploded;
    
    private void Awake()
    {
        _bombRenderer = GetComponent<Renderer>();
        _bombRigidbody = GetComponent<Rigidbody>();
        _bombRenderer.material.color = _bombColor;
    }
    
    private void OnEnable()
    {
        Reset();
        StartCountdown();
    }

    public void Reset()
    {

        if (_explodeCoroutine != null)
        {
            StopCoroutine(_explodeCoroutine);
            _explodeCoroutine = null;
        }

        _bombRigidbody.linearVelocity = Vector3.zero;
        _bombRigidbody.angularVelocity = Vector3.zero;
        Material material = _bombRenderer.material;
        Color currentColor = material.color;
        currentColor.a = 1f;
        material.color = currentColor;
    }

    private void StartCountdown()
    {
        float lifetime = Random.Range(_minLifetime, _maxLifetime);
        _explodeCoroutine = StartCoroutine(ExplodeCountdownCoroutine(lifetime));
    }

    private IEnumerator ExplodeCountdownCoroutine(float lifetime)
    {
        float timer = 0f;
        Material material = _bombRenderer.material;
        
        while (timer < lifetime)
        {
            timer += Time.deltaTime;
            float progress = timer / lifetime;
            Color currentColor = material.color;
            currentColor.a = 1f - progress;
            material.color = currentColor;

            yield return null;
        }
        
        Explode();
    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionRadius);

        foreach (Collider hit in colliders)
        {
            Rigidbody rigidbody = hit.attachedRigidbody;
            
            if (rigidbody != null && rigidbody != _bombRigidbody)
            {
                rigidbody.AddExplosionForce(_explosionForce, transform.position, _explosionRadius, 1f,
                    ForceMode.Impulse);
            }
        }
        
        BombExploded?.Invoke(this);
    }
}