public interface ISpawner
{
    event System.Action<int> SpawnedCountChanged;
    event System.Action<int> CreatedCountChanged;
    event System.Action<int> ActiveCountChanged;
    
    int TotalSpawned { get; }
    int TotalCreated { get; }
    int ActiveCount { get; }
}