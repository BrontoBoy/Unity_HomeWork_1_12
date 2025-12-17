using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

public class UI : MonoBehaviour
{
    [SerializeField] private TMP_Text Text;
    [SerializeField] private string Name;
    
    private ISpawner _spawner;
    
    private void OnDestroy()
    {
        if (_spawner != null)
            _spawner.StatsChanged -= UpdateText;
    }
    
    public void Initialize(string name, ISpawner spawner)
    {
        Name = name;
        _spawner = spawner;
        
        _spawner.StatsChanged += UpdateText;
        
        UpdateText(_spawner.TotalSpawned, _spawner.TotalCreated, _spawner.ActiveCount);
    }
    
    private void UpdateText(int spawned, int created, int active)
    {
        if (Text != null)
        {
            Text.text = $"{Name}\n" +
                        $"Заспавнено: {spawned} шт.\n" +
                        $"Создано: {created} шт.\n" +
                        $"Активно: {active} шт.";
        }
    }
}