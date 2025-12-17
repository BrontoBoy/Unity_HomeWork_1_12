using System;
using UnityEngine;
using UnityEngine.Pool;

public abstract class Spawner<T> : MonoBehaviour, ISpawner where T : MonoBehaviour, IResettable
{
    private const int DefaultPoolCapacity = 20;
    private const int DefaultMaxPoolSize = 50;
    
    [SerializeField] protected T Prefab;
    [SerializeField] protected int DefaultCapacity = DefaultPoolCapacity;
    [SerializeField] protected int MaxPoolSize = DefaultMaxPoolSize;
    [SerializeField] protected float _minLifetime = 2f;
    [SerializeField] protected float _maxLifetime = 5f;
    
    protected ObjectPool<T> ObjectPool;
    
    public event Action<int, int, int> StatsChanged;
    
    public int TotalSpawned { get; private set; }
    public int TotalCreated { get; private set; }
    public int ActiveCount => ObjectPool?.CountActive ?? 0;
    
    protected virtual void OnDestroy()
    {
        ObjectPool?.Clear();
    }
    
    public virtual void Initialize()
    {
        InitializePool();
    }
    
    public virtual void ReturnToPool(T item)
    {
        if (item == null) 
            return;
            
        if (ObjectPool == null) 
            return;
        
        ObjectPool.Release(item);
        NotifyStatsChanged();
    }
    
    protected virtual void InitializePool()
    {
        ObjectPool = new ObjectPool<T>(
            createFunc: CreatePooledItem,
            actionOnGet: OnTakeFromPool,
            actionOnRelease: OnReturnToPool,
            actionOnDestroy: OnDestroyPoolObject,
            collectionCheck: true,
            defaultCapacity: DefaultCapacity,
            maxSize: MaxPoolSize
        );
    }
    
    protected virtual T CreatePooledItem()
    {
        TotalCreated++;
        T newItem = Instantiate(Prefab);
        newItem.gameObject.SetActive(false);
        InitializePresenter(newItem);
        NotifyStatsChanged();
        
        return newItem;
    }
    
    protected abstract void InitializePresenter(T presenter);
    
    protected virtual void OnTakeFromPool(T item)
    {
        TotalSpawned++;
        item.gameObject.SetActive(true);
        SubscribeToPresenterEvents(item);
        NotifyStatsChanged();
    }
    
    protected abstract void SubscribeToPresenterEvents(T presenter);
    
    protected virtual void OnReturnToPool(T item)
    {
        if (item == null) 
            return;
        
        item.gameObject.SetActive(false);
        
        if (item is IResettable resettable)
            resettable.Reset();
    }
    
    protected virtual void OnDestroyPoolObject(T item)
    {
        Destroy(item.gameObject);
    }
    
    protected virtual T GetFromPool()
    {
        return ObjectPool.Get();
    }
    
    private void NotifyStatsChanged()
    {
        StatsChanged?.Invoke(TotalSpawned, TotalCreated, ActiveCount);
    }
}