using UnityEngine;
using TMPro;

public class UI<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] protected TMP_Text Text;
    [SerializeField] protected string ObjectName;

    protected Spawner<T> Spawner;
    
    public void Initialize(string name, Spawner<T> spawner)
    {
        ObjectName = name;
        Spawner = spawner;
        UpdateText();
    }
    public void UpdateText()
    {
        if (Spawner != null)
        {
            Text.text = $"{ObjectName}\n" +
                       $"Заспавнено: {Spawner.TotalSpawned} шт.\n" +
                       $"Создано: {Spawner.TotalCreated} шт.\n" +
                       $"Активно: {Spawner.ActiveCount} шт.";
        }
    }
}