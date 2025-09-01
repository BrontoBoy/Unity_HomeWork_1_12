using System.Collections;
using UnityEngine;

public class Cube : MonoBehaviour
{
    private const float MinLifetime = 2f;
    private const float MaxLifetime = 5f;

    [SerializeField] private Color _initialColor = Color.white;
    [SerializeField] private Color _touchedColor = Color.red;

    private Renderer _cubeRenderer;
    private bool _hasTouchedPlatform = false;
    private CubePool _cubePool;
    private WaitForSeconds _waitForLifetime;
    
    private void Awake()
    {
        _cubeRenderer = GetComponent<Renderer>();
    }

    private void OnEnable()
    {
        _hasTouchedPlatform = false;
        _cubeRenderer.material.color = _initialColor;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Platform _) && _hasTouchedPlatform == false)
        {
            HandlePlatformCollision();
        }
    }
    
    public void Initialize(CubePool cubePool)
    {
        _cubePool = cubePool;
    }
    
    private void HandlePlatformCollision()
    {
        _cubeRenderer.material.color = _touchedColor;
        _hasTouchedPlatform = true;
        
        if (_waitForLifetime == null)
        {
            float lifetime = Random.Range(MinLifetime, MaxLifetime);
            _waitForLifetime = new WaitForSeconds(lifetime);
        }

        StartCoroutine(CountdownToDeactivateCoroutine());
    }

    private IEnumerator CountdownToDeactivateCoroutine()
    {
        yield return _waitForLifetime;

        if (_cubePool != null)
        {
            _cubePool.ReturnCubeToPool(this);
        }
        else
        {
            Debug.LogWarning("Ссылка на CubePool отсутствует! Уничтожаю объект.");
            Destroy(gameObject);
        }
    }
    
    public void ResetCube()
    {
        _hasTouchedPlatform = false;
        _cubeRenderer.material.color = _initialColor;
        StopAllCoroutines();
    }
}