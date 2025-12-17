using System;

public interface ISpawner
{
    event Action<int, int, int> StatsChanged;
    
    int TotalSpawned { get; }
    int TotalCreated { get; }
    int ActiveCount { get; }
}