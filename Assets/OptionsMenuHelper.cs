using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OptionsMenuHelper : MonoBehaviour
{
    [SerializeField] private TMP_Text masterText, effectsText, musicText;

    public void SetMasterText(float value)
    {
        masterText.text = value + "%";
    }

    public void SetEffectsText(float value)
    {
        effectsText.text = value + "%";
    }

    public void SetMusicText(float value)
    {
        musicText.text = value + "%";
    }
}
