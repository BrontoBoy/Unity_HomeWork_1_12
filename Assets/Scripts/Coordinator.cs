using UnityEngine;

public class Coordinator : MonoBehaviour
{
    [SerializeField] private CubeSpawner CubeSpawner;
    [SerializeField] private BombSpawner BombSpawner;
    [SerializeField] private UI CubeUI;
    [SerializeField] private UI BombUI;
    [SerializeField] private float UIUpdateInterval = 0.5f;

    private float timer;

    private void Awake()
    {
        Initialize();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= UIUpdateInterval)
        {
            UpdateUI();
            timer = 0f;
        }
    }

    private void Initialize()
    {
        CubeSpawner.Initialize();
        BombSpawner.Initialize();
        CubeUI.Initialize("Кубы", CubeSpawner, null);
        BombUI.Initialize("Бомбы", null, BombSpawner);
        UpdateUI();
    }

    private void UpdateUI()
    {
        CubeUI.UpdateText();
        BombUI.UpdateText();
    }
}