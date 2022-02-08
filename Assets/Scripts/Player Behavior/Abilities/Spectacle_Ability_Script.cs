using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spectacle_Ability_Script : MonoBehaviour
{
    private SphereCollider specBounds;
    public GameObject HUD;
    public GameObject Collider;
    bool isCooling = false;
    private IEnumerator coroutine;

    [SerializeField]
    private float manaDrain = 31;
    [SerializeField]
    private float abilityDuration = 3;
    [SerializeField]
    private float cooldownDuration = 3;

    private void Awake()
    {
        specBounds = Collider.GetComponent<SphereCollider>();
    }

    private void Start()
    {
        specBounds.enabled = false;
    }

    void Update()
    {
        //Input
        if (Input.GetButtonDown("Fire2") && !isCooling && GetComponent<Player_Mana>().ConsumeMana(manaDrain))
        {
            //Collider.GetComponent<EnemyTagger_Script>().spectacleOn = true;
            specBounds.enabled = true;
            print("Activated spectacle");
            Spectacle();
        }
    }
    
    public void Spectacle()
    {
        //Collider.GetComponent<EnemyTagger_Script>().spectacleStart();

        isCooling = true;
        
        coroutine = cooldownDelay();
        StartCoroutine(coroutine);

        //Calls to the Cooldown bar on the UI to begin animating with given durations
        HUD.GetComponent<Cooldown_Script>().Cooldown(abilityDuration, cooldownDuration);
    }

    //Timing of the ability
    private IEnumerator cooldownDelay()
    {
        yield return new WaitForSeconds(abilityDuration);
        //Collider.GetComponent<EnemyTagger_Script>().spectacleOn = false;
        specBounds.enabled = false;
        yield return new WaitForSeconds(cooldownDuration);
        isCooling = false;
    }
}