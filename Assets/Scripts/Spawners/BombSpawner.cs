using UnityEngine;

public class BombSpawner : Spawner<Bomb>
{
    public void SpawnBombAtPosition(Vector3 position)
    {
        Bomb bomb = GetFromPool();
        bomb.transform.position = position;
        bomb.BombExploded += HandleBombExploded;
    }
    
    private void HandleBombExploded(Bomb bomb)
    {
        bomb.BombExploded -= HandleBombExploded;
        ReturnToPool(bomb);
    }
}