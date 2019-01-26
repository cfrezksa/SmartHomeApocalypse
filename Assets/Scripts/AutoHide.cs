using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoHide : MonoBehaviour
{
    Renderer theRenderer = null;
    // Start is called before the first frame update
    void Start()
    {
        theRenderer = GetComponentInChildren<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        CameraFollow cam = FindObjectOfType<CameraFollow>();
        Vector3 camPos = cam.transform.position;

        CharController c = FindObjectOfType<CharController>();
        Vector3 p = c.transform.position;

        Vector3 dir = p-camPos;
        float maxDistance = dir.magnitude;
        dir.Normalize();

        //theRenderer.enabled = true;
        RaycastHit[] hits = Physics.RaycastAll(camPos, dir, maxDistance);
        bool isHit = false;
        foreach (var h in hits)
        {
            if (h.collider.gameObject == this.gameObject)
            {
                isHit = true;
            }
        }

        theRenderer.enabled = !isHit;
       
    }
}
