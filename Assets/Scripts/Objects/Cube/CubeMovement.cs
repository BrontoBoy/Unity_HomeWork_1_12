using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CubeMovement : MonoBehaviour
{
    private Rigidbody _rigidbody;
    
    public Rigidbody Rigidbody => _rigidbody;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    
    public void ResetMovement()
    {
        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }
}