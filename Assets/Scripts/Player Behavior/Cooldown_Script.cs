using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cooldown_Script : MonoBehaviour
{
    public Slider Bar;

    [HideInInspector]
    public bool isUsing = false;
    [HideInInspector]
    public bool isCooling = false;
    float abilityDuration;
    float cooldownDuration;
    float elapsedTime;

    private void Start()
    {
        Bar.value = 0;
    }

    private void Update()
    {
        if (isCooling)
        {
            if (elapsedTime < abilityDuration)
            {
                isUsing = true;
            }
            else
            {
                isUsing = false;
            }
            if (elapsedTime < cooldownDuration + abilityDuration)
            {
                elapsedTime += Time.deltaTime;
                Bar.value = Mathf.Clamp((cooldownDuration + abilityDuration - elapsedTime) / cooldownDuration, 0, 1);
                //print("Cooling down: " + (cooldownDuration + abilityDuration - elapsedTime));
            }
            else
            {
                isCooling = false;
            }
        }
    }

    public void Cooldown(float ability, float cooldown)
    {
        //print("Cooldown recieved by HUD");
        Bar.value = 1;
        elapsedTime = 0;
        abilityDuration = ability;
        cooldownDuration = cooldown;
        isCooling = true;
    }
}
