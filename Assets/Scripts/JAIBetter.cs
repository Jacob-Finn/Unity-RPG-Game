using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class JAIBetter : MonoBehaviour
{
    /*        FIELDS          */
    NavMeshAgent agent;
    Actor myActor;
    public Actor foe = null;
    public WeaponContainer closestWeapon = null;
    public int searchRadius = 50;
    public bool debugging = false;
    private WeaponContainer equippedWeapon = null;
    // Use this for initialization
    void Awake()
    {
        agent = this.gameObject.GetComponent<NavMeshAgent>();
        myActor = this.gameObject.GetComponentInChildren<Actor>();
        if (agent == null || myActor == null)
        {
            print("YOU HAVE A JAI OBJECT WITH NO ACTOR AND/OR AGENT");
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (myActor.equippedItemGameobject == null)
        {
            SetDistancesItems();
            if (closestWeapon == null || closestWeapon.isOut)
            {
                agent.SetDestination(this.transform.position); //no weapons avail
                return;
            }
            if (Vector3.Distance(closestWeapon.gameObject.transform.position, gameObject.transform.position) < 1.2 && myActor.health > 0)
            {
                myActor.PickupItem(closestWeapon.gameObject);
                equippedWeapon = GameManager.linkedWeapons[myActor.equippedItemGameobject];
                return;
            }
            else
            {
                agent.SetDestination(closestWeapon.gameObject.transform.position);
            }
        }
        else
        {
            SetDistancesEnemies();
            //we should have a weapon
            if (foe == null)
            {
                if (debugging) print("no enemy found");
                agent.SetDestination(this.transform.position);
                return;
            }
            if (Vector3.Distance(foe.gameObject.transform.position, this.gameObject.transform.position) < myActor.GetWeapon().range)
            {
                Vector3 targetPosition = new Vector3(foe.transform.position.x,
                                                       this.transform.position.y,
                                                       foe.transform.position.z);

                gameObject.transform.LookAt(targetPosition);
                agent.SetDestination(this.transform.position); //done moving
                RaycastHit hit;
                if (Physics.Raycast(this.transform.position, this.gameObject.transform.forward, out hit, myActor.GetWeapon().range, 1 << 9))
                {
                    if (hit.collider.gameObject == foe.gameObject)
                    {
                        equippedWeapon.Use();
                    }

                }
            }
            else
            {
                Vector3 targetPosition = new Vector3(foe.transform.position.x,
                                                       this.transform.position.y,
                                                       foe.transform.position.z);

                gameObject.transform.LookAt(targetPosition);
                this.agent.SetDestination(foe.gameObject.transform.position);
            }
        }
    }

    private void SetDistancesEnemies()
    {
        LayerMask actorMask = 1 << 9;
        Collider[] nearbyCol = Physics.OverlapSphere(this.gameObject.transform.position, searchRadius, actorMask); // an array of nearby colliders
        List<Actor> nearbyEnemies = new List<Actor>();
        for (int col = 0; col < nearbyCol.Length; col++)
        {
            Actor a = GameManager.linkedActors[nearbyCol[col].gameObject];
            if (a != null)
            {
                if (debugging) print("found an actor of team " + a.team);
                if (a.team != 0 && a.team != myActor.team)
                {
                    nearbyEnemies.Add(a);
                }
            }
        }
        if (debugging) print("nearby enemies size " + nearbyEnemies.Count);
        float closest = -1f;
        closest = -1f;
        for(int a = 0; a < nearbyEnemies.Count; a++)
        {
            float thisDistance = Vector3.Distance(nearbyEnemies[a].gameObject.transform.position, gameObject.transform.position);
            if (closest == -1 | thisDistance < closest)
            {
                closest = thisDistance;
                foe = nearbyEnemies[a];
            }
        }
        if (debugging) print("END OF SETDIST, FOE IS  " + foe + " and closestWeapon is " + closestWeapon);
    }


    private void SetDistancesItems()
    {
        LayerMask itemMask = 1 << 8;
        Collider[] nearbyCol = Physics.OverlapSphere(this.gameObject.transform.position, searchRadius, itemMask); // an array of nearby colliders
        List<WeaponContainer> nearbyItems = new List<WeaponContainer>();
        for (int col = 0; col < nearbyCol.Length; col++)
        {
            WeaponContainer wc = GameManager.linkedWeapons[nearbyCol[col].gameObject];
            if (wc != null && wc.isOut == false)
            {
                nearbyItems.Add(wc);
            }
            if (debugging) print("nearby items size " + nearbyItems.Count);
            float closest = -1f;
            for(int ic = 0; ic < nearbyItems.Count; ic++)
            {
                float thisDistance = Vector3.Distance(nearbyItems[ic].gameObject.transform.position, gameObject.transform.position);
                if (closest == -1 | thisDistance < closest)
                {
                    closest = thisDistance;
                    closestWeapon = nearbyItems[ic];
                }
            }
        }
    }
}
