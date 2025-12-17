using UnityEngine;

public class BombSpawner : Spawner<Bomb>
{
    public void SpawnAtPosition(Vector3 position)
    {
        Bomb bomb = GetFromPool();
        bomb.transform.position = position;
        bomb.Exploded += HandleExploded;
    }
    
    private void HandleExploded(Bomb bomb)
    {
        bomb.Exploded -= HandleExploded;
        ReturnToPool(bomb);
    }
}