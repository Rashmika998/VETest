using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [Header("player Movement")]
    public float playerSpeed=1.9f;
    public string TextureURL = "https://hips.hearstapps.com/hmg-prod.s3.amazonaws.com/images/dog-puppy-on-garden-royalty-free-image-1586966191.jpg";
    [Header("Player Animator and Gravity")]
    public CharacterController cC;
    public float gravity=-9.81f;
    public Animator animator;

    [Header("Player Script Cameras")]
    public Transform playerCamera;

    [Header("Player Jumping and velocity")]
    Vector3 velocity;
    public float turnClamTime=0.1f;
    float turnCalmVelocity;
    public Transform surfaceCheck;
    bool onSurface;
    public float surfaceDistance=0.4f;
    public LayerMask surfaceMask;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()

    {
        onSurface=Physics.CheckSphere(surfaceCheck.position,surfaceDistance,surfaceMask);
        if(onSurface && velocity.y<0){
            velocity.y=-2f;
        }

        velocity.y+=gravity*Time.deltaTime;
        cC.Move(velocity*Time.deltaTime);
        playerMove();
    }

    void playerMove(){
        float horizontal_axis=Input.GetAxisRaw("Horizontal");  
        float vertical_axis=Input.GetAxisRaw("Vertical");  

        Vector3 direction=new Vector3(horizontal_axis,0f,vertical_axis).normalized;

        if(direction.magnitude>=0.1f){

            animator.SetBool("Walk",true);
            animator.SetBool("Idle",false);

            float targetAngle=Mathf.Atan2(direction.x,direction.z) * Mathf.Rad2Deg+playerCamera.eulerAngles.y;
            float angle=Mathf.SmoothDampAngle(transform.eulerAngles.y,targetAngle,ref turnCalmVelocity,turnClamTime);
            transform.rotation=Quaternion.Euler(0f,angle,0f);

            Vector3 moveDirection=Quaternion.Euler(0f,targetAngle,0f)*Vector3.forward;

            cC.Move(moveDirection.normalized*playerSpeed*Time.deltaTime);

        }else{
            
            animator.SetBool("Idle",true);
            animator.SetBool("Walk",false);


        }
    }

        
          
}
