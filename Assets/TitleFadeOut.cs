using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TitleFadeOut : MonoBehaviour
{
    private TMP_Text _text;
    private Color targetColor = new Color(1, 1, 1, 0); 

    void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }

    void Start()
    {
        StartCoroutine(FadeOut(3.5f));
    }

    IEnumerator FadeOut(float duration) 
    {
        float time = 0;
        Color startValue = _text.color;

        while (time < duration)
        {
            _text.color = Color.Lerp(startValue, targetColor, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        _text.color = targetColor;
    }
}
