using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField,Header("移動速度")] private float _speed;
    [SerializeField,Header("ジャンプ力")] private float _jp;
    [SerializeField,Header("回転速度")] private float _rotateSpeed;
    [SerializeField,Header("重力")] private float _gravity;
    [SerializeField,Header("炎のスピード")] private float _fireSpeed;
    [SerializeField,Header("炎のエフェクト")] GameObject _firePrefab;
    [SerializeField, Header("炎の生成位置")] Vector3 _offset;
    [SerializeField, Header("HP")] int _hp;

    [SerializeField] Animator _animator;
    CharacterController _characterController;
    PlayerInput _playerInput;

    Vector2 _playerVec;
    Vector3 _velocity;
    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _playerInput = GetComponent<PlayerInput>();
    }
    private void OnEnable()
    {
        _playerInput.actions["Move"].performed += OnMove;
        _playerInput.actions["Move"].canceled += OnMove;

        _playerInput.actions["Jump"].performed +=OnJump;

        _playerInput.actions["Attack"].performed += OnAttack;


    }
    private void OnDisable()
    {
        _playerInput.actions["Move"].performed -= OnMove;
        _playerInput.actions["Move"].canceled -= OnMove;

        _playerInput.actions["Jump"].performed -= OnJump;

        _playerInput.actions["Attack"].performed -= OnAttack;
    }

    private void Update()
    {
        Gravity();
        Move();
    }
    private void OnMove(InputAction.CallbackContext context)
    {
        _playerVec =context.ReadValue<Vector2>();
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if(_characterController.isGrounded)
        {
            _velocity.y = _jp;
        }
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        Vector3 position = transform.position + transform.TransformVector(_offset);
        var fireObj = Object.Instantiate(_firePrefab, position, Quaternion.identity);
        var fireRB = fireObj.GetComponent<Rigidbody>();

        if (fireRB != null)
        {
            fireRB.linearVelocity = transform.forward * _fireSpeed;
        }
    }

    private void Move()
    {
        Vector3 cameraforward = _playerInput.camera.transform.forward;
        cameraforward.y = 0;
        cameraforward = cameraforward.normalized;
        Vector3 cameraright = _playerInput.camera.transform.right;

        Vector3 direction = _playerVec.x * cameraright * _speed + _playerVec.y * cameraforward * _speed;
        _characterController.Move(direction * Time.deltaTime);

        Vector3 forward = transform.forward;

        transform.up = Vector3.up;
        transform.forward = Vector3.Slerp(forward, direction, _rotateSpeed * Time.fixedDeltaTime);

        Vector3 velocityXZ = _playerVec;
        velocityXZ.y = 0;
        _animator.SetFloat("MoveSpeed", _playerVec.magnitude);
    }

    private void Gravity()
    {
        if(_characterController.isGrounded && _velocity.y <0)
        {
            _velocity.y = -2f;
        }

        _velocity.y -= _gravity * Time.deltaTime;
        _characterController.Move(_velocity*Time.deltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        var attackObj = hit.gameObject.GetComponent<AttackObject>();

        if(attackObj != null)
        {
            _hp -= attackObj.power;
            if(_hp <= 0)
            {
                Destroy(gameObject);
            }
        }


    }
}
