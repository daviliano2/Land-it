using UnityEngine;
using UnityEngine.SceneManagement;

public class Ship : MonoBehaviour
{
    [SerializeField] float thrustPower = 5f;
    [SerializeField] float rotationPower = 200f;
    [SerializeField] float levelLoadDelay = 2f;

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
    bool ignoreCollision = false;

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

        // Before shipping the game we need to tick off the "Development Build" 
        // checkbox in the build settings so the players cannot access to it.
        if (Debug.isDebugBuild) DebugKeys();
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
        if (state != State.Alive || ignoreCollision) return;

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
                // Destroy(gameObject, 1f);
                Invoke("LoadNextScene", levelLoadDelay);
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
                Invoke("LoadFirstScene", levelLoadDelay);
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
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int totalNumberScenes = SceneManager.sceneCountInBuildSettings;

        if (currentScene == totalNumberScenes - 1)
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            SceneManager.LoadScene(currentScene + 1);
        }
    }

    void LoadFirstScene()
    {
        SceneManager.LoadScene(0);
    }

    void DebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextScene();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            ignoreCollision = !ignoreCollision;
        }
    }
}
