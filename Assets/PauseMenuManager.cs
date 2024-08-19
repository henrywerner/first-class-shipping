using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    private static PauseMenuManager _current;

    public static PauseMenuManager current
    {
        get
        {
            if (_current == null)
            {
                _current = FindObjectOfType<PauseMenuManager>();
            }
            return _current;
        }
        set => _current = value;
    }

    private void Awake()
    {
        current = this;
    }

    [SerializeField] private GameObject _pauseMenuCanvas;

    public void PauseGame()
    {
        Time.timeScale = 0;
        _pauseMenuCanvas.SetActive(true);
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1;
        _pauseMenuCanvas.SetActive(false);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("TestLevel_01");
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Main Menu");
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (!_pauseMenuCanvas.activeInHierarchy) {
                PauseGame();
            } else {
                UnpauseGame();
            }
        }
    }
}
