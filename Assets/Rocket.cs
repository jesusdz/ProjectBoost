using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] float mainThrust = 20.0f;
    [SerializeField] float rotationThrust = 90.0f;
    [SerializeField] AudioClip thrustClip;
    [SerializeField] AudioClip deathClip;
    [SerializeField] AudioClip winclip;
    [SerializeField] ParticleSystem thrustParticles;
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] ParticleSystem winParticles;

    Rigidbody rigidBody;
    AudioSource audioSource;

    enum State {  Alive, Dying, Transcending };
    State state = State.Alive;

    bool godModeIsEnabled = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Alive)
        {
            Thrust();
            Rotate();
        }
        
        Debug();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive)
        {
            return;
        }

        if (collision.gameObject.tag == "Obstacle" && godModeIsEnabled)
        {
            return;
        }

        switch(collision.gameObject.tag)
        {
            case "Obstacle":
                state = State.Dying;
                audioSource.Stop();
                audioSource.PlayOneShot(deathClip);
                deathParticles.Play();
                Invoke("RestartLevel", 1f);
                break;

            case "Finish":
                state = State.Transcending;
                audioSource.Stop();
                audioSource.PlayOneShot(winclip);
                winParticles.Play();
                Invoke("StartNextLevel", 1f);
                break;

            default:
                break;
        }
    }

    private void RestartLevel()
    {
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentLevel);
    }

    private void StartNextLevel()
    {
        int nextLevel = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextLevel);
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * mainThrust);

            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(thrustClip);
                thrustParticles.Play();
            }
        }
        else
        {
            audioSource.Stop();
            thrustParticles.Stop();
        }
    }

    private void Rotate()
    {
        rigidBody.freezeRotation = true; // take manual control of rotation

        float rotationThisFrame = rotationThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(- Vector3.forward * rotationThisFrame);
        }

        rigidBody.freezeRotation = false; // resume physics control of rotation
    }

    private void Debug()
    {
        if (Input.GetKeyDown(KeyCode.N)) // next level
        {
            StartNextLevel();
        }
        if (Input.GetKeyDown(KeyCode.G)) // god mode
        {
            godModeIsEnabled = !godModeIsEnabled;
        }
    }
}
