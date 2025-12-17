using System;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    private bool _isTouched = false;
    
    public event Action Touched;
    
    public bool IsTouched => _isTouched;

    private void OnCollisionEnter(Collision collision)
    {
        if (_isTouched == false && collision.gameObject.TryGetComponent(out Platform _))
        {
            _isTouched = true;
            Touched?.Invoke();
        }
    }
    
    public void ResetState()
    {
        _isTouched = false;
    }
}