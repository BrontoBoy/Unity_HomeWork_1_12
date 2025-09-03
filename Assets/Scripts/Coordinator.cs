using UnityEngine;

public class Coordinator : MonoBehaviour
{
    [SerializeField] private CubeSpawner _cubeSpawner;
    
    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        _cubeSpawner.Initialize();
    }
}