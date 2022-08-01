using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemCopsAudioController : MonoBehaviour
{
    public static SystemCopsAudioController instance;
    
    private Animator anim;
    private AudioClip gunShotSound;
    private AudioSource audioSource;

    private void Awake()
    {
        instance = this;
        
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        gunShotSound = Resources.Load("SystemCops_FireSound") as AudioClip;
    }

    public void Shoot()
    {
        audioSource.PlayOneShot(gunShotSound);
    }
}
