using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController : MonoBehaviour {

    Animator anim = null;
    public float turnSpeed = 1.0f;
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

        

        float walk = Input.GetAxis("Vertical");
        Debug.Log("walk = " + walk);
        anim.SetFloat("walk", walk);

        if (walk != 0.0f)
        {
            float turn = Input.GetAxis("Horizontal");
            Debug.Log("turn = " + turn);
            this.transform.Rotate(Vector3.up, turn * turnSpeed * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            anim.SetTrigger("Pickup");
        }
      
	}
}
