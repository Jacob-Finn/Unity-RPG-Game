using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : Item { // Weapon extends Item as it is a kind of Item
    public new WeaponContainer container = null; //the container of this weapon
    public int range = 2; // what is the range of the weapon?
    public GameObject ammoType; // Used for range weaponry. Not required unless you want a projectile shot out.
    public GameObject projectileHandler; //Used for range weaponry as well. Not required.
    public float attackRate; // How fast the weapon can attack
    public bool animationPlaying = false; // Is an animation playing on the weapon?
    public bool canDoDamage = false; // Used for some weapon behaviors, determines if the weapon can do damage on hit
    public enum Rarity
    { 
     Crap,
     Common,
     Rare,
     Legendary,
     Godlike
    };
    // The enum is used to simplfy the coding process and allow me to visualize the different kinds of item rarities that there are.
    public Rarity rarity;
    public int handRequirement = 1;
    // If the weapon is 2 handed or 1 handed
    // 1 = one handed
    // 2 = two handed
    public int baseDamage = 5;
    /* baseDamage should only be used in the constructor, but since every weapon will have a base damage, we're going to go ahead and define it here for use in all weapons that utilize this class.
    Some other classes that might use this class are as follows: */
    public readonly int weaponLevel = 1; // This int is used in order to set base damage on the constructor, so we define it here as all other weapons will have a weaponLevel.
    public List<WeaponModifier> activeWeaponModifiers = new List<WeaponModifier>(); // This handles weaponModifiers in my game, but I will not be exploring these particular classes.

    public void Attack(GameObject target) // Attacking the target and dealing damage, as well as updating and handling modifiers.
    {
        Actor a = target.GetComponent<Actor>();
        if(a == null)
        {
            return;
        }
        foreach (WeaponModifier m in activeWeaponModifiers)
        {
            m.OnAttack(target); // Grabs all modifiers on the weapon.
        }
        int dmg = GetDamage(); // Used to correctly calculate damage
       // MonoBehaviour.print("Attacking for damage: " + dmg);
        a.TakeDamge(dmg); // The actor component of the target that will take damage.
    }
    public int GetDamage()
    {
        int output = baseDamage;
        foreach(WeaponModifier m in activeWeaponModifiers)
        {
            m.AttackModifier(ref output); // Updating weapon modifiers
        }
        return output; // Returns the damage that we should be doing
    }
    public abstract void Animation(GameObject gameObject);
    // swords to swing, and spears to stab. And bows to draw. This is their animation method. Is to be overwritten in the actual Sword class.
    public abstract void AnimationEnd(GameObject gameObject);
}
