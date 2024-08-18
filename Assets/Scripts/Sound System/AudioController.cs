using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    public static AudioController controller;

    private void Awake()
    {
        if(controller == null)
        {
            controller = this;
        }
        else if(controller != this)
        {
            Destroy(gameObject);
        }
    }

    public void SpawnSFX(GameObject sfxPrefab, Vector3 pos)
    {
        if (sfxPrefab != null)
        {
            Instantiate(sfxPrefab, pos, Quaternion.identity);
        }
    }

    public void SetVolume(AudioMixerGroup mixerGroup)
    {
        //TODO Implement
        
    }
}
