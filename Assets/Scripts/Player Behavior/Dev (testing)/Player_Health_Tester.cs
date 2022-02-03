using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Health_Tester : MonoBehaviour
{
    public Player_Health playerHealth;

    public int healingDone;
    public bool healPlayer = false;

    public int damageDone;
    public bool damagePlayer = false;


    // Update is called once per frame
    void Update()
    {
        if(healPlayer == true)
        {
            healPlayer = false;
            playerHealth.HealDamageOnPlayer(healingDone);
        }

        if(damagePlayer == true)
        {
            damagePlayer = false;
            playerHealth.DealDamageToPlayer(damageDone);
        }
    }
}
