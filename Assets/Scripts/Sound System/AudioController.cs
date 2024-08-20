using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    public static AudioController controller;
    [SerializeField] AudioMixerGroup masterMixerGroup;
    [SerializeField] AudioMixerGroup sfxMixerGroup;
    [SerializeField] AudioMixerGroup musicMixerGroup;
    Dictionary<GameObject, GameObject> spawnSFXLocks = new Dictionary<GameObject, GameObject>();

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

    public void PlaySFX(GameObject sfxPrefab, Vector3 pos)
    {
        if (sfxPrefab != null)
        {
            Instantiate(sfxPrefab, pos, Quaternion.identity);
        }
    }

    public void SetVolume(string name, float volume)
    {
        if (volume > 0)
        {
            masterMixerGroup.audioMixer.SetFloat(name, Mathf.Log10(volume) * 20);
        }
        else
        {
            masterMixerGroup.audioMixer.SetFloat(name, -80);
        }
    }
    public void SetMasterVolume(float volume)
    {
        SetVolume("MasterVolume", volume / 100);
    }

    public void SetSFXVolume(float volume)
    {
        SetVolume("SFXVolume", volume / 100);
    }

    public void SetMusicVolume(float volume)
    {
        SetVolume("MusicVolume", volume / 100);
    }

    public void PlaySFXWithLock(GameObject sfxPrefab, Vector3 pos)
    {
        if (sfxPrefab != null && (!spawnSFXLocks.ContainsKey(sfxPrefab) || spawnSFXLocks[sfxPrefab] == null))
        {
            var audioObj = Instantiate(sfxPrefab, pos, Quaternion.identity);
            if(!spawnSFXLocks.TryAdd(sfxPrefab, audioObj))
            {
                spawnSFXLocks[sfxPrefab] = audioObj;
            }
        }
    }
}
