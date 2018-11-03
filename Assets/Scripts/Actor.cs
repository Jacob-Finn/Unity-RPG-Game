using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Actor : MonoBehaviour {
    // The basic script of all game character, contains health and basic stats that we can use for manipulating what the
    // that we can use for manipulating what the gameobject holds.
    public Slider healthBar;
    public int health = 100;
    public int maxHealth;
    public int strength = 5;
    public int intelligence = 5;
    public int agility = 5;
    public bool isPlayer = false;
    public int luck = 5;
    Inventory inventory; // Our inventory if we have one.
    WeaponContainer weaponContainer; // Used for visual representation of debugging. REMOVE LATER
    public GameObject equippedItemGameobject; // What item do we currently have out?
    public ItemContainer equippedItem;
    public GameObject itemHolder; // This is used for positioning the equippedItem and it should be an empty attached to the 
                                  // parent object.
    public int team = 0; //0=neutral, 1 = player, -1 = enemy


    void Awake()
    {
        this.gameObject.name = this.gameObject.transform.parent.name;
        maxHealth = health;
        if (itemHolder == null) // This is just in-case I accidently forget to set the ItemHolder in the inspector,
        {                      // If it is still attached, then this will try to find it. Otherwise it reports an error.
            try
            {
                itemHolder = this.gameObject.transform.Find("ItemHolder").gameObject;
            }
            catch
            {
                print("An ItemHolder hasn't been assigned! This will cause issues later on! - " + gameObject.name);
            }
        }

        inventory = this.gameObject.GetComponent<Inventory>();
        gameObject.layer = 9;
    }

    private void Start()
    {
        if (isPlayer && healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = health;
        }
        GameManager.linkedActors.Add(this.gameObject, this);

    }

    public void UseItem()
    {
        if (equippedItem == null) return;
        equippedItem.Use();

    }


    public void DropItem()
    {
        if (equippedItemGameobject == null) return;
        BoxCollider itemCollider = equippedItemGameobject.GetComponent<BoxCollider>(); // grabbing the MeshCollider of object to drop
        ItemContainer equippedContainer = equippedItemGameobject.GetComponent<ItemContainer>(); // grabbing the ItemContainer script
        Animator itemAnimator = equippedItemGameobject.GetComponent<Animator>();
        itemCollider.isTrigger = false; // Turning off the isTrigger on the equipped item's collider so it wont hurt us anymore
        if (itemAnimator != null)
        {
            itemAnimator.enabled = false;
        }
        this.equippedItemGameobject.transform.parent = null; // Removing the player as the parent of the item. 
        equippedContainer.isOut = false; // Returning this value to its default, as we aren't holding it out anymore
        equippedItemGameobject.AddComponent<Rigidbody>(); // Adding a rigidbody to give the object gravity and physics.
        equippedItemGameobject = null; // We're reseting our equipped item to nothing so we can pick up things again.   
    }

    public Weapon GetWeapon()
    {
        Weapon w = null;
        WeaponContainer wc = (WeaponContainer)equippedItemGameobject.GetComponent<ItemContainer>();
        w = wc.weapon;
        return w; // this needs to be re-wrote. This is trash.
    }
    





    public void PickupItem(GameObject item) // item is the object we're picking up.
    {
        if (equippedItemGameobject == null) // If we don't already have something. If we do, we'll add it to inventory instead.
        {

            BoxCollider itemCollider = item.GetComponent<BoxCollider>(); 
            if (itemCollider != null) // if the object does have a MeshCollider
            {
                ItemContainer gameItem = item.gameObject.GetComponent<ItemContainer>();
                if (gameItem.isOut) return;
                equippedItem = gameItem;
                gameItem.holder = this;
                gameItem.isOut = true; // Going to pick up the object, so it will be out.
                equippedItemGameobject = item;   // Setting equippeditem to the item we're picking up
                Destroy(item.GetComponent<Rigidbody>()); // Getting rid of the object's gravity.
                itemCollider.isTrigger = true; // Making the object a trigger so that it can do damage.
                item.transform.SetPositionAndRotation(itemHolder.transform.position, itemHolder.transform.rotation);
                // Set the position of the object we're picking up to the ItemHolder
                item.transform.parent = itemHolder.transform.parent.transform; // Make the object move with the player.
                Animator itemAnimator = item.gameObject.GetComponent<Animator>();
                if (itemAnimator != null)
                {
                    itemAnimator.enabled = true;
                }
                else
                {
                    return;
                }
                
            }
            else
            {
                print(item.gameObject.name + " doesn't contain a mesh collider"); // If the item doesn't have a mesh collider
            }                                                                     // we'll get errors later on, so we check
        }                                                                         // before picking it up.
        else
        {
            // add to inventory later!
            print("Adding to my fake inventory.");
            inventory.AddToInventory(item.GetComponent<ItemContainer>()); // A very temporary fix. Later on make PickupItem use the item script as a condition rather than the gameobject.
            return;
        }
    }
    public void CheckHealth()
    {
        if (health > maxHealth) health = maxHealth;
        if (isPlayer)
        {
            print("Taking damage");
            healthBar.value = health;
        }
        if (health <= 0)
        {
            Death();
        }
    }
    public void RestoreHealth(int healthRestored)
    {
        health += healthRestored;
        CheckHealth();

    }
	public void TakeDamge(int damageTaken)
    {
        health -= damageTaken;
        CheckHealth();
    }
    public void Death()
    {
        DropItem();
        GameManager.linkedActors.Remove(this.gameObject);
        GameObject thisParent = this.gameObject.transform.parent.gameObject;
        if (thisParent != null)
        {
            Destroy(thisParent); // Anytime we take damage we need to check this method.

        }
        else
        {
            Destroy(this.gameObject);
        }
    }





}
