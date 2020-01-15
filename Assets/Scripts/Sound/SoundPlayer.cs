using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public AudioClip[] jumpAudioClips;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {}

    public void playJumpSfx()
    {
        audioSource.clip = jumpAudioClips[Random.Range(0, 2)];
        audioSource.Play();
    }

    public void playWalkOnGrassSfx()
    {
        audioSource.clip = jumpAudioClips[Random.Range(2, 6)];
        audioSource.Play();
    }
}
