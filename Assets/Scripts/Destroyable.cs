using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour
{
    public Collider Trigger;
    public Collider Falling;
    // Start is called before the first frame update
    void Start()
    {
        Collider[] coll = this.GetComponents<Collider>();
        foreach(var x in coll) {
            if (x.isTrigger) Trigger = x;
            else { Falling = x; }
        }

        Trigger.enabled = true;
        Falling.enabled = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("BaseBallBat")) {
            Debug.Log("I'm hit by a baseball bat!");
            Trigger.enabled = false;
            Falling.enabled = true;

            var r = this.GetComponent<Rigidbody>();
            r.isKinematic = false;
        }
    }
}
