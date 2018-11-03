using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item  
{
    public int modifier; // This can be used to determine how much to modify by, for instance the health potion will use it to know how much to modify health by, a strength potion will use this to know how
                         // much to modify strength by.
    public bool consumable = false;
    public bool beingUsed = false;
    public string name; // What the player will see as the name of the weapon. This should be defined in the constructor as well.
    public int weight = 10; // Role playing game value that allows for random weight on a weapon. So that a player can only "carry" so many weapons.
    public ItemContainer container = null; // Allows us to know what GameObject in Unity currently contains our object.
    public abstract void OnClick();  // On left click we will use the OnClick method of whatever the player has grabbed. Weapons should always overwrite this.
}