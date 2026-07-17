using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float sp;
    [SerializeField] private float jp;
    [SerializeField] private float accel;
    [SerializeField] private float rotatesp;
    [SerializeField] float groundnormaly = 0.7f;
    [SerializeField] float groundDanping = 8.0f;
    [SerializeField] float airDanping = 0.7f;

    [SerializeField] Animator animator;

    bool isGround = false;
    Rigidbody rb;
    PlayerInput playerinput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerinput = GetComponent<PlayerInput>();
        rb.sleepThreshold = -1;
    }

    private void Update()
    {
        if (playerinput.actions["Jump"].WasPressedThisFrame()&&isGround)
        {
            rb.AddForce(new Vector3(0, jp,0) , ForceMode.VelocityChange);
        }
 
    }
    void FixedUpdate()
    {
        if (isGround)
        {
            rb.linearDamping = groundDanping;
        }
        else
        {
            rb.linearDamping = airDanping;
        }
        isGround = false;
        Vector2 accelvec = playerinput.actions["Move"].ReadValue<Vector2>();

        var cameraforward = playerinput.camera.transform.forward;
        cameraforward.y = 0;
        cameraforward = cameraforward.normalized;
        var cameraright = playerinput.camera.transform.right;

        Vector3 direction = accelvec.x * cameraright*accel + accelvec.y * cameraforward*accel;

        rb.AddForce (direction, ForceMode.Acceleration);

        Vector3 forward = transform.forward;

        transform.up = Vector3.up;
        transform.forward = Vector3.Slerp(forward, direction, rotatesp*Time.fixedDeltaTime);

        Vector3 velocityXZ = rb.linearVelocity;
        velocityXZ.y = 0;
        animator.SetFloat("MoveSpeed",velocityXZ.magnitude);
    }

    private void OnCollisionStay(Collision collision)
    {
        foreach(var contact in collision.contacts)
        {
           if(contact.normal.y>=groundnormaly)
            {
                isGround = true;
            }
        }
    }
}
