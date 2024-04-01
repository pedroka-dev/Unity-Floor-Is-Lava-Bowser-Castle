using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    AudioSource audioSource;

    public AudioClip jumpAudioClip;
    public AudioClip deathAudioClip;
    public AudioClip pickupCoinsAudioClip;

    public float movementForce = 0.1f;
    public float maxSpeed = 10f;

    public float jumpForce = 3; 
    private bool isGrounded = false;
    private bool jumpedPreviousFrame = false;   //fixes a bug that allows double jump becasue of coyote time and how unity handle OnCollision events

    public float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    public float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        HandlePlayerJump();
        HandlePlayerMovement();
    }

    void HandlePlayerJump()
    {
        jumpedPreviousFrame = false;

        //Coyote time allows player to jump a brief moment after being on air
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        //Jump buffer allows player to jump for a brief moment before touching the ground
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f )
        {
            //rigidbody.velocity = new Vector3(rigidbody.velocity.x, jumpThrust, rigidbody.velocity.z);
            Debug.Log("JUMP! CoyoteTime = " + coyoteTimeCounter + "; JumpBuffer = " + jumpBufferCounter + "; allowJump = ");
            Debug.DrawRay(rb.position, rb.position, Color.green, 2f);

            rb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
            
            audioSource.PlayOneShot(jumpAudioClip, 0.3f);
            isGrounded = false;
            jumpedPreviousFrame = true;
            coyoteTimeCounter = 0f;
            jumpBufferCounter = 0f;
        }
        //Allows smaller jumps if the players release the jump button early
        //if (Input.GetButtonUp("Jump") && rigidbody.velocity.y > 0f)
        //{
        //    rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y * 0.5f, rigidbody.velocity.z);
        //}
    }

    void HandlePlayerMovement()
    {
        //Player movement on the X axis
        if (Input.GetButton("Horizontal"))
        {
            float directionX = Input.GetAxis("Horizontal");
            rb.AddForce(movementForce * directionX, 0, 0, ForceMode.VelocityChange);
        }

        //Player movement on the Z axis
        if (Input.GetButton("Vertical"))
        {
            float directionZ = Input.GetAxis("Vertical");
            rb.AddForce(0, 0, movementForce  * directionZ, ForceMode.VelocityChange);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = true;
            Debug.DrawRay(collision.contacts[0].point, collision.contacts[0].normal, Color.yellow, 100f);
        }

        if (collision.gameObject.CompareTag("Death"))
        {
            audioSource.PlayOneShot(deathAudioClip, 1.8f);

            //TODO Death
            rb.velocity = Vector3.zero;
            rb.position = new Vector3(16, 20, 0);
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor") && !jumpedPreviousFrame)
        {
            isGrounded = true;
            Debug.DrawRay(collision.contacts[0].point, collision.contacts[0].normal, Color.red, 100f);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            //Debug.Log("OnCollisionExit");
            isGrounded = false;
            jumpBufferCounter = 0f;
            //Debug.DrawRay(collision.contacts[0].point, collision.contacts[0].normal, Color.blue, 10f);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Coin"))
        {
            audioSource.PlayOneShot(pickupCoinsAudioClip, 1.8f);

            //TODO Coin pickup
            Destroy(collider.gameObject);
        }
    }
}
