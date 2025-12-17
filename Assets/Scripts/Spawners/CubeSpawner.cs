using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class CubeSpawner : Spawner<Cube>
{
    [SerializeField] private float _spawnInterval = 0.5f;
    [SerializeField] private float _spawnPositionY = 15f;
    [SerializeField] private float _spawnAreaHalfSize = 10f;
    [SerializeField] private BombSpawner _bombSpawner;
    
    private Coroutine _spawnCoroutine;
    
    private void OnDisable()
    {
        StopSpawning();
    }
    
    public override void Initialize()
    {
        base.Initialize();
        StartSpawning();
    }
    
    public void StartSpawning()
    {
        if (_spawnCoroutine == null)
        {
            _spawnCoroutine = StartCoroutine(SpawnRoutine());
        }
    }
    
    public void StopSpawning()
    {
        if (_spawnCoroutine != null)
        {
            StopCoroutine(_spawnCoroutine);
            _spawnCoroutine = null;
        }
    }
    
    private IEnumerator SpawnRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(_spawnInterval);
        
        while (true)
        {
            SpawnCube();
            yield return wait;
        }
    }
    
    private void SpawnCube()
    {
        Cube cube = GetFromPool();
        cube.transform.position = GetRandomSpawnPosition();
        cube.CubeExpired += HandleCubeExpired;
    }
    
    private void HandleCubeExpired(Cube cube, Vector3 position)
    {
        cube.CubeExpired -= HandleCubeExpired;
        
        if (_bombSpawner != null)
        {
            _bombSpawner.SpawnAtPosition(position);
        }
        
        ReturnToPool(cube);
    }
    
    private Vector3 GetRandomSpawnPosition()
    {
        return new Vector3(
            Random.Range(-_spawnAreaHalfSize, _spawnAreaHalfSize), _spawnPositionY,
            Random.Range(-_spawnAreaHalfSize, _spawnAreaHalfSize));
    }
}