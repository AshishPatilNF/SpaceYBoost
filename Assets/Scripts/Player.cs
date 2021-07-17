using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField]
    float upThrust = 1000;

    [SerializeField]
    float rotateThrust = 100;

    [SerializeField]
    float levelLoadingDelay = 2.5f;

    [SerializeField]
    AudioClip[] playerAudioClips;


    int currentSceneIndex;

    bool hasMotion = true;

    Rigidbody rigidBody;

    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        hasMotion = true;
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasMotion)
            Movement();
    }

    private void Movement()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (!audioSource.isPlaying)
                audioSource.PlayOneShot(playerAudioClips[0]);

            rigidBody.AddRelativeForce(upThrust * Time.deltaTime * Vector3.up);
        }
        else
        {
            if (audioSource.isPlaying)
                audioSource.Stop();
        }
        
        if (Input.GetKey(KeyCode.A))
        {
            rigidBody.freezeRotation = true;
            transform.Rotate(rotateThrust * Time.deltaTime * Vector3.forward);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rigidBody.freezeRotation = true;
            transform.Rotate(rotateThrust * Time.deltaTime * -Vector3.forward);
        }

        rigidBody.freezeRotation = false;

        //transform.Rotate(0, 0, Input.GetAxis("Horizontal") * -rotateThrust * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasMotion)
        {
            switch (collision.gameObject.tag)
            {
                case "LaunchPad":
                    break;
                case "Finish":
                    FinishLevel();
                    break;
                case "Fuel":
                    break;
                default:
                    CrashHandler();
                    break;
            }
        }
    }

    void CrashHandler()
    {
        hasMotion = false;

        if (audioSource.isPlaying)
            audioSource.Stop();

        if (!audioSource.isPlaying)
            audioSource.PlayOneShot(playerAudioClips[1]);

        Invoke(nameof(ReLoadLevel), levelLoadingDelay);
    }

    void FinishLevel()
    {
        hasMotion = false;

        if (audioSource.isPlaying)
            audioSource.Stop();

        if (!audioSource.isPlaying)
            audioSource.PlayOneShot(playerAudioClips[2]);

        Invoke(nameof(LoadNextLevel), levelLoadingDelay);
    }

    void LoadNextLevel()
    {
        int nextScene = currentSceneIndex + 1;

        if (currentSceneIndex + 1 == SceneManager.sceneCountInBuildSettings)
            nextScene = 0;

        SceneManager.LoadScene(nextScene);
    }

    void ReLoadLevel()
    {
        SceneManager.LoadScene(currentSceneIndex);
    }
}
