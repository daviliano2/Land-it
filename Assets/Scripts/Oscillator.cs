using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector = new Vector3();
    [SerializeField] float period = 2f; // period is the time to complete a movement cycle
    [Range(0,1)] [SerializeField] float movementFactor = 0f;

    Vector3 startingPosition;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (period <= Mathf.Epsilon) return; // safety condition to prevent the use of 'period' if it is 0

        float cycles = Time.time / period; // grows continually from 0

        const float tau = Mathf.PI * 2f; // check tau in google
        float rawSinWave = Mathf.Sin(cycles * tau); // goes from -1 to +1

        movementFactor = rawSinWave / 2f + 0.5f; // We do this because we want the movement factor to go from 0 to 1

        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPosition + offset;
    }
}
