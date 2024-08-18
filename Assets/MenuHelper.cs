using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuHelper : MonoBehaviour
{
    private static MenuHelper _current;

    public static MenuHelper current
    {
        get
        {
            if (_current == null)
            {
                _current = FindObjectOfType<MenuHelper>();
            }
            return _current;
        }
        set => _current = value;
    }
    
    [SerializeField] private GameObject _buttonPrefab;
    [SerializeField] private GameObject _creditsCanvas;

    private int currentButton;

    void Start()
    {
        currentButton = 1;
        InvokeRepeating("SpawnNextButton", 0f, 4f);
    }

    void Update()
    {
        if (_creditsCanvas.activeInHierarchy) {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                CloseCredits();
            }
            else if (Input.GetAxis("Cancel") > 0) {
                CloseCredits();
            }
        }
    }

    void SpawnNextButton() {
        float xLoc = 15f;
        float yLoc = UnityEngine.Random.Range(-3, 3);
        float zLoc = 0f;
        
        GameObject newButton = Instantiate(_buttonPrefab, new Vector3(xLoc, yLoc, zLoc), Quaternion.identity);
        
        switch (currentButton)
        {
            case 1:
                newButton.GetComponent<MenuButton>().type = MenuButton.buttonType.Start;
                break;
            case 2:
                newButton.GetComponent<MenuButton>().type = MenuButton.buttonType.Options;
                break;
            case 3:
                newButton.GetComponent<MenuButton>().type = MenuButton.buttonType.Credits;
                break;
            case 4:
                newButton.GetComponent<MenuButton>().type = MenuButton.buttonType.Quit;
                break;
            default:
                break;
        }

        Rigidbody2D rb = newButton.GetComponent<Rigidbody2D>();
        rb.AddTorque(UnityEngine.Random.Range(-0.2f, 0.2f), ForceMode2D.Impulse);
        // rb.AddForce(new Vector2(UnityEngine.Random.Range(-30, -20), 0));

        currentButton++;
        if (currentButton > 4) {
            currentButton = 1;
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("TestLevel_01");
    }

    public void OpenOptions()
    {
        // TODO: Add options
    }

    public void OpenCredits()
    {
        _creditsCanvas.SetActive(true);
    }

    public void CloseCredits()
    {
        _creditsCanvas.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
