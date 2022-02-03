using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cooldown_Script : MonoBehaviour
{
    public Image Bar;

    bool isCooling = false;
    float abilityDuration;
    float cooldownDuration;
    float elapsedTime;

    private void Start()
    {
        Bar.fillAmount = 0;
    }

    private void Update()
    {
        if (isCooling && elapsedTime < cooldownDuration + abilityDuration)
        {
            elapsedTime += Time.deltaTime;
            Bar.fillAmount = Mathf.Clamp((cooldownDuration + abilityDuration - elapsedTime) / cooldownDuration, 0, 1);
            //print("Cooling down: " + (cooldownDuration + abilityDuration - elapsedTime));
        }
    }

    public void Cooldown(float ability, float cooldown)
    {
        //print("Cooldown recieved by HUD");
        Bar.fillAmount = 1;
        elapsedTime = 0;
        abilityDuration = ability;
        cooldownDuration = cooldown;
        isCooling = true;
    }
}
