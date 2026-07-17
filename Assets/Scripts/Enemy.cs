using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 3f;
    public Collider _playerCollider { get; set;}
    Rigidbody _rb;
    [SerializeField] float _rotateSpeed = 3f;
    [SerializeField] int hp = 0;
    [SerializeField] float invincibleTime = 0.5f;
    [SerializeField] float knockbacksp = 5f;
    float invincibleTimeMax =1.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        invincibleTime = invincibleTimeMax;

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
        
        if(isScenePlayer&&invincibleTime<=0)
        {
            var subVec = _playerCollider.bounds.center - _rb.position;
            subVec.y = 0;
            _rb.linearVelocity = subVec.normalized * _moveSpeed;

            var rotateTarget = subVec.normalized;
            Vector3 forward = transform.forward;

            transform.forward = Vector3.Slerp(forward, rotateTarget, _rotateSpeed * Time.deltaTime);
        }
        

        if(invincibleTime>0)
        {
            invincibleTime -= Time.deltaTime;
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        var attackObj = collision.gameObject.GetComponent<AttackObject>();
        if(attackObj != null && invincibleTime<0)
        {
            hp -= attackObj.power;
            invincibleTime = invincibleTimeMax;
            if(hp<=0)
            {
                Destroy(gameObject);
            }

            var dir = transform.position - collision.transform.position;
            dir.y = 0;
            var knocbackVec = dir.normalized * knockbacksp;
            _rb.linearVelocity = knocbackVec;
        }
    }
}
