using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundOnEnter : MonoBehaviour
{

    public Animator anim;
    AudioSource source;
    public AudioClip clip;
    bool played = false;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        
    }
    float elapsedTime = 0.0f;
    void Update()
    {
        if (played)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > 10.0f)
            {
                played = false;
            } else {
                elapsedTime = 0.0f;
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (played == false)
        {
            played = true;
            source.PlayOneShot(clip);
            if (anim != null) anim.SetTrigger("Start");
        }
    }

}
