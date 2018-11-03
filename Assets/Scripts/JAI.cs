using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class JAI : MonoBehaviour {
    /*        FIELDS          */
    NavMeshAgent agent;
    Actor myActor;
    public Actor foe = null;
    public WeaponContainer closestWeapon = null;
    public int searchRadius = 50;
    public bool debugging = false;
    private WeaponContainer equippedWeapon = null;
	// Use this for initialization
	void Awake () {
        agent = this.gameObject.GetComponent<NavMeshAgent>();
        myActor = this.gameObject.GetComponentInChildren<Actor>();
        if(agent == null || myActor == null)
        {
            print("YOU HAVE A JAI OBJECT WITH NO ACTOR AND/OR AGENT");
            return;
        }

	}
	
	// Update is called once per frame
	void Update () {
        SetDistances();
        if (myActor.equippedItemGameobject == null)
        {
            if (closestWeapon == null || closestWeapon.isOut)
            {
                agent.SetDestination(this.transform.position); //no weapons avail
                return;
            }
            if (Vector3.Distance(closestWeapon.gameObject.transform.position, gameObject.transform.position) < 1.2)
            {
                myActor.PickupItem(closestWeapon.gameObject);
                equippedWeapon = myActor.equippedItemGameobject.GetComponent<WeaponContainer>();
                return;
            }
            else
            {
                agent.SetDestination(closestWeapon.gameObject.transform.position);
            }
        }
        else
        {
            //we should have a weapon
            if(foe == null)
            {
               if(debugging) print("no enemy found");
                agent.SetDestination(this.transform.position);
                return;
            }
            if (Vector3.Distance(foe.gameObject.transform.position, this.gameObject.transform.position) < myActor.GetWeapon().range)
            {
                gameObject.transform.LookAt(foe.gameObject.transform.position);
                agent.SetDestination(this.transform.position); //done moving
                RaycastHit hit;
                if (Physics.Raycast(this.transform.position, this.gameObject.transform.forward, out hit, myActor.GetWeapon().range, 1 << 9))
                {
                    if(hit.collider.gameObject == foe.GetComponent<Collider>().gameObject)
                    {
                        equippedWeapon.Use();
                    }

                }
            }
            else
            {
                gameObject.transform.LookAt(foe.gameObject.transform.position);
                this.agent.SetDestination(foe.gameObject.transform.position);
            } 
        }
	}

    private void SetDistances()
    {
        LayerMask layerMask = 1 << 9;
        LayerMask layerMask2 = 1 << 8;
        LayerMask toUse = layerMask | layerMask2;
        Collider[] nearbyCol = Physics.OverlapSphere(this.gameObject.transform.position, searchRadius, toUse); // an array of nearby colliders
        List<Actor> nearbyEnemies = new List<Actor>();
        List<WeaponContainer> nearbyItems = new List<WeaponContainer>();
        for (int col = 0; col < nearbyCol.Length; col++)
        {
            Actor a = nearbyCol[col].gameObject.GetComponent<Actor>();
            WeaponContainer wc = nearbyCol[col].gameObject.GetComponent<WeaponContainer>();
            if (a != null)
            {
                if (debugging) print("found an actor of team " + a.team);
                if(a.team != 0 && a.team != myActor.team)
                {
                    nearbyEnemies.Add(a);
                }
            }
            if (wc != null && wc.isOut==false)
            {
                nearbyItems.Add(wc);
            }
        }
        if(debugging) print("nearby enemies size " + nearbyEnemies.Count);
        if (debugging) print("nearby items size " + nearbyItems.Count);
        float closest = -1f;
        foreach(WeaponContainer ic in nearbyItems)
        {
            float thisDistance = Vector3.Distance(ic.gameObject.transform.position,gameObject.transform.position);
            if(closest==-1 | thisDistance < closest)
            {
                closest = thisDistance;
                closestWeapon = ic;
            }
        }
        closest = -1f;
        foreach(Actor a in nearbyEnemies){
            float thisDistance = Vector3.Distance(a.gameObject.transform.position, gameObject.transform.position);
            if (closest == -1 | thisDistance < closest)
            {
                closest = thisDistance;
                foe = a;
            }
        }
       if(debugging) print("END OF SETDIST, FOE IS  " + foe + " and closestWeapon is " + closestWeapon);
    }


}
