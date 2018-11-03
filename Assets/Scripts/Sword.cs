using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon    // Sword extends Weapon as it is a kind of Weapon.
{
    string[] prefix = new string[] { "Amazing", "Okay", "Decent", "Superb", "Powerful", "Iron", "Crystallized" }; // Random prefixes the sword can choose from to create its name.
    private float nextAttack;
    public bool isSwinging = false; //swinging forwards
    public bool recoiling = false; //moving back to defau lt position
    public float maxSwing;
    public float attackPeakTime; //time we start recoiling
    public float swingDuration; //for how long we swing one way
    public float swingTime; //time we started swinging

    public Sword(Rarity rarity, int weaponLevel, int weight, float attackRate) // Constructor
    {
        this.attackRate = attackRate; // How fast we'll attack
        animationPlaying = false; // These are only used for animation control
        canDoDamage = false;     //*
        maxSwing = 0.6f;         //*
        attackRate = 0.5f;       //*
        attackPeakTime = 0f;     //*
        swingDuration = .3333f; //for how long we swing one way
        swingTime = 0f; //time we started swinging
        this.weight = weight;
        this.handRequirement = 1; // Is the weapon one-handed or two-handed?
        this.baseDamage = weaponLevel; // We're going to set a base level of damage so that a lvl 70 sword will not be super weak.
        this.rarity = rarity; // We're taking the rarity from the condition statement so that way the constructed sword knows what rarity it is
        switch (rarity) // Here we're going to use rarity n order to determine the damage. The better the rarity, the more damage.
        {
            case Rarity.Crap:
                this.baseDamage = (int)(this.baseDamage * Random.Range(1.04f, 1.08f)); // This quality of item is very bad and doesn't get much of a modifier. We're going to 
                break;                                                                 // multiply from a range of 1.04 (inclusive) to a range of 1.08 (exclusive) then, since we don't want any decimal
            case Rarity.Common:                                                        // places in our damage, we're going to cast the final result into an int.
                this.baseDamage = (int)(this.baseDamage * Random.Range(1.09f, 1.20f)); // For the different quality of the item, we're going to multiply by a different amount. The amount isn't specific and
                break;                                                                 // is mostly just what I felt was appropriate for my game.
            case Rarity.Rare:
                this.baseDamage = (int)(this.baseDamage * Random.Range(1.21f, 1.45f));
                break;
            case Rarity.Legendary:
                this.baseDamage = (int)(this.baseDamage * Random.Range(1.45f, 1.75f));
                break;
            case Rarity.Godlike:
                this.baseDamage = (int)(this.baseDamage * Random.Range(1.75f, 2.05f));
                break;
        }
        this.name = prefix[Random.Range(0, prefix.Length)] + " Sword";
    }
    override
    public void OnClick() // If we left click, what does the sword do? Inherited from Weapon and allows for me to change what seperate weapons do on attack.
    {
        if (Time.time > nextAttack)
        {


            nextAttack = Time.time + attackRate;
            if (container.weaponAnimation.GetCurrentAnimatorStateInfo(0).IsName("Swing")) return; // If we're already playing the swing animation, moving forward would mess up the animation.
            Animation(container.gameObject);

        }
        else
        {
            return;
        }
    }
    override
    public void Animation(GameObject gameObject)
    {
        container.weaponAnimation.SetBool("isAttacking", true);
        canDoDamage = true;

    }
    override
    public void AnimationEnd(GameObject gameObject)
    {
        container.weaponAnimation.SetBool("isAttacking", false);
        canDoDamage = false;

    }
        



}