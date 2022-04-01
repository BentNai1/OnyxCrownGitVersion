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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        randomnumber = Random.Range (0,15);
        Debug.Log (randomnumber); 
        if (randomnumber > 5 && randomnumber <= 10)
        {
            audioSource.clip = ambientSound;
        }
        if (randomnumber <= 5)
        {
            audioSource.clip = ambientSound2;
        }
        if (randomnumber >= 10)
        {
            audioSource.clip = ambientSound;
        }
    }
}
