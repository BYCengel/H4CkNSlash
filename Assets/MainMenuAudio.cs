using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAudio : MonoBehaviour
{
    private AudioSource audioSource;
    private AudioClip doorSound;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        doorSound = Resources.Load("DoorLaserSound") as AudioClip;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            audioSource.PlayOneShot(doorSound);
        }
    }
}
