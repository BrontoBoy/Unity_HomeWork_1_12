using UnityEngine;

public class CubeSpawner : Spawner<Cube>
{
    [SerializeField] private BombSpawner BombSpawner;

    protected override Cube CreatePooledItem()
    {
        Cube newCube = base.CreatePooledItem();
        newCube.SetCubeSpawner(this);
        return newCube;
    }

    public void CreateBombAtPosition(Vector3 position)
    {
        if (BombSpawner != null)
        {
            BombSpawner.SpawnBombAtPosition(position);
        }
    }

    protected override void OnTakeFromPool(Cube item)
    {
        base.OnTakeFromPool(item);
        item.gameObject.SetActive(true);
    }
}