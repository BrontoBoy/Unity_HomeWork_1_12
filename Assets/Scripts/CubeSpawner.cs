using System.Collections;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private float _spawnInterval = 0.5f;
    
    private CubePool _cubePool;
    private WaitForSeconds _spawnWait;
    private Coroutine _spawnCoroutine;
    private bool _isSpawning = false;
    private bool _isInitialized = false;

    private void OnDestroy()
    {
        StopSpawning();
    }

    public void Initialize(CubePool cubePool)
    {
        _cubePool = cubePool;
        _isInitialized = true;
        
        StartSpawning();
    }

    public void StartSpawning()
    {
        if (_isInitialized == true && _isSpawning == false && _spawnCoroutine == null)
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

    private IEnumerator SpawnRoutine()
    {
        int spawnCount = 0;
        
        while (_isSpawning)
        {
            spawnCount++;
            SpawnCube();
            yield return _spawnWait;
        }
        
        _spawnCoroutine = null;
    }

    private void SpawnCube()
    {
        if (_cubePool == null)
        {
            return;
        }
        
        Cube cube = _cubePool.GetCube();
    }
}