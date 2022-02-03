using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManaPickup : MonoBehaviour
{
    public bool givesMana;
    public float manaAmount;
    public bool givesHealth;
    public int healthAmount;
    private bool consumed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            print("collided with pickup");
            if (givesHealth && !other.GetComponent<Player_Health>().isFullHealth)
            {
                other.GetComponent<Player_Health>().HealDamageOnPlayer(healthAmount);
                consumed = true;
            }
            if (givesMana)
            {
                if (other.GetComponent<Player_Mana>().ReplenishMana(manaAmount))
                {
                    consumed = true;
                }
            }
            if (consumed) Destroy(gameObject);
        }
    }
}
