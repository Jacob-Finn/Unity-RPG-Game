using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponModifier {

    public abstract void OnAttack(GameObject target);


    public abstract void AttackModifier(ref int i);
}
