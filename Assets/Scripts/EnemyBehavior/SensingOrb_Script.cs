using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensingOrb_Script : MonoBehaviour
{
    //Behavior References
    [HideInInspector]
    public GameObject Creator;
    private ShatteredAI_Script ShatteredScript;
    [HideInInspector]
    public GameObject Player;
    public LayerMask whatIsPlayer;
    
    //Behavior variables
    private float orbScale = 1;
    private float orbMin;
    private float orbMax;
    private float growRate;
    private float shrinkRate;

    //Particles
    public GameObject ParticleSys;
    private ParticleSystem ps;
    private float orbProgress;
    private bool psStarted = false;
    private float percentToStop;
    private float percentToEmitParticles;
    private float percentToSpeedParticles;
    private float percentToMaxParticles;

    private void Start()
    {
        Player = GameObject.FindWithTag("Player");
        ShatteredScript = Creator.GetComponent<ShatteredAI_Script>();
        orbScale = ShatteredScript.orbScale;
        growRate = ShatteredScript.growRate;
        orbMax = ShatteredScript.orbMax;
        shrinkRate = ShatteredScript.shrinkRate;
        orbMin = ShatteredScript.orbMin;
        if (Player == null)
        {
            print("No player!");
            Destroy(gameObject);
        }
        if (Player.GetComponent<CrouchToHide_Script>().hiding)
        {
            Destroy(gameObject);
        }

        //Particles
        percentToEmitParticles = ShatteredScript.percentToEmitParticles;
        percentToSpeedParticles = ShatteredScript.percentToSpeedParticles;
        percentToMaxParticles = ShatteredScript.percentToMaxParticles;
        ps = ParticleSys.GetComponent<ParticleSystem>();
        ps.Stop();
        percentToStop = 100 - ((growRate * 0.5f * 100) / orbMax);
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

        //Particles
        orbProgress = (orbScale * 100) / orbMax;
        if (orbProgress >= percentToEmitParticles && !psStarted)
        {
            ps.Play();
            psStarted = true;
        }
        if (psStarted)
        {
            var ParMain = ps.main;
            var ParShape = ps.shape;
            var ParEmission = ps.emission;
            if (orbProgress <= percentToSpeedParticles)
            {
                ParMain.startSpeed = -(orbScale / 5);
            }
            else if (orbProgress > percentToSpeedParticles && orbProgress < percentToMaxParticles)
            {
                ParMain.maxParticles = 20;
                ParShape.randomDirectionAmount = 0;
                ParEmission.rateOverTime = 10;
                ParMain.startSpeed = -(orbScale / 5) * 2;
                ParMain.startLifetime = 1;
            }
            else if (orbProgress >= percentToStop)
            {
                ps.Stop();
            }
            else
            {
                ParMain.startSpeed = -(orbScale / 5) * 4;
                ParMain.startLifetime = 0.5f;
            }
        }
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, orbScale/2);
    }
}
