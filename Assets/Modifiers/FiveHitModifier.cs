using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiveHitModifier : WeaponModifier  {

    private int numHits = 0;
    private int bonusDamage = 10;
    override
    public void OnAttack(GameObject target)
    {
        numHits++;

    }
    override
    public void AttackModifier(ref int i)
    {
        if(numHits >= 5)
        {
            numHits = 0;
            i += bonusDamage;
        }

    }








































}
