using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Of_The_Shadows : MonoBehaviour
{

    //public GameObject HUD;

   
    private IEnumerator coroutine;
    public ParticleSystem invis;

    bool isCooling = false;

   


    [SerializeField]
    private float manaDrain = 1;

    [SerializeField]
    private float abilityDuration = 4;

    private float coolDownDuration = 5;


    void Update()
    {
        if (Input.GetButtonDown("Fire3")/*&& GetComponent<Player_Mana>().ConsumeMana(manaDrain)*/)
        {
            Debug.Log("Left Shift Pressed!");
            Activate();
        }
    }

    public void Activate()
    {
        if (!isCooling)
        {
            if (/**gameObject.name.Equals ("Eggs") && **/GetComponent<Player_Health>().playerCurrentHealth > 0)
            {
                Debug.Log("Calling Invincible");
                coroutine = Invulnerable();
                StartCoroutine(coroutine);
            }

            shadowStep();
        }
    }

    public void shadowStep() 
    {
        isCooling = true;

        coroutine = coolDownDelay();
        StartCoroutine(coroutine);

        //HUD.GetComponent<Cooldown_Script>().Cooldown(abilityDuration, coolDownDuration);

    }

    private IEnumerator Invulnerable()
    {
        Physics.IgnoreLayerCollision(6, 7, true);

        Debug.Log("Invincible!");
        invis.Play();

        yield return new WaitForSeconds(abilityDuration);

        Physics.IgnoreLayerCollision(6, 7, false);

        Debug.Log("No longer Invincible.");
        invis.Stop();
    }



    private IEnumerator coolDownDelay()
    {
        yield return new WaitForSeconds(abilityDuration);


        yield return new WaitForSeconds(coolDownDuration);

        isCooling = false;
    }
    
}
