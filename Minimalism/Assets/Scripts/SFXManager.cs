using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager sm;

    public AudioSource a;

    public AudioClip block;
    public AudioClip slice;
    public AudioClip damage;

    // Start is called before the first frame update
    void Start()
    {
        sm = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayClip(AudioClip clip)
    {
        a.PlayOneShot(clip);
    }
}
