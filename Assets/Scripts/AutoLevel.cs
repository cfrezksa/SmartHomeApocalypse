using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoLevel : MonoBehaviour
{
    public GameObject[] Objects;
    List<Bounds> B = new List<Bounds>();
    // Start is called before the first frame update
    void Start()
    {
        BoxCollider[] c = GetComponents<BoxCollider>();
        foreach (var x in c)
        {
        B.Add(x.bounds);
        }
    }

    // Update is called once per frame
    void Update()
    {
        CharController c = FindObjectOfType<CharController>();
        Vector3 pos = c.transform.position;

        bool isInside = false;
        foreach (var b in B)
        {
            if (b.Contains(pos))
            {
                isInside = true;
            }
        }

        foreach (var o in Objects)
        {
            o.SetActive(isInside);
        }
    }
}
