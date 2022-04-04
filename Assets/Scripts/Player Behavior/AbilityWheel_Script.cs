using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityWheel_Script : MonoBehaviour
{
    public Sprite Cloak;
    public Sprite Spectacle;
    public Sprite Lyre;

    public void UpdateUI(int abilityNum)
    {
        switch (abilityNum)
        {
            case 0:
                AssignImage(Cloak);
                break;
            case 1:
                AssignImage(Spectacle);
                break;
            case 2:
                AssignImage(Lyre);
                break;
        }
    }

    private void AssignImage(Sprite sprite)
    {
        gameObject.GetComponent<Image>().sprite = sprite;
    }
}
