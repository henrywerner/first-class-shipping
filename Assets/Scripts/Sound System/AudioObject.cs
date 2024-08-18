using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioObject : MonoBehaviour
{
    [SerializeField] AudioClip[] clips; //Only put one clip in to play the same clip each time
    AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        PlayAudio();
    }

    private void PlayAudio()
    {
        // Can expand this to do more indepth stuff, but works for now. Like pitch varience, loop x times, etc.
        // Likely need an overall volume slider, make sure there is a channel these go through that can be adjusted.
        _audioSource.clip = clips[Random.Range(0, clips.Length - 1)];
        _audioSource.Play();
        Destroy(gameObject, _audioSource.clip.length);
    }
}
