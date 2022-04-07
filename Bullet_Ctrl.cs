using UnityEngine;
using System.Collections;

public class Bullet_Ctrl : MonoBehaviour {

    public int damage = 20;
    public float speed = 1000.0f;


	// Use this for initialization
	void Start () {
        GetComponent<Rigidbody>().AddForce(transform.forward * speed);
        Destroy(gameObject, 3);
	}
	
	// Update is called once per frame
	void Update () {
	
	}




}
