using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Struggle : MonoBehaviour
{
    [Header("- Variables for Corrutped")]
    public GameObject playerSocket;
    public bool isStruggling;

    [Header("- Button Mash")]
    public float mashDelay = 1f;
    public float mash;
    public bool pressed;
    public float buttonHoldTime = 4;

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


            if(Input.GetButtonDown("Jump") && !pressed)
            {
                pressed = true;
                mash += .5f;
            }

            else if(Input.GetButtonUp("Jump"))
            {
                pressed = false;
            }

            if(pressed)
            {
                buttonHoldTime -= Time.deltaTime;
            }

            if (mash >= 7 || buttonHoldTime <= 0)
            {
                isStruggling = false;
                playerScript2.isGrabbed = false;
                Debug.Log("Player finished struggling, can move again");
                enemyHoldingPlayer.StunThisEnemy();
                mash = 0;
                buttonHoldTime = 4;
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
