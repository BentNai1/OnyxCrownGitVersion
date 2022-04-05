using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Of_The_Shadows : MonoBehaviour
{

    private IEnumerator coroutine;
    public ParticleSystem invis;

    bool isCooling = false;

   


    [SerializeField]
    private float manaDrain = 1;
    
    public float abilityDuration = 4;

    public float coolDownDuration = 5;


/*    void update()
    {
        if (input.getbuttondown("fire3"))
        {
            debug.log("left shift pressed!");
            activate();
        }
    }*/

    //Reads if player has above 0 health and calls for the ability
    public void Activate()
    {
        if (!isCooling)
        {
            if (GetComponent<Player_Health>().playerCurrentHealth > 0)
            {
                Debug.Log("Calling Invincible");
                coroutine = Invulnerable();
                StartCoroutine(coroutine);
            }

            shadowStep();
        }
    }

    //starts cooldown once coroutine is used
    public void shadowStep() 
    {
        isCooling = true;

        coroutine = coolDownDelay();
        StartCoroutine(coroutine);
    }

    //Makes it so player is only invincible to eggs and no other enemies.
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


    //cooldown
    private IEnumerator coolDownDelay()
    {
        yield return new WaitForSeconds(abilityDuration);


        yield return new WaitForSeconds(coolDownDuration);

        isCooling = false;
    }
    
}
