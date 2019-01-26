using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float ShakeDuration = 0.5f;
    public CameraFollow Cam = null;
    // Start is called before the first frame update
    void Start()
    {
        currentTime = 0.0f;
        if (Cam == null) Cam = FindObjectOfType<CameraFollow>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.X))
        {
            Hit(1.0f);
        }
        currentTime += Time.deltaTime;
    
        while ((Shakes.Count > 0) && ((currentTime - Shakes[0].time) > ShakeDuration))
        {
            Shakes.RemoveAt(0);
        }

        float shakeAmount = 0.0f;
        foreach (var s in Shakes)
        {
            float shake = Mathf.Clamp01((currentTime-s.time) / ShakeDuration);
            shake = (1.0f - (shake * shake)) * s.strength;
            shakeAmount = Mathf.Max(shakeAmount, shake);

        }

        Cam.CameraShake = shakeAmount;
        
       
    }
    public void Hit(float s = 1.0f)
    {
        Shakes.Add(new Shake() { time = currentTime, strength = s });
    }

    struct Shake { public float time; public float strength; }
    List<Shake> Shakes = new List<Shake>();
    float currentTime = 0.0f;
      
}
