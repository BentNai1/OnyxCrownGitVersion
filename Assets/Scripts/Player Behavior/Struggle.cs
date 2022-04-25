using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Struggle : MonoBehaviour
{
    [Header("- Variables for Corrutped")]
    public bool isStruggling;

    [Header("- Button Mash")]
    public float mashDelay = 1f;
    public float mash;
    public bool pressed;
    public int buttonHoldTime;

    [Header("- Scritps")]
    PlayerMovement playerScript2;
    public CharacterController playerCharacterController;
    private Corrupted_Script enemyHoldingPlayer;
    

    void Start()
    {
        mash = mashDelay;

        playerScript2 = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (isStruggling == true)
        {


            if(Input.GetButtonDown("Fire3") && !pressed)
            {
                pressed = true;
                mash += .5f;
                
            }

            else if(Input.GetButtonUp("Fire3"))
            {
                buttonHoldTime = 0;
                pressed = false;
            }

            if (pressed == true)
            {
                buttonHoldTime += 1;
            }

            if (mash >= 7 || buttonHoldTime >= 125)
            {
                isStruggling = false;
                playerScript2.isGrabbed = false;
                enemyHoldingPlayer.isHoldingPlayer = false;
                Debug.Log("Player finished struggling, can move again");
                enemyHoldingPlayer.StunThisEnemy();
                mash = 0;
            }
        }
    }

    public void StartStruggling (Corrupted_Script EnemyHoldingThePlayer)
    {
        isStruggling = true;
        enemyHoldingPlayer = EnemyHoldingThePlayer;
        //turn off normal player movement
        playerScript2.isGrabbed = true;
    }
}
