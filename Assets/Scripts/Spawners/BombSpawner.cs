using UnityEngine;

public class BombSpawner : Spawner<BombPresenter>
{
    [SerializeField] private float _explosionRadius = 5f;
    [SerializeField] private float _explosionForce = 500f;
    
    public void SpawnBombAtPosition(Vector3 position)
    {
        BombPresenter bomb = GetFromPool();
        bomb.transform.position = position;
    }
    
    protected override void InitializePresenter(BombPresenter presenter)
    {
        BombModel model = new BombModel(_minLifetime, _maxLifetime, _explosionRadius,
            _explosionForce);
        presenter.Initialize(model);
    }
    
    protected override void SubscribeToPresenterEvents(BombPresenter presenter)
    {
        presenter.BombExploded += HandleBombExploded;
    }
    
    private void HandleBombExploded(BombPresenter presenter)
    {
        presenter.BombExploded -= HandleBombExploded;
        ReturnToPool(presenter);
    }
}