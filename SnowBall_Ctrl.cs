using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBall_Ctrl : MonoBehaviour {

    public int damage = 20;
    public float speed = 1000.0f;

    public GameObject expEffect;
    public GameObject startEffect;
    Transform tr;



    // Use this for initialization
    void Start () {

        Instantiate(startEffect, this.transform.position, Quaternion.identity);
        GetComponent<Rigidbody>().AddForce(transform.forward * speed);
        tr = GetComponent<Transform>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "SnowBall" && collision.gameObject.tag != "Enemy")
        {
            Destroy(this.gameObject);
            ExpSnowBall();
        }

    }


    void ExpSnowBall()
    {

        Instantiate(expEffect, tr.position, Quaternion.identity);

        Collider[] colls = Physics.OverlapSphere(tr.position, 5.0f);
        foreach(Collider coll in colls)
        {
            Rigidbody rbody = coll.GetComponent<Rigidbody>();

            if (coll.gameObject.tag == "Player")
                coll.gameObject.SendMessage("SnowAttacked");
            

            if(rbody != null)
            {
                rbody.mass = 1.0f;
                rbody.AddExplosionForce(1.0f, tr.position, 5.0f, 1.0f);
            }
        }


    }


}
