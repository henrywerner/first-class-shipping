using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButton : MonoBehaviour, IPointerDownHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public enum buttonType
    {
        Start,
        Options,
        Credits,
        Quit
    };
    public buttonType type;

    private SpriteRenderer _spriteRend;

    void Start()
    {
        _spriteRend = GetComponent<SpriteRenderer>();

        switch (type)
        {
            // We're assuming that all of the button text images will be disabled on start
            case buttonType.Start:
                transform.GetChild(0).gameObject.SetActive(true);
                break;
            case buttonType.Options:
                transform.GetChild(1).gameObject.SetActive(true);
                break;
            case buttonType.Credits:
                transform.GetChild(2).gameObject.SetActive(true);
                break;
            case buttonType.Quit:
                transform.GetChild(3).gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }

    void Update()
    {
        if (transform.position.x < -25 || transform.position.x > 25) {
            Destroy(gameObject);
        }
        if (transform.position.y < -10 || transform.position.y > 10) {
            Destroy(gameObject);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked");
        _spriteRend.color = new Color32(89, 122, 255, 255);

        switch (type)
        {
            case buttonType.Start:
                MenuHelper.current.StartGame();
                break;
            case buttonType.Options:
                MenuHelper.current.OpenOptions();
                break;
            case buttonType.Credits:
                MenuHelper.current.OpenCredits();
                break;
            case buttonType.Quit:
                MenuHelper.current.QuitGame();
                break;
            default:
                break;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // throw new System.NotImplementedException();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _spriteRend.color = new Color32(153, 173, 255, 255);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _spriteRend.color = new Color(1f, 1f, 1f, 1f);
    }
}
