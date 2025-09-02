using UnityEngine;

public class Coordinator : MonoBehaviour
{
    [SerializeField] private CubePool _cubePool;
    [SerializeField] private CubeSpawner _cubeSpawner;

    private void Awake()
    {
        InitializeDependencies();
    }

    private void InitializeDependencies()
    {
        if (_cubePool == null)
        {
            _cubePool = FindFirstObjectByType<CubePool>();
        }

        if (_cubeSpawner == null)
        {
            _cubeSpawner = FindFirstObjectByType<CubeSpawner>();
        }

        if (_cubePool != null && _cubeSpawner != null)
        {
            _cubeSpawner.Initialize(_cubePool);
        }
        else
        {
            Debug.LogError("Не удалось найти все необходимые компоненты!");
        }
    }
}