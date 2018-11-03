using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponContainer : ItemContainer {
    public int weaponLevel = 5; // Our passed variable into the constructor
    [SerializeField] public GameObject ammoType; // On weapons that are ranged, what projectile should they shoot?
    public Weapon weapon; // Our passed variable into the constructor
    public int weaponWeight = 5; // Our passed variable into the constructor
    public Weapon.Rarity rarity; // Our passed variable into the constructor
    public GameObject projectileHandler; // Used for ranged weaponry, will be found automatically if not defined.
    public float attackRate = 3f; // Our passed variable into the constructor
    public int weaponRange;  // Our passed variable into the constructor
    public enum WeaponType // Our passed variable into the constructor
    {
        Sword,
        Axe,
        Warhammer,
        Bow,
        Spear
    }
    public Animator weaponAnimation;
    public WeaponType weaponType;
    void Awake(){
        // rarity = (Weapon.Rarity)Random.Range(0, 4); // The weapon rarity for now is a random rarity between all the possible types.
        gameObject.layer = 8; // This is a Unity function and changes the sorting layer to layer 8
        switch (weaponType)
        {
            case WeaponType.Sword:
                weapon = new Sword(rarity, weaponLevel, weaponWeight, attackRate); // creating the sword with the required conditions.
                break;
            case WeaponType.Bow:
                projectileHandler = this.gameObject.transform.Find("projectileHandler").gameObject;  // We have to find a projectile handler first in order for a bow to work.
                weapon = new Bow(rarity, weaponLevel,weaponWeight, attackRate, weaponRange, projectileHandler, ammoType); // creating a bow with the required conditions.
                break;
            default:
                Debug.LogError("No implemented weapon set. Error."); // If we use a weaponType that I haven't defined yet then this will catch it.
                DestroyImmediate(this.gameObject);
                break;
        }
        weapon.container = this;
        weaponAnimation = this.gameObject.GetComponent<Animator>();
    }

    new void Start () {
        Collider weaponCollider = this.gameObject.GetComponent<Collider>();
        if (weaponCollider != null) // Gathering the Unity collider of the gameobject. If the gameobject doesn't have a collider we'll print an error.
        {
            weaponCollider.isTrigger = false; // We are setting the trigger function of the collider off so it won't do damage. 
        }
        else
        {
            Debug.LogError(gameObject.name + " doesn't contain a collider and will not function.");
        }
        GameManager.linkedWeapons.Add(this.gameObject, this);
       // weapon.activeWeaponModifiers.Add(new FiveHitModifier()); // adding the modifiers that we want the sword to have.
	}

    private void OnTriggerStay(Collider other)  // Whenever we collide with an enemy. This will trigger
    {
        Actor actor = other.gameObject.GetComponent<Actor>(); // grabbing the health script of what we collided with
        if (actor != null && weapon.canDoDamage) // If what we collided with has a health script and our weapon can do damage, then we'll go ahead and apply damage.
        {
          //  print("Attacking: " + other.gameObject.name);
            weapon.Attack(other.gameObject); 
            weapon.canDoDamage = false;   // whenever we left click, canDoDamage will be reset to true, but otherwise it is false.
        }
        else
        {
            return;
        }
        // If we collide with something, while in trigger form, do damage.
    }
    override
    public void Use()
    {
        weapon.OnClick(); // Running the OnClick method inside of the weapon that we created.
    }
    private void StopAnimation()
    {
        weapon.AnimationEnd(this.gameObject);
    }
}