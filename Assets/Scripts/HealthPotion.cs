using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : Item {

    private GameObject parent;

    public HealthPotion(int healthRestoreAmount, bool consumable)
    {
        this.consumable = consumable;
        this.modifier = healthRestoreAmount;
    }
    






    override
    public void OnClick()
    {
        MonoBehaviour.print("Clicked");
        MonoBehaviour.print(container.gameObject.name);
        GainHealth(this.container.gameObject);
    }
    void GainHealth(GameObject container)
    {
        MonoBehaviour.print("Going to restore health");
        parent = container.gameObject.transform.parent.gameObject;
        Actor a = parent.GetComponentInChildren<Actor>();   
        if (a == null)
        {
            MonoBehaviour.print("A is null");
        }
        else
        {
            a.RestoreHealth(modifier);
            if (consumable) MonoBehaviour.Destroy(container.gameObject);

        }
    }




}
