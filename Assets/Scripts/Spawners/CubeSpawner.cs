using UnityEngine;
using System.Collections;

public class CubeSpawner : Spawner<Cube>
{
    [SerializeField] private float _spawnInterval = 0.5f;
    [SerializeField] private float _spawnPositionY = 15f;
    [SerializeField] private float _spawnAreaHalfSize = 10f;
    [SerializeField] private BombSpawner _bombSpawner;

    private WaitForSeconds _spawnWait;
    private Coroutine _spawnCoroutine;
    private bool _isSpawning = false; 
    
    public override void Initialize()
    {
        base.Initialize();
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
        Cube cube = GetFromPool();
        cube.transform.position = GetRandomSpawnPosition();
        cube.CubeExpired += HandleCubeExpired;
    }
    
    private void HandleCubeExpired(Cube cube, Vector3 position)
    {
        cube.CubeExpired -= HandleCubeExpired;
        
        if (_bombSpawner != null)
        {
            _bombSpawner.SpawnBombAtPosition(position);
        }
        
        ReturnToPool(cube);
    }
    
    private Vector3 GetRandomSpawnPosition()
    {
        return new Vector3(
            Random.Range(-_spawnAreaHalfSize, _spawnAreaHalfSize), _spawnPositionY,
            Random.Range(-_spawnAreaHalfSize, _spawnAreaHalfSize)
        );
    }
}