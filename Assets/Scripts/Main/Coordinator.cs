using UnityEngine;

public class Coordinator : MonoBehaviour
{
    [SerializeField] private CubeSpawner _cubeSpawner;
    [SerializeField] private BombSpawner _bombSpawner;
    [SerializeField] private UI _cubeUI;
    [SerializeField] private UI _bombUI;
    
    private void Awake()
    {
        Initialize();
    }
    
    private void Initialize()
    {
        _cubeSpawner.Initialize();
        _bombSpawner.Initialize();
        _cubeUI.Initialize("Кубы", _cubeSpawner);
        _bombUI.Initialize("Бомбы", _bombSpawner);
    }
}