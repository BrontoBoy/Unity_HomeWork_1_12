using UnityEngine;

public class BombSpawner : Spawner<Bomb>
{
    public override void StartSpawning()
    {
        InitializePool();
    }
    
    public void Initialize()
    {
        InitializePool();
    }

    public void SpawnBombAtPosition(Vector3 position)
    {
        Bomb bomb = ObjectPool.Get();
        bomb.transform.position = position;
        bomb.SetBombSpawner(this);
    }
    
    public void ReturnBombToPool(Bomb bomb)
    {
        ReturnToPool(bomb);
    }
}