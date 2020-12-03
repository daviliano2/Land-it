using UnityEngine;
using UnityEngine.SceneManagement;

public class Ship : MonoBehaviour
{
    [SerializeField] float thrustPower = 5f;
    [SerializeField] float rotationPower = 200f;

    [SerializeField] AudioClip droneEngine = null;
    [SerializeField] AudioClip finishBell = null;
    [SerializeField] AudioClip deathAudio = null;

    [SerializeField] ParticleSystem thrustParticleOne = null;
    [SerializeField] ParticleSystem thrustParticleTwo = null;
    [SerializeField] ParticleSystem deathParticle = null;
    [SerializeField] ParticleSystem finishParticle = null;

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
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(droneEngine);
            }
            if (!thrustParticleOne.isPlaying && !thrustParticleTwo.isPlaying) // super important!
            {
                thrustParticleOne.Play();
                thrustParticleTwo.Play();
            }
            audioSource.pitch = 3;
        }
        else
        {
            // audioSource.Stop();
            audioSource.pitch = 1;
            if (thrustParticleOne.isPlaying && thrustParticleTwo.isPlaying) // super important!
            {
                thrustParticleOne.Stop();
                thrustParticleTwo.Stop();
            }
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
                PlaySounds(finishBell);
                if (!finishParticle.isPlaying) // super important!
                {
                    finishParticle.Play();
                }
                Invoke("LoadNextScene", 1f);
                break;
            default:
                state = State.Dying;
                audioSource.Stop();
                PlaySounds(deathAudio);
                if (!deathParticle.isPlaying) // super important!
                {
                    deathParticle.Play();
                }
                print("BOOM! Dead");
                Invoke("LoadFirstScene", 2f);
                // Destroy(gameObject);
                break;


        }
    }

    void PlaySounds(AudioClip oneTimeAudio)
    {
        audioSource.volume = 0.40f;
        audioSource.PlayOneShot(oneTimeAudio);
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
