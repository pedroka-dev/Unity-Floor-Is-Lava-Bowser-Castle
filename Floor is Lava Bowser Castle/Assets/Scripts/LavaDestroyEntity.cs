using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaDestroyEntity : MonoBehaviour
{
    Rigidbody rb;
    AudioSource audioSource;
    public AudioClip destroyEntityAudioClip;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
    }

    void DestroySelf()
    {
        Destroy(rb.gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Death"))
        {
            rb.angularVelocity = Vector3.zero;
            rb.velocity = Vector3.zero;
            audioSource.PlayOneShot(destroyEntityAudioClip,0.4f);
            Invoke("DestroySelf",0.16f);
        }
    }
}
