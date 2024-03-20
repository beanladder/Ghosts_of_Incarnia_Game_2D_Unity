using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepController : MonoBehaviour
{
    public AudioClip footstepClip;
    public AudioClip dashClip;
    public AudioSource audioSource;
    

    public static FootstepController Instance;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
    }

    void Update()
    {
        

        // Check for key presses and releases
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            // If any movement key is pressed and not dashing, play the footstep audio
            if (!PlayerController.Instance.isDashing && !audioSource.isPlaying)
            {
                audioSource.clip = footstepClip;
                audioSource.Play();
            }
        }
        else
        {
            // If no movement keys are pressed, stop the footstep audio
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }

      
    }
}