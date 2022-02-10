using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Song_Of_Sorrows : MonoBehaviour
{
    private SphereCollider lyreBounds;
    public GameObject HUD;
    public GameObject Collider;

    private IEnumerator coroutine;

    bool isCooling = false;

    [SerializeField]
    private float manaDrain = 4;

    [SerializeField]
    private float abilityDuration = 3;

    [SerializeField]
    private float coolDownDuration = 8;

    [SerializeField]
    private float stunDuration = 3;

    //Creates sphere collider for ability
    private void Awake()
    {
        lyreBounds = Collider.GetComponent<SphereCollider>();
    }

    private void Start()
    {
        lyreBounds.enabled = false;
    }

    
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !isCooling /**&& GetComponent<Player_Mana>().ConsumeMana(manaDrain))**/)
        {
            Debug.Log("Tab Pressed"); //Tested and being read

            lyreBounds.enabled = true;
            Debug.Log("Sphere created"); //Tested and being read, sphere created
            Lyre();
        }
    }

    //Activates Lyre Ability
    public void Lyre()
    {
        Debug.Log("Lyre Active!"); //Tested and being read

        isCooling = true;

        coroutine = coolDownDelay();
        StartCoroutine(coroutine);

        //HUD.GetComponent<Cooldown_Script>().Cooldown(abilityDuration, coolDownDuration);
       
    }

    //Makes it so you can't just spam the ability
    private IEnumerator coolDownDelay()
    {
        yield return new WaitForSeconds(abilityDuration);

        lyreBounds.enabled = false;
        yield return new WaitForSeconds(coolDownDuration);
       
        isCooling = false;
    }

}
