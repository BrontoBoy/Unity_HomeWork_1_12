using UnityEngine;
using TMPro;

public class UI : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private string _name;
    
    private ISpawner _spawner;
    
    private void OnDestroy()
    {
        if (_spawner != null)
        {
            _spawner.SpawnedCountChanged -= OnCountChanged;
            _spawner.CreatedCountChanged -= OnCountChanged;
            _spawner.ActiveCountChanged -= OnCountChanged;
        }
    }
    
    public void Initialize(string name, ISpawner spawner)
    {
        _name = name;
        _spawner = spawner;
        
        if (_spawner != null)
        {
            _spawner.SpawnedCountChanged += OnCountChanged;
            _spawner.CreatedCountChanged += OnCountChanged;
            _spawner.ActiveCountChanged += OnCountChanged;
        }
        
        UpdateText();
    }
    
    private void OnCountChanged(int value)
    {
        UpdateText();
    }
    
    private void UpdateText()
    {
        if (_spawner != null && _text != null)
        {
            _text.text = $"{_name}\n" +
                         $"Заспавнено: {_spawner.TotalSpawned} шт.\n" +
                         $"Создано: {_spawner.TotalCreated} шт.\n" +
                         $"Активно: {_spawner.ActiveCount} шт.";
        }
    }
}