using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
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
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up);
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

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward);
        }

        rigidBody.freezeRotation = false; // Stop manual rotation input
    }
}
