using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private Camera mainCamera;
    private Vector3 direction;
    private Vector3 direction2;
    public Animator animator;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private bool jumpPressed;
    [SerializeField] private Transform upperTorso;
    [SerializeField] private Transform lowerTorso;
    [SerializeField] private float upperTorsoRotationSpeed;
    [SerializeField] private float lowerTorsoRotationSpeed;
    [SerializeField] private LayerMask ignoreMask;

    [SerializeField] private bool isGrounded;
    private float nextTimeToJump = 0f;
    public float jumpRate = 15f;
    public float jumpPackFuel;
    public bool jumpSwitch;
    public InputAction.CallbackContext jumpContext;

    public float gravity = -9.81f;
    public float gravityScale = 1;
    public Transform groundCheck;
    public LayerMask groundLayer;
    private float groundCheckRadius = 0.1f;

    private bool isMoving;
    private InputAction.CallbackContext callbackContext;

    private void Awake()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        isGrounded = GroundCheck();
    }

    private void FixedUpdate()
    {
        //xy movement
        Movement();

        if (jumpSwitch)
        {
            //power jumping
            PowerJump(jumpPressed);
        }
        else
        {
            //jumppack
            JumpPack(jumpPressed);
        }

        //rotate upper torso
        RotateTorwardsMouse();

        //animations and lower torso rotation
        if (isMoving)
        {
            RotateTorwardsMovementDirection();
            animator.SetBool("IsMoving", true);
            animator.SetFloat("MoveSpeed", direction.magnitude);
        }
        else
        {
            animator.SetBool("IsMoving", false);
            animator.SetFloat("MoveSpeed", 0f);
        }
    }

    void Movement()
    {
        // apply gravity
        if (!isGrounded)
        {
            if (rb.velocity.y > -15f)
            {
                direction.y += (gravity * gravityScale) * Time.fixedDeltaTime;
            }
        }
        else
        {
            direction.y = 0;
        }

        var cam = mainCamera.transform.TransformVector(transform.forward).normalized;
        var newforward = new Vector3(cam.x, 0, cam.z);

        var forward = callbackContext.ReadValue<Vector2>().y;
        var sideward = callbackContext.ReadValue<Vector2>().x;

        direction = newforward * forward + mainCamera.transform.TransformVector(transform.right).normalized * sideward + new Vector3(0, direction.y, 0);
        direction.Normalize();

        // movement for non-kinematic
        //rb.MovePosition(rb.position + movementSpeed * Time.fixedDeltaTime * direction);

        rb.Move(rb.position + movementSpeed * Time.fixedDeltaTime * direction, Quaternion.identity);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        callbackContext = context;

        //check if input is cancelled and if it is pressed get the direction and start animations
        if (context.canceled)
        {
            isMoving = false;

            direction.x = 0;
            direction.z = 0;
        }
        else
        {
            isMoving = true;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.canceled)
        {
            jumpPressed = true;
        }
        else
        {
            jumpPressed = false;
        }
    }

    public void PowerJump(bool jumpPressed)
    {
        if(jumpPressed)
        {
            if (Time.time >= nextTimeToJump && isGrounded)
            {
                BattleManager.Instance.playerComponent.rb.velocity = new Vector3(BattleManager.Instance.playerComponent.rb.velocity.x, 0, BattleManager.Instance.playerComponent.rb.velocity.z);
                nextTimeToJump = Time.time + 1f / jumpRate;
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }

    public void JumpPack(bool jumpPressed)
    {
        if(jumpPressed && jumpPackFuel > 0)
        {
            BurnJetPackFuel();
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    IEnumerator BurnJetPackFuel()
    {
        for(float i = jumpPackFuel; i >= 1; i--)
        {
            jumpPackFuel -= 1f;
            yield return new WaitForSeconds(0.01f);

            if (jumpContext.canceled)
            {
                break;
            }
        }
    }

    void RotateTorwardsMouse()
    {
        //rotate a rigidbody torwards a mouse position using raycast but without rotating on y axis
        Vector3 mouse = Input.mousePosition;
        Ray castPoint = Camera.main.ScreenPointToRay(mouse);
        RaycastHit hit;

        if (Physics.Raycast(castPoint, out hit, 5000, ~ignoreMask))
        {
            Quaternion toRotation = Quaternion.LookRotation(hit.point - upperTorso.position, transform.up);
            upperTorso.rotation = Quaternion.RotateTowards(upperTorso.rotation, toRotation, upperTorsoRotationSpeed * Time.deltaTime);
        }
    }

    void RotateTorwardsMovementDirection()
    {
        var cam = mainCamera.transform.TransformVector(transform.forward).normalized;
        var newforward = new Vector3(cam.x, 0, cam.z);
        var forward = callbackContext.ReadValue<Vector2>().y;
        var sideward = callbackContext.ReadValue<Vector2>().x;
        direction2 = newforward * forward + mainCamera.transform.TransformVector(transform.right).normalized * sideward;
        direction2.Normalize();

        Quaternion toRotation = Quaternion.LookRotation(direction2, lowerTorso.up);
        lowerTorso.rotation = Quaternion.RotateTowards(lowerTorso.rotation, toRotation, lowerTorsoRotationSpeed * Time.deltaTime);
    }

    private bool GroundCheck()
    {
        // Perform a sphere cast and check if it hits the ground layer
        return Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
    }
}