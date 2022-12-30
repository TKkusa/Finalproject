using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMcontroller : MonoBehaviour
{
    public AudioSource bgm;
    
    void Start()
    {
        bgm = GetComponent<AudioSource>();
        bgm.volume = 0.05f;
        bgm.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if(bgm.isPlaying == true)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                bgm.Pause();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                bgm.Play();
            }
        }
    }
}
