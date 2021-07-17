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

    int currentSceneIndex;

    bool isAlive = true;

    Rigidbody rigidBody;

    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        isAlive = true;
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
            Movement();
    }

    private void Movement()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (!audioSource.isPlaying)
                audioSource.Play();

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
        switch (collision.gameObject.tag)
        {
            case "LaunchPad":
                break;
            case "Finish":
                LoadNextLevel();
                break;
            case "Fuel":
                break;
            default:
                CrashHandler();
                break;
        }
    }

    void CrashHandler()
    {
        isAlive = false;

        if (audioSource.isPlaying)
            audioSource.Stop();

        Invoke(nameof(ReLoadLevel), 1f);
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
