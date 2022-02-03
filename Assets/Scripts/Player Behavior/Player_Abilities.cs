using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public abstract class Abilities : ScriptableObject
{
    public new string name;
    public float cooldownTime;
    public float activeTime;

    public virtual void Activate(GameObject parent) {}
}