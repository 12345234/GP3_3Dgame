using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float sp;
    [SerializeField] private float jp;
    [SerializeField] private float accel;
    [SerializeField] private float rotatesp;

    [SerializeField] Animator animator;


    Rigidbody rb;
    PlayerInput playerinput;
    Vector3 accelvecrotate;

    [SerializeField] LayerMask mask;

    RaycastHit hit;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerinput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        if (playerinput.actions["Jump"].WasPressedThisFrame()&&MapCheck())
        {
            rb.AddForce(new Vector3(0, jp,0) , ForceMode.VelocityChange);
        }

        Debug.DrawRay(transform.position, Vector3.down,Color.red ,0.3f);

        
    }
    void FixedUpdate()
    {
        Vector2 accelvec = playerinput.actions["Move"].ReadValue<Vector2>();

        var cameraforward = playerinput.camera.transform.forward;
        cameraforward.y = 0;
        cameraforward = cameraforward.normalized;
        var cameraright = playerinput.camera.transform.right;

        Vector3 direction = accelvec.x * cameraright*accel + accelvec.y * cameraforward*accel;

        rb.AddForce (direction, ForceMode.Acceleration);

        if(direction!=Vector3.zero)
        {
            accelvecrotate = direction.normalized;
        }
        Vector3 forward = transform.forward;

        transform.up = Vector3.up;
        transform.forward = Vector3.Slerp(forward, direction, rotatesp*Time.fixedDeltaTime);

        Vector3 velocityXZ = rb.linearVelocity;
        velocityXZ.y = 0;
        animator.SetFloat("MoveSpeed",velocityXZ.magnitude);
    }

    bool MapCheck()
    {
        if(Physics.Raycast(transform.position,Vector3.down,out hit ,0.1f, mask))
        {
            return true;
        }

        return hit.collider != null;
    }
}
