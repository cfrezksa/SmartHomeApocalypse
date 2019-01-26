using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController : MonoBehaviour
{

    Animator anim = null;
    public float turnSpeed = 1.0f;
    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {



        float walk = Input.GetAxis("Vertical");
        if (Picking == true) walk = 0.0f
                ;
        //Debug.Log("walk = " + walk);
        anim.SetFloat("walk", walk);

        if (walk != 0.0f)
        {
            float turn = Input.GetAxis("Horizontal");
            //Debug.Log("turn = " + turn);
            this.transform.Rotate(Vector3.up, turn * turnSpeed * Time.deltaTime);
        }

        if (CanGrab() && Input.GetKeyDown(KeyCode.E))
        {
            anim.SetTrigger("Pickup");
        }

    }

    bool CanGrab() {
        return GrabObject != null;
    }

    void DisableControl()
    {
        int upperBodyId = anim.GetLayerIndex("UpperBody");
        anim.SetLayerWeight(upperBodyId, 0.0f);
        Debug.Log("Disable Control");
    }

    void EnableControl()
    {
        Debug.Log("Enable Control");
        int upperBodyId = anim.GetLayerIndex("UpperBody");
        anim.SetLayerWeight(upperBodyId, 1.0f);
    }

    private void OnAnimatorIK(int layerIndex) {

        Transform rh = anim.GetBoneTransform(HumanBodyBones.RightHand);
        Debug.DrawLine(rh.position, rh.position + rh.forward, Color.red);
        Debug.DrawLine(rh.position, rh.position + rh.right, Color.green);
        Debug.DrawLine(rh.position, rh.position + rh.up, Color.blue);
        if (Picking) {
            picktime += Time.deltaTime;
            float weight = Mathf.Clamp(Mathf.Abs(picktime - Grabtime) / Grabtime, 0.0f, 1.0f);
            if (picktime > Grabtime) {
                Picking = false;
                weight = 0.0f;
                Transform rightHand = anim.GetBoneTransform(HumanBodyBones.RightHand);
                Vector3 offset = rightHand.forward * rightHandOffset[0];
                offset += rightHand.right * rightHandOffset[1];
                offset += rightHand.up * rightHandOffset[2];
                GrabObject.transform.position = rightHand.position + offset;
                GrabObject.transform.parent = rightHand;
                GrabObject.GetComponent<Collider>().enabled = false;
                PickedObject = GrabObject;
                GrabObject = null;
            }
            anim.SetIKPosition(AvatarIKGoal.RightHand, GrabObject.transform.position);
            anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f - weight);

        } else {
            picktime = 0.0f;
        }
    }
    public Vector3 rightHandOffset;
    public float Grabtime = 1.0f;
    bool Picking = false;
    float picktime = 0.0f;
    void StartPick()
    {
        picktime = 0.0f;
        Picking = true;
        Debug.Log("StartPick");
    }
    public void AddGrabbable(GrabbableObject other) {
        Debug.Log("Grabbable is near");
        GrabObject = other;
    }

    public void RemoveGrabbable() {
        Debug.Log("Grabbable is away");
        GrabObject = null;
    }

    public void OnTriggerEnter(Collider other) {
        GrabbableObject grab = other.GetComponent<GrabbableObject>();
        if (grab != null) AddGrabbable(grab);
    }
    public void OnTriggerExit(Collider other) {
        GrabbableObject grab = other.GetComponent<GrabbableObject>();
        if (GrabObject == grab) {
            RemoveGrabbable();
        }
    }

    public GrabbableObject GrabObject = null;
    public GrabbableObject PickedObject = null;
}
