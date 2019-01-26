using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public GameObject FollowTarget;
    public Vector3 FollowOffset;
    public Vector3 LookAtOffset = Vector3.zero;
    public float CameraSmooth = 0.99f;
    public float CameraShake = 0.0f;
    public float HandHeld = 0.0f;
    // Use this for initialization
    void Awake () {

        if (null == FollowTarget) {
            var t = FindObjectOfType<CharController>();
            FollowTarget = t? t.gameObject : null;
        }
        Debug.Log("FollowTarget = " + FollowTarget);

       
        FollowOffset = this.transform.position - FollowTarget.transform.position;
        Debug.Log("FollowOffset = " + FollowOffset);

    }

    float elapsedTime = 0.0f;
	// Update is called once per frame
	void Update () {
        elapsedTime += Time.deltaTime;
        Vector3 TargetPosition = FollowTarget.transform.position;
        Vector3 CameraPosition = TargetPosition + FollowOffset;
        this.transform.position = CameraSmooth * this.transform.position + (1.0f- CameraSmooth) *CameraPosition;
        this.transform.LookAt(TargetPosition + LookAtOffset, Vector3.up);
		
        float ShakeFrequency =  (CameraShake>0.0f)? 5.0f : 1.0f;
        float ShakeAmount = HandHeld + CameraShake;
        float shakeYaw   = ShakeAmount * ShakeAmount * (2.0f * Mathf.PerlinNoise(0.0f, ShakeFrequency * ShakeFrequency * elapsedTime)-1.0f);
        float shakePitch = ShakeAmount * ShakeAmount * (2.0f * Mathf.PerlinNoise(1.0f, ShakeFrequency * ShakeFrequency *elapsedTime )-1.0f);
        float shakeRoll = CameraShake * ShakeAmount  * (2.0f * Mathf.PerlinNoise(2.0f, ShakeFrequency * ShakeFrequency *elapsedTime )-1.0f);
        this.transform.Rotate(new Vector3(shakeYaw, shakePitch, shakeRoll));
    }

}
