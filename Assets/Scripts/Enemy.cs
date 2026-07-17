using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 3f;
    public Collider _playerCollider { get; set;}
    Rigidbody _rb;
    [SerializeField] float _rotateSpeed = 3f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        var direction = _playerCollider.bounds.center - _rb.position;
        bool isScenePlayer = true;

        if(Physics.Raycast(_rb.position,direction.normalized,out var hit))
        {
            if(hit.collider != null)
            {
                isScenePlayer = false;
            }
        }
        var subVec = _playerCollider.bounds.center - _rb.position;
        subVec.y = 0;
        _rb.linearVelocity = subVec.normalized * _moveSpeed;

        var rotateTarget = subVec.normalized;
        Vector3 forward = transform.forward;
        transform.forward = Vector3.Slerp(forward, rotateTarget,_rotateSpeed * Time.deltaTime);
    }
}
