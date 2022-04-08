using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Song_Of_Sorrows : MonoBehaviour
{
    private SphereCollider lyreBounds;
    public GameObject HUD;
    public GameObject Collider;
    public ParticleSystem song;

    private IEnumerator coroutine;

    bool isCooling = false;

    [SerializeField]
    public float abilityDuration = 3;

    [SerializeField]
    public float coolDownDuration = 8;

    [SerializeField]
    private float stunDuration = 3;

    public AudioSource lyreSound;

    //Creates sphere collider for ability
    private void Awake()
    {
        lyreBounds = Collider.GetComponent<SphereCollider>();
    }

    private void Start()
    {
        lyreBounds.enabled = false;
    }

/*    
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log("Tab Pressed"); //Tested and being read
            Activate();
        }
    }
*/
    public void Activate()
    {
        if (!isCooling)
        {
            lyreBounds.enabled = true;
            Debug.Log("Sphere created"); //Tested and being read, sphere created
            Lyre();

            lyreSound.Play();
        }
    }

    //Activates Lyre Ability
    public void Lyre()
    {
        Debug.Log("Lyre Active!"); //Tested and being read

        isCooling = true;

        coroutine = coolDownDelay();
        StartCoroutine(coroutine);
       
    }

    //Makes it so you can't just spam the ability
    private IEnumerator coolDownDelay()
    {
        song.Play();

        yield return new WaitForSeconds(abilityDuration);

        lyreBounds.enabled = false;
        song.Stop();
        yield return new WaitForSeconds(coolDownDuration);
       
        isCooling = false;
    }

}
