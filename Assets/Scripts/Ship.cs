using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        ProcessInput();
    }

    void ProcessInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            print("thrusting");
        }

        if (Input.GetKey(KeyCode.A))
        {
            print("rotating left");
        }
        else if (Input.GetKey(KeyCode.D))
        {
            print("rotating right");
        }
    }
}
