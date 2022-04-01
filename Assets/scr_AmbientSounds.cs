using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_AmbientSounds : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip ambientSound;
    [SerializeField] AudioClip ambientSound2;
    [SerializeField] AudioClip ambientSound3;
    int randomnumber;
    bool play;
    [SerializeField] float delaytime;
    float delayTimer;
    void Start()
    {
        randomnumber = Random.Range (0,15);
        audioSource.Play();
    }
    // Update is called once per frame
    void Update()
    {
        Debug.Log (randomnumber);
        Debug.Log (delayTimer);
        Debug.Log(play);
            if (randomnumber > 5 && randomnumber <= 10)
        {
            audioSource.clip = ambientSound;
            play = true;
        }
        if (randomnumber <= 5)
        {
            audioSource.clip = ambientSound2;
            play = true;
        }
        if (randomnumber >= 10)
        {
            audioSource.clip = ambientSound3;
            play = true;
        } 
       /* if (play)
        {
            audioSource.Play();
            delayTimer -= Time.deltaTime;
            if (delayTimer < 0)
            {
            randomnumber = Random.Range (0,15);
            delayTimer = audioSource.clip.length + delaytime;
            play = false;
            }
        }*/
         
    }
}
