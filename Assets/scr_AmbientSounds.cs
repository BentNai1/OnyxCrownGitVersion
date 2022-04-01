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
        play = false;
        randomnumber = Random.Range (0,15);
    }
    // Update is called once per frame
    void Update()
    {
        Debug.Log (randomnumber);
        Debug.Log(play);
        if(!play)
        {
            if (randomnumber > 5 && randomnumber <= 10)
            {
            audioSource.clip = ambientSound;
            audioSource.Play();
            delayTimer = audioSource.clip.length + delaytime;
            Debug.Log (delayTimer);
            play = true;
            }
         if (randomnumber <= 5)
            {
            audioSource.clip = ambientSound2;
            audioSource.Play();
            delayTimer = audioSource.clip.length + delaytime;
            Debug.Log (delayTimer);
            play = true;
            }
        if (randomnumber >= 10)
            {
            audioSource.clip = ambientSound3;
            audioSource.Play();
            delayTimer = audioSource.clip.length + delaytime;
            Debug.Log (delayTimer);
            play = true;
            } 
         }
                 if (play)
            {
            //audioSource.Play();
            delayTimer = delayTimer - Time.deltaTime;
            Debug.Log(delayTimer);
            if (delayTimer < 0)
                {
            randomnumber = Random.Range (0,15);
            play = false;
                }
             }
         
    }
}
