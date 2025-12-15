using UnityEngine;
using UnityEngine.Pool;
using System.Collections;
using System.Collections.Generic;

public abstract class Spawner<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] protected T Prefab;
    [SerializeField] protected float SpawnInterval = 0.5f;
    [SerializeField] protected int DefaultCapacity = 20;
    [SerializeField] protected int MaxPoolSize = 100;
    [SerializeField] protected float SpawnPositionY = 15f;
    [SerializeField] protected float SpawnAreaHalfSize = 10f;
    
    protected ObjectPool<T> ObjectPool;
    protected List<T> ActiveObjects = new List<T>();
    protected WaitForSeconds SpawnWait;
    protected Coroutine SpawnCoroutine;
    protected bool IsSpawning = false;
    
    public int TotalSpawned { get; private set; } = 0;
    public int TotalCreated { get; private set; } = 0;
    public int ActiveCount => ActiveObjects.Count;
    
    protected virtual void OnDestroy()
    {
        StopSpawning();
        
        foreach (T obj in ActiveObjects.ToArray())
        {
            ReturnToPool(obj);
        }

        ObjectPool?.Clear();
    }
    
    public void Initialize()
    {
        InitializePool();
        StartSpawning();
    }
    
    public virtual void StartSpawning()
    {
        if (IsSpawning == false && SpawnCoroutine == null)
        {
            IsSpawning = true;
            SpawnWait = new WaitForSeconds(SpawnInterval);
            SpawnCoroutine = StartCoroutine(SpawnRoutine());
        }
    }
    
    public virtual void StopSpawning()
    {
        IsSpawning = false;

        if (SpawnCoroutine != null)
        {
            StopCoroutine(SpawnCoroutine);
            SpawnCoroutine = null;
        }
    }
    
    public virtual void ReturnToPool(T item)
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

        PrewarmPool();
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
        ActiveObjects.Add(item);
        item.gameObject.SetActive(true);
    }
    
    protected virtual void OnReturnToPool(T item)
    {
        ActiveObjects.Remove(item);
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
    
    protected virtual void PrewarmPool()
    {
        List<T> prewarmItems = new List<T>();

        for (int i = 0; i < DefaultCapacity; i++)
        {
            prewarmItems.Add(ObjectPool.Get());
        }

        foreach (T item in prewarmItems)
        {
            ObjectPool.Release(item);
        }
    }
    
    protected virtual IEnumerator SpawnRoutine()
    {
        while (IsSpawning)
        {
            SpawnItem();
            
            yield return SpawnWait;
        }
    }
    
    protected virtual void SpawnItem()
    {
        T item = ObjectPool.Get();
        item.transform.position = GetRandomSpawnPosition();
    }
    
    protected virtual Vector3 GetRandomSpawnPosition()
    {
        return new Vector3(Random.Range(-SpawnAreaHalfSize, SpawnAreaHalfSize),
            SpawnPositionY, Random.Range(-SpawnAreaHalfSize, SpawnAreaHalfSize));
    }
}