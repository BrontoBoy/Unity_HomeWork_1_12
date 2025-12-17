using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Timer : MonoBehaviour
{
    [SerializeField] private float _minTime = 2f;
    [SerializeField] private float _maxTime = 5f;
    
    private Coroutine _coroutine;
    private float _lifetime;
    
    public event Action TimeOver;
    
    public void StartTimer()
    {
        StopTimer();
        
        _lifetime = Random.Range(_minTime, _maxTime);
        _coroutine = StartCoroutine(TimerRoutine());
    }
    
    public void StopTimer()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
    }
    
    public void ResetTimer()
    {
        StopTimer();
    }
    
    private IEnumerator TimerRoutine()
    {
        yield return new WaitForSeconds(_lifetime);
        TimeOver?.Invoke();  
    }
}