using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private float _radius = 5f;
    [SerializeField] private float _force = 500f;
    [SerializeField] private float _upwardsModifier = 1f;
    
    private Rigidbody _ownerRigidbody;

    private void Awake()
    {
        _ownerRigidbody = GetComponent<Rigidbody>();
    }
    
    public void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _radius);

        foreach (Collider hit in colliders)
        {
            Rigidbody rigidbody = hit.attachedRigidbody;
            
            if (rigidbody == null || rigidbody == _ownerRigidbody) 
                continue;
            
            rigidbody.AddExplosionForce(_force, transform.position, _radius, 
                _upwardsModifier, ForceMode.Impulse);
        }
    }
    
    public void SetParameters(float radius, float force)
    {
        _radius = radius;
        _force = force;
    }
}