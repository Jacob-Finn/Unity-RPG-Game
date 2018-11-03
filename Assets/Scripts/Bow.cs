using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : Weapon {
    private float nextFire;
    public Bow(Rarity rarity, int weaponLevel, int weight, float fireRate, int weaponRange, GameObject projectileHandler, GameObject ammoType = null)
    {
        canDoDamage = false;
        this.projectileHandler = projectileHandler;
        this.attackRate = fireRate;
        this.ammoType = ammoType;
        this.handRequirement = 2;
        this.range = weaponRange;
        this.baseDamage = weaponLevel;
        this.weight = weight;
        this.rarity = rarity;
        switch (rarity) // Here we're going to use rarity n order to determine the damage. The better the rarity, the more damage.
        {
            case Rarity.Crap:
                this.baseDamage = (int)(this.baseDamage * Random.Range(1.04f, 1.08f));
                break;
            case Rarity.Common:
                this.baseDamage = (int)(this.baseDamage * Random.Range(1.09f, 1.20f));
                break;
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



    }

    override
    public void OnClick() // If we left click, what does the sword do? Inherited from Weapon
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + attackRate;
            Fire(container.gameObject);
        }
        else
        {
            return;
        }
  
    }

    override
    public void Animation(GameObject gameObject)
    {
    }
    override
    public void AnimationEnd(GameObject gameObject)
    {

    }
    public void Fire(GameObject me)
    {
      GameObject projectile = MonoBehaviour.Instantiate(ammoType);
      projectile.transform.position = projectileHandler.transform.position;
      Rigidbody rb = projectile.GetComponent<Rigidbody>();
      Arrow projectileArrow = projectile.GetComponent<Arrow>();
      projectileArrow.arrowDamage = this.baseDamage;
      rb.velocity = me.gameObject.transform.TransformDirection(Vector3.forward) * 55;
    }


}
