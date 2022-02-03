using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manabar_Script : MonoBehaviour
{
    private Image ManaBar;
    private float currentMana;
    private float maxMana;
    private float displayMana;
    public Player_Mana Player;

    private IEnumerator coroutine;
    private bool noManaAnim;

    private void Start()
    {
        //Find
        ManaBar = GetComponent<Image>();
        Player = FindObjectOfType<Player_Mana>();
        maxMana = Player.playerMaxMana;
        displayMana = maxMana;
        noManaAnim = false;
    }

    private void Update()
    {
        //Get
        currentMana = Player.playerCurrentMana;

        //Smoothly lower/raise health bar to current ammount
        if (displayMana > currentMana)
        {
            displayMana -= ((displayMana - currentMana) / 100) + 0.001f;
            //print("Player current: " + currentMana);
        }

        if (displayMana < currentMana)
        {
            displayMana += Mathf.Abs(displayMana - currentMana) / 100;
            //print("Player current: " + currentMana);
        }

        if (!noManaAnim)
        {
            //Display
            ManaBar.fillAmount = displayMana / maxMana;
        }
    }

    public void NotEnoughMana()
    {
        noManaAnim = true;
        coroutine = NotEnoughManaSequence();
        StartCoroutine(coroutine);
    }

    private IEnumerator NotEnoughManaSequence()
    {
        ManaBar.color = Color.black;
        ManaBar.fillAmount = 100;
        yield return new WaitForSeconds(0.1f);
        ManaBar.color = Color.blue;
        ManaBar.fillAmount = currentMana;
        yield return new WaitForSeconds(0.1f);
        ManaBar.color = Color.black;
        ManaBar.fillAmount = 100;
        yield return new WaitForSeconds(0.1f);
        ManaBar.color = Color.blue;
        ManaBar.fillAmount = currentMana;
        noManaAnim = false;
    }
}
