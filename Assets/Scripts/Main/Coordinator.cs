using UnityEngine;

public class Coordinator : MonoBehaviour
{
    [SerializeField] private CubeSpawner _cubeSpawner;
    [SerializeField] private BombSpawner _bombSpawner;
    [SerializeField] private UI<Cube> _cubeUI;
    [SerializeField] private UI<Bomb> _bombUI;
    [SerializeField] private float _uIUpdateInterval = 0.5f;

    private float _timer;

    private void Awake()
    {
        Initialize();
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        
        if (_timer >= _uIUpdateInterval)
        {
            UpdateUI();
            _timer = 0f;
        }
    }

    private void Initialize()
    {
        _cubeSpawner.Initialize();
        _bombSpawner.Initialize();
        _cubeUI.Initialize("Кубы", _cubeSpawner);
        _bombUI.Initialize("Бомбы", _bombSpawner);
        UpdateUI();
    }

    private void UpdateUI()
    {
        _cubeUI.UpdateText();
        _bombUI.UpdateText();
    }
}