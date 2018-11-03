using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
public class BasicAI : MonoBehaviour // Organize this script later! It really needs it :(
{
    public bool needsWeapon = false; // Does the AI need a weapon?
    public float searchRadius = 25f; // how far the AI can "see" to grab a weapon.
    Actor actor; // the health component of the AI
    public int tester = 0; // I am a test variable and do nothing but help debug.
    NavMeshAgent agent;
    WeaponContainer weaponContainer;
    float pickupDistance = 4f; // how far the AI can pick items up?
    public bool hasWeapon = true; // Do we have a weapon?
    public string target; // <---- Don't forget this! This is the string version of the tag of the enemy. Defined in Unity Inspec.
    public float playerDistance; // Our distance from the gameobject player
    public bool isMelee = true; // Used to define where we should stop, currently we only have swords so this is always true.
    private int closestEnemy = -1; // used for finding the closest enemy
    private Vector3 idolPos;
    int closestItem = -1;         // used for finding the closest item
    float smallestDistance = -1; // used for finding the smallest distance
    Collider theClosestItem;
    //public List<GameObject> enemy;    // Later on I want to cache this to make it quicker rather than have each AI look for an enemy


    private List<GameObject> Enemy()
    {

        List<GameObject> output = new List<GameObject>();
        output.AddRange(GameObject.FindGameObjectsWithTag(target));
        return output;
    }


    void Update() // update is used for logic control. Such as if the AI doesn't have a weapon.
    {


        if (Enemy().Contains(null)) // If there is a null in the list then something has happened (Probably a death)
        {                                // we will reiterate through the list to grab a closest enemy.
            Enemy().Remove(null);
            SearchForEnemy();
        }


        if (actor.equippedItemGameobject == null)  // if we have nothing out, then we need a weapon
        {
            hasWeapon = false;
            SearchForItems();
        }
        else // If the equipped item isn't null, then we have something, we don't need a weapon.
        {
            hasWeapon = true;
        }
        if (needsWeapon) // if we need a weapon then we'll pick one up from the search for items script
        {
            float weaponDistance = Vector3.Distance(this.gameObject.transform.position, theClosestItem.gameObject.transform.position);
            if (weaponDistance <= pickupDistance)
            {
                ItemContainer gameItem = theClosestItem.GetComponent<ItemContainer>();
                if (gameItem.isOut == false)
                {
                    actor.PickupItem(theClosestItem.gameObject);
                     weaponContainer = this.actor.equippedItemGameobject.GetComponent<WeaponContainer>();
                }
                needsWeapon = false;
            }

        }



        if (hasWeapon && Enemy() != null) // If we have our weapon and their is an enemy to be had, chase
        {
            ChasePlayer();
            if(Enemy().Count==0)agent.SetDestination(idolPos);
        }
    }



    private void SearchForEnemy()
    {
        smallestDistance = -1;
        for (int i = 0; i < Enemy().Count; i++)
        {
            float distance = Vector3.Distance(Enemy()[i].gameObject.transform.position, this.gameObject.transform.position);
           
            if (distance < smallestDistance | smallestDistance == -1)
            {
                smallestDistance = distance;
                closestEnemy = i;
            }
            playerDistance = Vector3.Distance(this.gameObject.transform.position, Enemy()[closestEnemy].transform.position);
        }
    } // the search method for finding the closest enemy.







    private void Awake()
    {
        if (target.Length == 0)
        {
            print(this.gameObject.name + " doesn't have a target defined! Will not function.");
        }
        
        if (Enemy() != null) // populating the list on AI awake in order to avoid errors, Update will maintian the list.
        {
            for (int i = 0; i < Enemy().Count; i++)
            {
                float distance = Vector3.Distance(Enemy()[i].gameObject.transform.position, this.gameObject.transform.position);
                if (distance < smallestDistance | smallestDistance == -1)
                {
                    smallestDistance = distance;
                    closestEnemy = i;
                }
                playerDistance = Vector3.Distance(this.gameObject.transform.position, Enemy()[closestEnemy].transform.position);
            }
        }
        actor = GetComponent<Actor>();
        agent = GetComponent<NavMeshAgent>();
        if (agent == null) // Ai has to have a navmesh agent, if not, we're going to add a default.
        {
            this.gameObject.AddComponent<NavMeshAgent>();
            agent = GetComponent<NavMeshAgent>();
        }


    } // used for initilization

    void SearchForItems()
    {
        LayerMask layerMask = 1 << 8; // Ignore all colliders except for colliders on layer 8
        Collider[] nearbyInteractable = Physics.OverlapSphere(this.gameObject.transform.position, searchRadius, layerMask); // an array of nearby colliders
        var nearbyInteractableList = nearbyInteractable.ToList(); // a list of the arrays
        foreach (Collider item in nearbyInteractableList)
        {
            WeaponContainer gameItem = item.gameObject.GetComponent<WeaponContainer>();
            if (gameItem == null)
            {
                //   There are no nearby weapons. RUN!!! Still need to work on this
                
            }
            else
            {
                if (gameItem.isOut == false) // If we DO find an item and the AI isn't already holding an item, he will path find towards it after finding the closest.
                {
                    for (int i = 0; i < nearbyInteractableList.Count; i++)
                    {
                        float distance = Vector3.Distance(nearbyInteractableList[i].gameObject.transform.position, this.gameObject.transform.position);
                        if (distance < smallestDistance | smallestDistance == -1)
                        {
                            smallestDistance = distance;
                            closestItem = i;
                            
                        }
                    }
                    if (closestItem != -1) // There is a weapon nearby.
                    {
                        theClosestItem = nearbyInteractableList[closestItem];
                        agent.SetDestination(theClosestItem.gameObject.transform.position);
                        needsWeapon = true;
                    }

                }
            }
        }

    }
    
    void ChasePlayer()
    {
        if (isMelee)
        {
            SearchForEnemy();
            if (Enemy().Count == 0)
            {
                idolPos = gameObject.transform.position; 
                return;
            }
            this.gameObject.transform.LookAt(Enemy()[closestEnemy].transform.position);
            float aistoppingDistance = 3.2f; // we have a sword, so lets stop pretty close to the player.
            agent.stoppingDistance = aistoppingDistance;
            agent.SetDestination(Enemy()[closestEnemy].transform.position);
            if (playerDistance <= (agent.stoppingDistance + 3.5f))
            {
                StartCoroutine("Attack");
            }
            else // not near us anymore so we cannot attack
            {

            }

        }
        else
        {
            // if we aren't melee then we're probably ranged
        }
    }
    IEnumerator Attack()
    {

        weaponContainer.Use();
        yield return new WaitForSeconds(1);
        

    } // Grabs the WC and uses the Swing method.
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(this.gameObject.transform.position, searchRadius);
    } // Only used for editor visual stuff



}
