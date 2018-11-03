using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// The inventory system is contained in its own script to make things easier to read and that not all actors have to have an inventory.
public class Inventory : MonoBehaviour {
    public List<ItemContainer> inventory = new List<ItemContainer>();
    private ItemContainer itemToAdd;

    public void AddToInventory(ItemContainer item)
    {
        int inventorySlot = 0; // what inventory slot are we on? Arrays start with 0 so we start with 0 
        if (inventorySlot <= 3)
        {
            inventory.Insert(inventorySlot, itemToAdd); // Inserting the item into an empty inventory slot.
            inventorySlot++; // This method needs  to be completed later. Currently no way to pull out of your inventory.
            Destroy(item); 
        }
        else
        {
            // Inventory full return.
            return;
        }
    }

}
