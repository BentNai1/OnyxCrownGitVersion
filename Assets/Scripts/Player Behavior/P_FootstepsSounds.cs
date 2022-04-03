using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_FootstepsSounds : MonoBehaviour
{
    public AudioClip[] footstepSound;

    public AudioSource audioSource;

    public void PlayFootStep()
    {
        audioSource.clip = footstepSound[Random.Range(0, footstepSound.Length - 1)];
        audioSource.Play();
        print("Step");
    }
}
