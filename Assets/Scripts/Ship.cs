using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{

    [SerializeField] float thrustPower = 5f;
    [SerializeField] float rotationPower = 200f;

    Rigidbody rigidBody;
    AudioSource audioSource;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        ThrustShip();
        RotateShip();
    }

    void ThrustShip()
    {
        // float thrustThisFrame = thrustPower * Time.deltaTime;
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * thrustPower);
            // if (!audioSource.isPlaying) audioSource.Play();
            audioSource.pitch = 3;
        }
        else
        {
            // audioSource.Stop();
            audioSource.pitch = 1;
        }
    }

    void RotateShip()
    {
        rigidBody.freezeRotation = true; // Freeze the rotation to manually control it

        float rotationThisFrame = rotationPower * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidBody.freezeRotation = false; // Stop manual rotation input
    }

    void OnCollisionEnter(Collision otherCollision)
    {
        switch (otherCollision.gameObject.tag)
        {
            case "Friendly":
                print("collided with friendly object");
                break;
            default:
                print("collided with hazard. BOOM");
                // Destroy(gameObject);
                break;


        }
    }
}
