using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableObject : MonoBehaviour {

    Collider TriggerCollider = null;
    Collider PhysicsCollider = null;
    void Start()
    {
        Collider[] colliders = GetComponents<Collider>();
        foreach (var x in colliders)
        {
            if (x.isTrigger)
            {
                TriggerCollider = x;
            }
            else
            {
                PhysicsCollider = x;
            }
        }

        PhysicsCollider.enabled = false;
        TriggerCollider.enabled = true;
    }
    public void Take(Transform parent)
    {
        this.transform.parent = parent;
        PhysicsCollider.enabled = false;
        TriggerCollider.enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().useGravity = false;
    }

    public void Drop()
    {
        this.transform.parent = null;
        PhysicsCollider.enabled = true;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().useGravity = true;
    }
}
