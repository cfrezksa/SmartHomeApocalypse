using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController : MonoBehaviour {

    Animator anim = null;
    public float turnSpeed = 1.0f;
    // Use this for initialization
    void Start() {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        Vector3 moveDir = new Vector3(
            Input.GetAxis("Horizontal"),
            0.0f,
            Input.GetAxis("Vertical")
            );
        float walk = moveDir.magnitude;
        anim.SetFloat("walk", walk);

        moveDir.Normalize();
        float turn = Mathf.Acos(Vector3.Dot(this.transform.forward, moveDir));
        if (Vector3.Cross(this.transform.forward, moveDir)[2] < 0.0) {
            turn *= -1.0f;
        }

        Debug.DrawLine(this.transform.position, this.transform.position + moveDir, Color.yellow);

        if (walk > 0.1f) {
            Quaternion targetRot = Quaternion.LookRotation(moveDir, Vector3.up);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRot, 0.1f);
            anim.SetFloat("turn", 0.0f);
        }

        if (Input.GetAxis("Fire1") > 0.0) {
            if (CanGrab()) {
                Picking = true;
                anim.SetTrigger("Pickup");
            } else if (CanDrop()) {
                DropObject();
            } else {
                if (walk > 0.1f) {
                    anim.SetTrigger("Beat1Walk");
                } else {
                    anim.SetTrigger("Beat1");
                }
            }
        }
    }

    bool CanGrab() {
        return (PickedObject== null) && (Picking == false) && (GrabObject != null);
    }

    bool CanDrop() {
        return (PickedObject != null);
    }


    void DisableControl() {
        int upperBodyId = anim.GetLayerIndex("UpperBody");
        anim.SetLayerWeight(upperBodyId, 0.0f);
        Debug.Log("Disable Control");
    }

    void EnableControl() {
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

            if (GrabObject != null) {
                GrabTargetPos = GrabObject.transform.position;
            }

            picktime += Time.deltaTime;
            float weight = Mathf.Clamp(Mathf.Abs(picktime - Grabtime) / Grabtime, 0.0f, 1.0f);

            if (picktime > Grabtime) {
                if (GrabObject != null) {
                    TakeObject();
                }
            }

            if (picktime > 2.0f * Grabtime) {
                Picking = false;
                weight = 0.0f;
                anim.SetIKPosition(AvatarIKGoal.RightHand, GrabTargetPos);
                anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f - weight);
            } else {
                anim.SetIKPosition(AvatarIKGoal.RightHand, GrabTargetPos);
                anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f - weight);
            }

        }
    }


    private void TakeObject() {
        Transform rightHand = anim.GetBoneTransform(HumanBodyBones.RightHand);
        Vector3 offset = rightHand.forward * RightPalmOffset[0];
        offset += rightHand.right * RightPalmOffset[1];
        offset += rightHand.up * RightPalmOffset[2];

        GrabObject.transform.position = rightHand.position + offset;
        GrabObject.Take(rightHand);
        PickedObject = GrabObject;
        GrabObject = null;
    }

    private void DropObject() {
        Debug.Log("Drop");
        PickedObject.Drop();

    }

    void StartPick() {
        picktime = 0.0f;
        Picking = true;
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

    public Vector3 GrabTargetPos;
    public Vector3 RightPalmOffset;
    public float Grabtime = 1.0f;
    bool Picking = false;
    float picktime = 0.0f;
}
