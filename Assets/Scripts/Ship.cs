using UnityEngine;
using UnityEngine.SceneManagement;

public class Ship : MonoBehaviour
{

    [SerializeField] float thrustPower = 5f;
    [SerializeField] float rotationPower = 200f;

    Rigidbody rigidBody;
    AudioSource audioSource;

    enum State { Alive, Dying, Transcending }
    State state = State.Alive;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (state == State.Alive)
        {
            ThrustShip();
            RotateShip();
        }
    }

    void ThrustShip()
    {
        // float thrustThisFrame = thrustPower * Time.deltaTime;
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * (thrustPower * Time.deltaTime));
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
        if (state != State.Alive) return;

        switch (otherCollision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                state = State.Transcending;
                Invoke("LoadNextScene", 1f);
                break;
            default:
                state = State.Dying;
                print("BOOM! Dead");
                Invoke("LoadFirstScene", 2f);
                // Destroy(gameObject);
                break;


        }
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(1);
    }

    void LoadFirstScene()
    {
        SceneManager.LoadScene(0);
    }
}
