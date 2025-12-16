using UnityEngine;
using UnityEngine.Pool;

public abstract class Spawner<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] protected T Prefab;
    [SerializeField] protected int DefaultCapacity = 20;
    [SerializeField] protected int MaxPoolSize = 100;

    protected ObjectPool<T> ObjectPool;
    
    public int TotalSpawned { get; private set; } = 0;
    public int TotalCreated { get; private set; } = 0;
    public int ActiveCount => ObjectPool?.CountActive ?? 0;
    
    protected virtual void OnDestroy()
    {
        ObjectPool?.Clear();
    }
    
    public virtual void Initialize()
    {
        InitializePool();
    }

    protected virtual void ReturnToPool(T item)
    {
        ObjectPool.Release(item);
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