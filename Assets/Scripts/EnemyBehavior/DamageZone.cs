using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class DamageZone : MonoBehaviour
{
    private Collider eggCollider;

   
    [SerializeField] private float noDamageTimer;
    private float timer;

    [Tooltip ("A value of '0' will use the default value in Player_Health.")]
    [SerializeField] private int damageDelt;


    void Start()
    {
        eggCollider = gameObject.GetComponent<Collider>();
    }

    private void Update()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        //print("Collission Detected");
        if (other.gameObject.tag == "Player" && timer <= 0)
        {
            timer = noDamageTimer;
            Player_Health playerHealth = other.GetComponent<Player_Health>();

            playerHealth.DealDamageToPlayer(damageDelt);
        }
    }

}
