using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IngameMenuManager : MonoBehaviour
{
    private static IngameMenuManager _current;

    public static IngameMenuManager current
    {
        get
        {
            if (_current == null)
            {
                _current = FindObjectOfType<IngameMenuManager>();
            }
            return _current;
        }
        set => _current = value;
    }

    private void Awake()
    {
        current = this;
    }

    [SerializeField] private GameObject _pauseMenuCanvas, _deathScreenCanvas, _winScreenCanvas;

    public void ShowDeathScreen() 
    {
        Time.timeScale = 0;
        _deathScreenCanvas.SetActive(true);
    }

    public void ShowWinScreen()
    {
        Time.timeScale = 0;
        _winScreenCanvas.SetActive(true);
    }

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
