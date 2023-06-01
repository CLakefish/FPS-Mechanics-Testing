using System.Collections;
using System.Collections.Generic; using CUtilities.Entity;
using UnityEngine;

public class Target : Health
{
    public override void OnDamage(int damage)
    {
        Debug.Log("hit");
        CurrentHP -= damage;
        //throw new System.NotImplementedException();
    }

    public override void OnDeath()
    {
        //CurrentHP = MaxHP;
        //throw new System.NotImplementedException();
    }

    public override void OnGainHealth(int health)
    {
        //throw new System.NotImplementedException();
    }
}
