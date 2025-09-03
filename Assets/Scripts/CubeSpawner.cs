using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class CubeSpawner : MonoBehaviour
{
    private readonly List<Cube> _activeCubes = new List<Cube>();
    
    [SerializeField] private float _spawnInterval = 0.5f;
    [SerializeField] private Cube _cubePrefab;
    [SerializeField] private int _defaultCapacity = 20;
    [SerializeField] private int _maxPoolSize = 100;
    [SerializeField] private float _spawnPositionY = 15f;
    [SerializeField] private float _spawnAreaHalfSize = 10f;
    
    private ObjectPool<Cube> _objectPool;
    private WaitForSeconds _spawnWait;
    private Coroutine _spawnCoroutine;
    private bool _isSpawning = false;

    private void OnDestroy()
    {
        StopSpawning();
        
        foreach (Cube cube in _activeCubes.ToArray())
        {
            ReturnCube(cube);
        }
        
        _objectPool?.Clear();
    }

    public void Initialize()
    {
        InitializePool();
        StartSpawning();
    }

    public void StartSpawning()
    {
        if (_isSpawning == false && _spawnCoroutine == null)
        {
            _isSpawning = true;
            _spawnWait = new WaitForSeconds(_spawnInterval);
            _spawnCoroutine = StartCoroutine(SpawnRoutine());
        }
    }

    public void StopSpawning()
    {
        _isSpawning = false;
        
        if (_spawnCoroutine != null)
        {
            StopCoroutine(_spawnCoroutine);
            _spawnCoroutine = null;
        }
    }
    
    public void ReturnCube(Cube cube)
    {
        _objectPool.Release(cube);
    }
    
    private void InitializePool()
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
        Cube newCube = Instantiate(_cubePrefab);
        newCube.Expired += HandleCubeExpired;
        newCube.gameObject.SetActive(false);
        
        return newCube;
    }

    private void OnTakeFromPool(Cube cube)
    {
        cube.gameObject.SetActive(true);
        _activeCubes.Add(cube);
    }

    private void OnReturnToPool(Cube cube)
    {
        cube.gameObject.SetActive(false);
        cube.Reset();
        _activeCubes.Remove(cube);
    }

    private void OnDestroyCube(Cube cube)
    {
        cube.Expired -= HandleCubeExpired;
        Destroy(cube.gameObject);
    }

    private void HandleCubeExpired(Cube cube)
    {
        ReturnCube(cube);
    }

    private void PrewarmPool()
    {
        List<Cube> prewarmCubes = new List<Cube>();
        
        for (int i = 0; i < _defaultCapacity; i++)
        {
            prewarmCubes.Add(_objectPool.Get());
        }
        
        foreach (Cube cube in prewarmCubes)
        {
            _objectPool.Release(cube);
        }
    }

    private IEnumerator SpawnRoutine()
    {
        while (_isSpawning)
        {
            SpawnCube();
            
            yield return _spawnWait;
        }
    }

    private void SpawnCube()
    {
        Cube cube = _objectPool.Get();
        cube.transform.position = GetRandomSpawnPosition();
    }
    
    private Vector3 GetRandomSpawnPosition()
    {
        return new Vector3(
            Random.Range(-_spawnAreaHalfSize, _spawnAreaHalfSize),
            _spawnPositionY,
            Random.Range(-_spawnAreaHalfSize, _spawnAreaHalfSize)
        );
    }
}