using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotherAnimSoundScript : MonoBehaviour
{

    public AudioClip[] idleVocalizationSound;
    private int idleSoundIndex;

    public AudioClip[] walkingSound;
    private int walkingSoundIndex;

    public AudioClip[] spottedSound;
    private int spottedSoundIndex;

    public AudioClip[] leapPrepSound;
    private int leapPrepSoundIndex;

    public AudioClip[] leapSound;
    private int leapSoundIndex;


    public AudioSource movementAudioSource;
    public AudioSource vocalizationAudioSource;
    // Start is called before the first frame update
    void Start()
    {
        
        vocalizationAudioSource.loop = false;
        movementAudioSource.loop = true;

        PlayIdle();
        PlayWalking(true);

    }

    public void PlayIdle()
    {
        vocalizationAudioSource.clip = idleVocalizationSound[idleSoundIndex];
        vocalizationAudioSource.Play();
    }

    public void PlayWalking(bool activate)
    {
        if (activate)
        {
            movementAudioSource.clip = walkingSound[walkingSoundIndex];
            movementAudioSource.Play();
        }

        if(!activate)
        {
            movementAudioSource.Stop();
        }
        
    }

    public void PlaySpotted()
    {
        vocalizationAudioSource.clip = spottedSound[spottedSoundIndex];
        vocalizationAudioSource.Play();
    }

    public void PlayLeapPrep()
    {
        vocalizationAudioSource.clip = leapPrepSound[leapPrepSoundIndex];
        vocalizationAudioSource.Play();

        PlayWalking(false);
    }

    public void PlayLeap()
    {
        vocalizationAudioSource.clip = leapSound[leapSoundIndex];
        vocalizationAudioSource.Play();

        PlayWalking(false);
    }
}
