using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotherAnimSoundScript : MonoBehaviour
{

    public Animator motherAnimator;

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

        if (motherAnimator.GetInteger("LookWalkAttack") != 0)
            motherAnimator.SetInteger("LookWalkAttack", 0);
    }

    public void PlayWalking(bool activate)
    {
        if (activate)
        {
            movementAudioSource.clip = walkingSound[walkingSoundIndex];
            movementAudioSource.Play();

            if (motherAnimator.GetInteger("LookWalkAttack") != 1)
                motherAnimator.SetInteger("LookWalkAttack", 1);
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

        if (motherAnimator.GetInteger("LookWalkAttack") != 2)
            motherAnimator.SetInteger("LookWalkAttack", 2);
    }

    public void PlayLeap()
    {
        vocalizationAudioSource.clip = leapSound[leapSoundIndex];
        vocalizationAudioSource.Play();

        PlayWalking(false);
    }
}
