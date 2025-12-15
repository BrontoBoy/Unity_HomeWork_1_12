using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour, IResettable
{
    [SerializeField] private float MinLifetime = 2f;
    [SerializeField] private float MaxLifetime = 5f;
    [SerializeField] private float ExplosionRadius = 5f;
    [SerializeField] private float ExplosionForce = 500f;
    [SerializeField] private Color BombColor = Color.black;

    private Renderer BombRenderer;
    private Rigidbody BombRigidbody;
    private Coroutine ExplodeCoroutine;
    private BombSpawner BombSpawner;
    private Material BombMaterial;
    
    private void Awake()
    {
        BombRenderer = GetComponent<Renderer>();
        BombRigidbody = GetComponent<Rigidbody>();
        BombMaterial = BombRenderer.material;
    }
    
    public void SetBombSpawner(BombSpawner spawner)
    {
        BombSpawner = spawner;
    }

    private void OnEnable()
    {
        Reset();
        StartCountdown();
    }

    public void Reset()
    {

        if (ExplodeCoroutine != null)
        {
            StopCoroutine(ExplodeCoroutine);
            ExplodeCoroutine = null;
        }
        
        BombRigidbody.linearVelocity = Vector3.zero;
        BombRigidbody.angularVelocity = Vector3.zero;
        Color resetColor = BombColor;
        resetColor.a = 1f;
        BombMaterial.color = resetColor;
    }

    private void StartCountdown()
    {
        float lifetime = Random.Range(MinLifetime, MaxLifetime);
        ExplodeCoroutine = StartCoroutine(ExplodeCountdownCoroutine(lifetime));
    }

    private IEnumerator ExplodeCountdownCoroutine(float lifetime)
    {
        float timer = 0f;
        
        while (timer < lifetime)
        {
            timer += Time.deltaTime;
            float progress = timer / lifetime;
            Color currentColor = BombMaterial.color;
            currentColor.a = 1f - progress;
            BombMaterial.color = currentColor;

            yield return null;
        }
        
        Explode();
    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, ExplosionRadius);

        foreach (Collider hit in colliders)
        {
            Rigidbody rigidbody = hit.attachedRigidbody;
            
            if (rigidbody != null && rigidbody != BombRigidbody)
            {
                rigidbody.AddExplosionForce(ExplosionForce, transform.position, ExplosionRadius, 
                    1f, ForceMode.Impulse);
            }
        }
        
        if (BombSpawner != null)
        {
            BombSpawner.ReturnBombToPool(this);
        }
    }
}