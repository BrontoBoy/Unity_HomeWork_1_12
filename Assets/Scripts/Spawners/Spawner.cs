using System;
using UnityEngine;
using UnityEngine.Pool;

public abstract class Spawner<T> : MonoBehaviour, ISpawner where T : MonoBehaviour
{
    private const int DefaultPoolCapacity = 10;
    private const int DefaultMaxPoolSize = 30;
    private const int DefaultActiveCount = 0;
    
    [SerializeField] protected T Prefab;
    [SerializeField] protected int DefaultCapacity = DefaultPoolCapacity;
    [SerializeField] protected int MaxPoolSize = DefaultMaxPoolSize;
    
    protected ObjectPool<T> ObjectPool;
    
    private int _totalSpawned = 0;
    private int _totalCreated = 0; 
    
    public event Action<int> SpawnedCountChanged;
    public event Action<int> CreatedCountChanged; 
    public event Action<int> ActiveCountChanged;
    
    public int TotalSpawned 
    { 
        get => _totalSpawned;
        private set
        {
            _totalSpawned = value;
            
            SpawnedCountChanged?.Invoke(_totalSpawned);
        }
    }
    
    public int TotalCreated 
    { 
        get => _totalCreated;
        private set
        {
            _totalCreated = value;
            
            CreatedCountChanged?.Invoke(_totalCreated);
        }
    }
    
    public int ActiveCount => ObjectPool?.CountActive ?? DefaultActiveCount;
    
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
        ObjectPool.Release(item);
        ActiveCountChanged?.Invoke(ActiveCount);
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
        
        return newItem;
    }
    
    protected virtual void OnTakeFromPool(T item)
    {
        TotalSpawned++;
        item.gameObject.SetActive(true);
        ActiveCountChanged?.Invoke(ActiveCount);
    }
    
    protected virtual void OnReturnToPool(T item)
    {
        item.gameObject.SetActive(false);
        
        if (item is IResettable resettable)
        {
            resettable.Reset();
        }
    }
    
    protected virtual void OnDestroyPoolObject(T item)
    {
        Destroy(item.gameObject);
    }
    
    protected virtual T GetFromPool()
    {
        return ObjectPool.Get();
    }
}