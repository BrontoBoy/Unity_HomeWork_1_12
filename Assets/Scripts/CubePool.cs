using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class CubePool : MonoBehaviour
{
    private const float SpawnPositionY = 15f;
    private const float SpawnAreaHalfSize = 10f;
    
    [SerializeField] private Cube _cubePrefab;
    [SerializeField] private int _defaultCapacity = 20;
    [SerializeField] private int _maxPoolSize = 100;

    private ObjectPool<Cube> _cubePool;
    
    public ObjectPool<Cube> Pool => _cubePool;

    private void Awake()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        _cubePool = new ObjectPool<Cube>(
            createFunc: CreateCube,
            actionOnGet: OnTakeFromPool,
            actionOnRelease: OnReturnToPool,
            actionOnDestroy: OnDestroyCube,
            collectionCheck: true,
            defaultCapacity: _defaultCapacity,
            maxSize: _maxPoolSize
        );
        
        List<Cube> prewarmCubes = new List<Cube>();
        
        for (int i = 0; i < _defaultCapacity; i++)
        {
            prewarmCubes.Add(_cubePool.Get());
        }
        
        foreach (var cube in prewarmCubes)
        {
            _cubePool.Release(cube);
        }
    }

    private Cube CreateCube()
    {
        Cube newCube = Instantiate(_cubePrefab);
        newCube.Initialize(this);
        newCube.gameObject.SetActive(false);
        
        return newCube;
    }

    private void OnTakeFromPool(Cube cube)
    {
        Vector3 randomPosition = new Vector3(Random.Range(-SpawnAreaHalfSize, SpawnAreaHalfSize), 
            SpawnPositionY, Random.Range(-SpawnAreaHalfSize, SpawnAreaHalfSize));
        cube.transform.position = randomPosition;
        cube.gameObject.SetActive(true);
    }

    private void OnReturnToPool(Cube cube)
    {
        cube.ResetCube();
        cube.gameObject.SetActive(false);
    }

    private void OnDestroyCube(Cube cube)
    {
        Destroy(cube.gameObject);
    }

    public Cube GetCube()
    {
        return _cubePool.Get();
    }

    public void ReturnCubeToPool(Cube cube)
    {
        _cubePool.Release(cube);
    }

    private void OnDestroy()
    {
        if (_cubePool != null)
        {
            _cubePool.Clear();
        }
    }
}