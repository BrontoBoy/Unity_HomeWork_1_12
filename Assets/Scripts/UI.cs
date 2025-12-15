using UnityEngine;
using TMPro;

public class UI : MonoBehaviour
{
    [SerializeField] private bool IsForCubes = true;
    
    private TMP_Text Text;
    private CubeSpawner CubeSpawner;
    private BombSpawner BombSpawner;
    private string ObjectName;

    public void Initialize(string name, CubeSpawner cubeSpawner = null, BombSpawner bombSpawner = null)
    {
        ObjectName = name;
        Text = GetComponent<TMP_Text>();
        CubeSpawner = cubeSpawner;
        BombSpawner = bombSpawner;
        UpdateText();
    }

    public void UpdateText()
    {
        int totalSpawned = 0;
        int totalCreated = 0;
        int activeCount = 0;

        if (CubeSpawner != null)
        {
            totalSpawned = CubeSpawner.TotalSpawned;
            totalCreated = CubeSpawner.TotalCreated;
            activeCount = CubeSpawner.ActiveCount;
        }
        else if (BombSpawner != null)
        {
            totalSpawned = BombSpawner.TotalSpawned;
            totalCreated = BombSpawner.TotalCreated;
            activeCount = BombSpawner.ActiveCount;
        }

        Text.text = $"{ObjectName}\n" +
                    $"Заспавнено: {totalSpawned} шт.\n" +
                    $"Создано: {totalCreated} шт.\n" +
                    $"Активно: {activeCount} шт.";
    }
}