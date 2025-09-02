using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class CubePool : MonoBehaviour
{
    [SerializeField] private Cube _cubePrefab;
    [SerializeField] private int _defaultCapacity = 20;
    [SerializeField] private int _maxPoolSize = 100;

    private ObjectPool<Cube> _objectPool;
    private readonly List<Cube> _activeCubes = new List<Cube>();

    private void Awake()
    {
        Initialize();
    }

    private void OnDestroy()
    {
        Cleanup();
    }

    public Cube GetCube()
    {
        if (_objectPool == null)
        {
            return null;
        }
        
        try
        {
            Cube cube = _objectPool.Get();
            _activeCubes.Add(cube);
            
            return cube;
        }
        catch (System.Exception e)
        {
            return null;
        }
    }

    public void ReturnCube(Cube cube)
    {
        if (cube == null) return;
        
        if (_activeCubes.Contains(cube))
        {
            _activeCubes.Remove(cube);
            _objectPool.Release(cube);
        }
    }

    private void Initialize()
    {
        _objectPool = new ObjectPool<Cube>(
            createFunc: CreateCube,
            actionOnGet: OnTakeFromPool,
            actionOnRelease: OnReturnToPool,
            actionOnDestroy: OnDestroyCube,
            collectionCheck: true,
            defaultCapacity: _defaultCapacity,
            maxSize: _maxPoolSize
        );

        PrewarmPool();
    }

    private Cube CreateCube()
    {
        if (_cubePrefab == null)
        {
            return null;
        }
        
        Cube newCube = Instantiate(_cubePrefab);
        newCube.Expired += HandleCubeLifeTimeExpired;
        newCube.gameObject.SetActive(false);
        
        return newCube;
    }

    private void OnTakeFromPool(Cube cube)
    {
        if (cube == null)
        {
            return;
        }
        
        cube.gameObject.SetActive(true);
        cube.transform.position = GetRandomSpawnPosition();
    }

    private void OnReturnToPool(Cube cube)
    {
        if (cube == null)
        {
            return;
        }
        
        cube.gameObject.SetActive(false);
        cube.Reset();
    }

    private void OnDestroyCube(Cube cube)
    {
        if (cube == null)
        {
            return;
        }
        
        cube.Expired -= HandleCubeLifeTimeExpired;
        Destroy(cube.gameObject);
    }

    private void PrewarmPool()
    {
        List<Cube> prewarmCubes = new List<Cube>();
        
        for (int i = 0; i < _defaultCapacity; i++)
        {
            Cube cube = _objectPool.Get();
            
            if (cube != null)
            {
                prewarmCubes.Add(cube);
            }
        }
        
        foreach (Cube cube in prewarmCubes)
        {
            _objectPool.Release(cube);
        }
    }

    private void Cleanup()
    {
        foreach (Cube cube in _activeCubes.ToArray())
        {
            ReturnCube(cube);
        }
        
        _objectPool?.Clear();
    }

    private void HandleCubeLifeTimeExpired(Cube cube)
    {
        ReturnCube(cube);
    }

    private Vector3 GetRandomSpawnPosition()
    {
        const float spawnPositionY = 15f;
        const float spawnAreaHalfSize = 10f;
        
        return new Vector3(
            Random.Range(-spawnAreaHalfSize, spawnAreaHalfSize), spawnPositionY, Random.Range(-spawnAreaHalfSize, spawnAreaHalfSize)
        );
    }
}