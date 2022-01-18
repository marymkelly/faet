using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundOnMove : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip clip;
    public AudioSource source;
    void Start()
    {
        source = gameObject.AddComponent<AudioSource>();
        source.clip = clip;
    }

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");

        if (moveX > 0) {
            if (!source.isPlaying) {
                source.loop = true;
                source.Play();
            }
        } else {
            source.Stop();
        }
    }
}
