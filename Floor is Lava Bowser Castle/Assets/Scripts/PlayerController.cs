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

        if (coyoteTimeCounter > 0f && jumpBufferCounter >0f)
        {
            //rigidbody.velocity = new Vector3(rigidbody.velocity.x, jumpThrust, rigidbody.velocity.z);
            rb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
            audioSource.PlayOneShot(jumpAudioClip, 0.25f);
            isGrounded = false;
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
        float currentSpeed = rb.velocity.magnitude;
        float forceMultiplier;

        //This method decreases player acceleration util almost 0 after getting close to max speed
        if (currentSpeed > maxSpeed - (maxSpeed / 4))
        {
            forceMultiplier = movementForce * maxSpeed - (currentSpeed / maxSpeed);
        }
        else
        {
            forceMultiplier = 1;
        }

        //Player movement on the X axis
        if (Input.GetButton("Horizontal"))
        {
            float directionX = Input.GetAxis("Horizontal");
            rb.AddForce(movementForce * forceMultiplier * directionX, 0, 0, ForceMode.Impulse);
        }

        //Player movement on the Z axis
        if (Input.GetButton("Vertical"))
        {
            float directionZ = Input.GetAxis("Vertical");
            rb.AddForce(0, 0, movementForce * forceMultiplier * directionZ, ForceMode.Impulse);
        }
    }

    //OnCollisionStay
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = true;
        }

        if (collision.gameObject.CompareTag("Death"))
        {
            audioSource.PlayOneShot(deathAudioClip, 1.5f);

            //TODO Death
            rb.velocity = Vector3.zero;
            rb.position = new Vector3(16, 20, 0);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = false;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Coin"))
        {
            audioSource.PlayOneShot(pickupCoinsAudioClip, 1.5f);

            //TODO Coin pickup
            Destroy(collider.gameObject);
        }
    }
}
