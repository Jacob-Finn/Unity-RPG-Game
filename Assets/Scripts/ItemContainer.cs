using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemContainer : Interactable {
    public int itemModifier;
    public Item item;
    public bool isOut = false;
    public bool consumable = false;
    public Actor holder;
    public enum PotionTypes  // To-Do for later, make the weapon container use its weapon type here and then define the weapons here as well and make a custom inspector view to organize it all.
    {
       Health,
       Mana
    }
    public PotionTypes potionTypes;

    private void Awake()
    {
        switch (potionTypes)
        {
            case PotionTypes.Health:
                item = new HealthPotion(itemModifier, consumable);
                return;
            case PotionTypes.Mana:
                return;
        }
    }

    public void Start()
    {
        GameManager.linkedItems[this.gameObject] = this;
        item.container = this;
    }


    public virtual void Use()
    {
        item.OnClick();
    }
}
