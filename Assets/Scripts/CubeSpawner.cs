using System.Collections;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private float _spawnInterval = 0.5f;
    [SerializeField] private bool _isSpawning = true;

    private CubePool _cubePool;
    private WaitForSeconds _spawnWait;
    
    private void Start()
    {
        _cubePool = FindObjectOfType<CubePool>();
        
        if (_cubePool == null)
        {
            Debug.LogError("CubePool не найден на сцене!");
            return;
        }
        
        _spawnWait = new WaitForSeconds(_spawnInterval);
        StartCoroutine(SpawnCubesRoutine());
    }
    
    private IEnumerator SpawnCubesRoutine()
    {
        while (_isSpawning == true)
        {
            _cubePool.GetCube();
            yield return _spawnWait;
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
