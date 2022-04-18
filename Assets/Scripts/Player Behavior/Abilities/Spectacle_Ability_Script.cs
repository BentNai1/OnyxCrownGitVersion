using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spectacle_Ability_Script : MonoBehaviour
{
    private SphereCollider specBounds;
    //public GameObject HUD;
    public GameObject Collider;
    bool isCooling = false;
    private IEnumerator coroutine;
    public ParticleSystem specs;

    //OBSOLETE
    //[SerializeField]
    //private float manaDrain = 31;

    [SerializeField]
    public float abilityDuration = 3;
    [SerializeField]
    public float cooldownDuration = 3;

    public AudioSource spectacleSound;

    private void Awake()
    {
        specBounds = Collider.GetComponent<SphereCollider>();
    }

    private void Start()
    {
        specBounds.enabled = false;
    }

    //void Update()
    //{
    //    //Input
    //    if (Input.GetButtonDown("Fire2") /* OBSOLETE: && GetComponent<Player_Mana>().ConsumeMana(manaDrain)*/)
    //    {
    //        Activate();
    //    }
    //}

    public void Activate()
    {
        if (!isCooling)
        {
            specBounds.enabled = true;
            print("Activated spectacle");
            Spectacle();

            spectacleSound.Play();
        }
    }
    
    public void Spectacle()
    {
        //Collider.GetComponent<EnemyTagger_Script>().spectacleStart();

        isCooling = true;
        
        coroutine = CooldownDelay();
        StartCoroutine(coroutine);

        //Calls to the Cooldown bar on the UI to begin animating with given durations
        //HUD.GetComponent<Cooldown_Script>().Cooldown(abilityDuration, cooldownDuration);
    }

    //Timing of the ability
    private IEnumerator CooldownDelay()
    {
        specs.Play();

        yield return new WaitForSeconds(abilityDuration);
        //Collider.GetComponent<EnemyTagger_Script>().spectacleOn = false;
        specBounds.enabled = false;
        specs.Stop();
        yield return new WaitForSeconds(cooldownDuration);
        isCooling = false;

    }
}
