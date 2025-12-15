using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(Rigidbody))]
public class Cube : MonoBehaviour, IResettable
{
    private const float MinLifetime = 2f;
    private const float MaxLifetime = 5f;

    [SerializeField] private Color InitialColor = Color.white;
    [SerializeField] private Color TouchedColor = Color.red;

    private bool HasTouchedPlatform = false;
    private Renderer CubeRenderer;
    private Rigidbody CubeRigidbody;
    private Coroutine LifetimeCoroutine;
    private CubeSpawner CubeSpawner;
    
    private void Awake()
    {
        CubeRenderer = GetComponent<Renderer>();
        CubeRigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        Reset();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (HasTouchedPlatform == false && collision.gameObject.TryGetComponent(out Platform _))
        {
            HandlePlatformCollision();
        }
    }

    public void SetCubeSpawner(CubeSpawner spawner)
    {
        CubeSpawner = spawner;
    }
    
    public void Reset()
    {
        HasTouchedPlatform = false;
        CubeRenderer.material.color = InitialColor;
        CubeRigidbody.linearVelocity = Vector3.zero;
        CubeRigidbody.angularVelocity = Vector3.zero;

        if (LifetimeCoroutine != null)
        {
            StopCoroutine(LifetimeCoroutine);
            LifetimeCoroutine = null;
        }
    }

    private void HandlePlatformCollision()
    {
        if (HasTouchedPlatform) return;

        HasTouchedPlatform = true;
        CubeRenderer.material.color = TouchedColor;
        float lifetime = Random.Range(MinLifetime, MaxLifetime);
        LifetimeCoroutine = StartCoroutine(CountdownLifetimeCoroutine(lifetime));
    }

    private IEnumerator CountdownLifetimeCoroutine(float lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        
        if (CubeSpawner != null)
        {
            CubeSpawner.CreateBombAtPosition(transform.position);
        }
        
        if (CubeSpawner != null)
        {
            CubeSpawner.ReturnToPool(this);
        }
    }
}