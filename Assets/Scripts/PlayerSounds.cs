using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    private AudioSource audio;
    public AudioClip blade;
    public AudioClip phaseShift;
    public AudioClip footstep1;
    public AudioClip footstep2;
    public AudioClip Jump1;
    public AudioClip Jump2;


    private void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    public void PlayBladeSound()
    {
        audio.clip = blade;
        audio.Play();
    }

    public void PlayPhaseShiftSound()
    {
        audio.clip = phaseShift;
        audio.Play();
    }

    public void FootStepSound1()
    {
        audio.clip = footstep1;
        audio.Play();
    }

    public void FootStepSound2()
    {
        audio.clip = footstep2;
        audio.Play();
    }

    public void JumpSound1()
    {
        audio.clip = Jump1;
        audio.Play();
    }

    public void JumpSound2()
    {
        audio.clip = Jump2;
        audio.Play();
    }
}
