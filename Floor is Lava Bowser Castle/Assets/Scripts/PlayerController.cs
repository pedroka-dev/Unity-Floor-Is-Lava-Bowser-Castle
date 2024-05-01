using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    AudioSource audioSource;

    public AudioClip deathAudioClip;
    public AudioClip jumpAudioClip;
    public AudioClip pickupCoinsAudioClip;

    public Transform respawnLocation;

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
        if (Input.GetButton("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f )
        {
            //Debug.Log("JUMP! CoyoteTime = " + coyoteTimeCounter + "; JumpBuffer = " + jumpBufferCounter);
            //Debug.DrawRay(rb.position, rb.position.normalized, Color.red, 50f);

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
        if (collision.gameObject.CompareTag("Death"))
        {
            audioSource.PlayOneShot(deathAudioClip, 1.8f);

            //TODO Death
            rb.velocity = Vector3.zero;
            rb.position = respawnLocation.position;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        var contactPoint = collision.contacts[0];
        if (collision.gameObject.CompareTag("Floor") && !jumpedPreviousFrame && contactPoint.normal.y >= 0.34)
        {
            isGrounded = true;
            //Debug.DrawRay(contactPoint.point, contactPoint.normal, Color.yellow, 50f);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            //Debug.Log("OnCollisionExit");
            isGrounded = false;
            jumpBufferCounter = 0f;
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
