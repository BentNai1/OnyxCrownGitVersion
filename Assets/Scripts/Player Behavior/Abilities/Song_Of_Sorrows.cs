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
    private float manaDrain = 24;

    [SerializeField]
    private float abilityDuration = 3;

    [SerializeField]
    private float coolDownDuration = 10;

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
    
        if (Input.GetButtonDown("Fire1") && !isCooling && GetComponent<Player_Mana>().ConsumeMana(manaDrain))
        {
            lyreBounds.enabled = true;
            Lyre();
        }
    }

    public void Lyre()
    {
        isCooling = true;

        coroutine = coolDownDelay();
        StartCoroutine(coroutine);

        HUD.GetComponent<Cooldown_Script>().Cooldown(abilityDuration, coolDownDuration);
       
    }

    private IEnumerator coolDownDelay()
    {
        yield return new WaitForSeconds(abilityDuration);

        lyreBounds.enabled = false;
        yield return new WaitForSeconds(coolDownDuration);
       
        isCooling = false;
    }
}
