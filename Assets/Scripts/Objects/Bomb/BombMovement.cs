using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BombMovement : MonoBehaviour
{
    private const float NeverSleepThreshold = 0f;
    
    private Rigidbody _rigidbody;
    
    public Rigidbody Rigidbody => _rigidbody;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.sleepThreshold = NeverSleepThreshold;
        _rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }
    
    public void ResetMovement()
    {
        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        _rigidbody.WakeUp();
    }
}