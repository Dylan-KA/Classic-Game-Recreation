using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioControl : MonoBehaviour
{
    public AudioSource backgroundMusic;
    public AudioSource introMusic;

    // Start is called before the first frame update
    void Start()
    {
        introMusic.Play();
        backgroundMusic.PlayDelayed(introMusic.clip.length);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
