using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensingOrb_Script : MonoBehaviour
{
    [HideInInspector]
    public GameObject Creator;
    private ShatteredAI_Script ShatteredScript;
    [HideInInspector]
    public GameObject Player;
    public LayerMask whatIsPlayer;
    
    private float orbScale = 1;
    private float orbMin;
    private float orbMax;
    private float growRate;
    private float shrinkRate;

    private void Start()
    {
        Player = GameObject.FindWithTag("Player");
        ShatteredScript = Creator.GetComponent<ShatteredAI_Script>();
        orbScale = ShatteredScript.orbScale;
        growRate = ShatteredScript.growRate;
        orbMax = ShatteredScript.orbMax;
        shrinkRate = ShatteredScript.shrinkRate;
        orbMin = ShatteredScript.orbMin;
    }

    private void Update()
    {
        //Change orb size over time depending if colliding with player or not
        if (Physics.CheckSphere(transform.position, orbScale/2, whatIsPlayer))
        {
            if (Player.GetComponent<CrouchToHide_Script>().hiding || Creator.GetComponent<ShatteredAI_Script>().activelyAttacking) Destroy(gameObject);
            orbScale += Time.deltaTime * growRate;
        }
        else
        {
            orbScale -= Time.deltaTime * shrinkRate;
        }
        transform.localScale = Vector3.one * orbScale;

        //Destroy orb if too small
        if (orbScale < orbMin) Destroy(gameObject);

        //Alert enemy if large enough
        if (orbScale > orbMax)
        {
            Creator.GetComponent<ShatteredAI_Script>().activelyAttacking = true;
            print("ATTACKINGGG");
            Destroy(gameObject);
        }
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, orbScale/2);
    }
}
