using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 direction;
    public float forwardSpeed;
    public float maxSpeed;

    private int desiredLane = 1;//0:left 1:middle 2:right
    public float laneDistance = 4;//the distance between 2 lanes
    public float Gravity = -20;
    public static bool gameHasStarted = false;

    public float jumpForce;
    public Animator animator;
    public bool playerIsGrounded;
    private bool isSliding = false;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();

        playerIsGrounded = true;
    }

    // Update is called once per frame
    void Update()
    {
        
        direction.z = forwardSpeed;

        //increase game speed
        if (gameHasStarted == true)
        {
            if (forwardSpeed < maxSpeed)
                forwardSpeed += 0.1f * Time.deltaTime;
        }

        // Jump Action
        if (controller.isGrounded) //So the player will only jump if it's grounded and won't be able to jump continuously
        {
            
            if (SwipeManager.swipeUp)
            {
                Jump();
            } 
        }
        else
        {
            direction.y += Gravity * Time.deltaTime; // not sure why not use a rigidbody since that gives gravity but whatever
        }

        //Gather the inputs on which lane we should be

        if (SwipeManager.swipeRight)
        {
            desiredLane++;
            if (desiredLane == 3)
                desiredLane = 2;
        }

        if (SwipeManager.swipeLeft) // can access these because they are public static variables from another script
        {
            desiredLane--;
            if (desiredLane == -1)
                desiredLane = 0;
        }

        //Sliding
        {
            if(SwipeManager.swipeDown && !isSliding)
            {
                StartCoroutine(Slide());
            }
        }

        //Calculate where we should be in the future

        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;
 
        if (desiredLane == 0)
        {
            targetPosition += Vector3.left * laneDistance;
        }
        else if (desiredLane == 2)
        {
            targetPosition += Vector3.right * laneDistance;
        }

        transform.position = Vector3.Lerp(transform.position ,targetPosition, 80 * Time.fixedDeltaTime); //Changed to fixedDeltaTime coz the player model was shaky 
        //originally it was = targetposition. but now changed to Vector3.Lep in order to have a smoother transition
        controller.center = controller.center;
    }

    private void FixedUpdate() //FixedUpdate is used for being in-step with the physics engine,
                               //so anything that needs to be applied to a rigidbody should happen in FixedUpdate.
                               //Update, on the other hand, works independantly of the physics engine.
                               //This can be benificial if a user's framerate were to drop but you need a certain calculation to keep executing,
                               //like if you were updating a chat or voip client, you would want regular old update. - according to google
    {
        if(!PlayerManager.isGameStarted)
            return;

        controller.Move(direction * Time.fixedDeltaTime);
        animator.SetBool("isGameStarted", true);
    }

    private void Jump()
    {
        direction.y = jumpForce;
        playerIsGrounded = false;
        animator.SetBool("isGrounded", true);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.transform.tag == "Obstacle" )
        {
            PlayerManager.gameOver = true; //Can access PlayerManager.gameOver because gameOver is static
            FindObjectOfType<AudioManager>().StopSound("Main OST");
            FindObjectOfType<AudioManager>().PlaySound("Game Over SFX");
        }

        if(hit.transform.tag == "Ground")
        {
            playerIsGrounded = true;
            animator.SetBool("isGrounded", false);
        }
    }

    private IEnumerator Slide()
    {
        isSliding = true;
        animator.SetBool("isSliding", true);
        controller.center = new Vector3(0, -0.5f, 0);
        controller.height = 1;

        yield return new WaitForSeconds(0.6f);

        controller.center = new Vector3(0, 0, 0);
        controller.height = 2;
        animator.SetBool("isSliding", false);
        isSliding = false;
    }
}
