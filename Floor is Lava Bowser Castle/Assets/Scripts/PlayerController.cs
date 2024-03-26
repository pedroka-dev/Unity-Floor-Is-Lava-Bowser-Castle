using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody rigidbody;
    public float jumpThrust = 3;
    public float movementThrust = 0.1f;
    public bool isGrounded = false;

    void Start()
    {
        //Fetch the Rigidbody from the GameObject with this script attached
        rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        HandlePlayerJump();
        HandlePlayerMovement();
    }

    void HandlePlayerJump()
    {
        if (Input.GetButton("Jump") && isGrounded)
        {
            //rigidbody.velocity = new Vector3(rigidbody.velocity.x, jumpThrust, rigidbody.velocity.z);
            rigidbody.AddForce(0, jumpThrust, 0, ForceMode.Impulse);
            isGrounded = false;
        }

        //if (Input.GetButton("Jump") && rigidbody.velocity.y > 0f)
        //{
        //    rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y * 0.5f, rigidbody.velocity.z);
        //}
    }

    void HandlePlayerMovement()
    {
        if (Input.GetButton("Horizontal"))
        {
            float directionX = Input.GetAxis("Horizontal");
            Debug.Log(directionX);
            rigidbody.AddForce(movementThrust * directionX, 0, 0, ForceMode.Impulse);
        }

        if (Input.GetButton("Vertical"))
        {
            float directionZ = Input.GetAxis("Vertical");
            Debug.Log(directionZ);
            rigidbody.AddForce(0, 0, movementThrust * directionZ, ForceMode.Impulse);
        }
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = true;
        }

    }

    void OnCollisionExit(Collision collision)
    {
        //if (collision.gameObject.CompareTag("Floor"))
        //{
            isGrounded = false;
        //}
    }
}
