using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar_Script : MonoBehaviour
{
    private Image HealthBar;
    private float currentHealth;
    private float maxHealth;
    private float displayHealth;
    [HideInInspector]
    public Player_Health Player;
    public GameOver GameOverScript;

    private void Start()
    {
        Player = GameObject.FindWithTag("Player").GetComponent<Player_Health>();

        //Find
        HealthBar = GetComponent<Image>();
        Player = FindObjectOfType<Player_Health>();
        maxHealth = Player.playerMaxHealth;
        displayHealth = maxHealth;
        print("Player start: " + maxHealth);
        print("Player current: " + currentHealth);
        Time.timeScale = 1f;
    }

    private void Update()
    {
        //Get
        currentHealth = Player.playerCurrentHealth;

        //Smoothly lower/raise health bar to current ammount
        if (displayHealth > currentHealth)
        {
            displayHealth -= ((displayHealth - currentHealth) / 100) + 0.001f;
            //print("Player current: " + currentHealth);
        }

        if (displayHealth < currentHealth)
        {
            displayHealth += Mathf.Abs(displayHealth - currentHealth) / 100;
            //print("Player current: " + currentHealth);
        }
        if (currentHealth <= 0)
        {
            GameOverScript.Activate();

        }

        //Display
        HealthBar.fillAmount = displayHealth / maxHealth;
    }

    public void ChangeHealthBar(float newValue)
    {

    }
}
