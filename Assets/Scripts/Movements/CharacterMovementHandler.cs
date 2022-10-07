using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class CharacterMovementHandler : NetworkBehaviour
{
    Vector2 viewInput;
    [Header("player Movement")]
    public float playerSpeed = 1.9f;

    [Header("Player Animator and Gravity")]
    public float gravity = -9.81f;
    public Animator animator;

    [Header("Player Script Cameras")]
    public Transform playerCamera;

    [Header("Player Jumping and velocity")]
    Vector3 velocity;
    public float turnClamTime = 0.1f;
    float turnCalmVelocity;
    public Transform surfaceCheck;
    bool onSurface;
    public float surfaceDistance = 0.4f;
    public LayerMask surfaceMask;
 
    //Other components
    NetworkCharacterControllerPrototypeCustom networkCharacterControllerPrototypeCustom;

    private void Awake()
    {
        networkCharacterControllerPrototypeCustom = GetComponent<NetworkCharacterControllerPrototypeCustom>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    public override void FixedUpdateNetwork()
    {
        //Get the input from the network
        if (GetInput(out NetworkInputData networkInputData) &&  Runner.IsForward)
        {
            Vector3 direction = new Vector3(networkInputData.movementInput.x, 0f, networkInputData.movementInput.y).normalized;
            if (direction.magnitude >= 0.1f)
            {

                animator.SetBool("Walk", true);
                animator.SetBool("Idle", false);

                //rotation
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnCalmVelocity, turnClamTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

                networkCharacterControllerPrototypeCustom.Move(moveDirection.normalized * playerSpeed * Time.deltaTime);

            }
            else
            {
                animator.SetBool("Idle", true);
                animator.SetBool("Walk", false);
            }
        }
    }

    public void SetViewInputVector(Vector2 viewInput)
    {
        this.viewInput = viewInput;
    }
}
