using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioSource normal;
    [SerializeField] private AudioSource scared;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("playNormal", 3.0f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void playNormal()
    {
        normal.Play();
        scared.Stop();
    }

    public void playScared()
    {
        scared.Play();
        normal.Pause();
        Invoke("unpauseNormal", 10.0f);
    }

    public void unpauseNormal()
    {
        normal.UnPause();
    }

    public void stopAllMusic()
    {
        normal.Stop();
        scared.Stop();
    }
}
