using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {
    Rigidbody rb;
    public int arrowDamage = 10;
    public bool alive = false;
    public GameObject myParent;
    public int team;
    public float timeToDeath = 15f;
    void Awake()
    {
        alive = true;
        rb = this.gameObject.GetComponent<Rigidbody>();

    }
    private void Start()
    {
        Debug.Log(arrowDamage);
        if (alive) StartCoroutine("DeathTimer");
    }


    void Update()
    {
        if(alive) transform.rotation = Quaternion.LookRotation(rb.velocity);

    }
    private void OnCollisionEnter(Collision collision)
    {
        Actor a = collision.gameObject.GetComponent<Actor>();
        if(a == null)
        {
            Destroy(this.gameObject);
        }
        else
        {

            a.TakeDamge(arrowDamage);
            Destroy(this.gameObject);
            
        }
    }
    IEnumerator DeathTimer(){
        yield return new WaitForSeconds(timeToDeath);
        Destroy(this.gameObject);

    }


}
