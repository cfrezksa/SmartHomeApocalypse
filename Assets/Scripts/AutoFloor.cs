using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoFloor : MonoBehaviour
{

    public GameObject[] Walls;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CharController c = FindObjectOfType<CharController>();

        Vector3 rayStart = c.transform.position + Vector3.up;
        //theRenderer.enabled = true;
        RaycastHit[] hits = Physics.RaycastAll(rayStart, Vector3.down, 5.0f);

        Debug.DrawLine(rayStart, rayStart + 5.0f * Vector3.down, Color.red);
        bool isHit = false;
        foreach (var h in hits)
        {
            if (h.collider.gameObject == this.gameObject)
            {
                isHit = true;
            }
        }
        
            foreach(var r in Walls) {
                r.SetActive(isHit);
            }

            Renderer rend = GetComponentInChildren<Renderer>();
            if (rend != null)
            {
                rend.enabled = isHit;
            }
    }


    
}
