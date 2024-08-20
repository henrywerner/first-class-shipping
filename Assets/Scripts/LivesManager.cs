using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivesManager : MonoBehaviour
{
    [SerializeField] public int lives = 3;
    [SerializeField] float delayRespawnTime = 1f;
    [SerializeField] GameObject PlayerObject;
    [SerializeField] float spawnEyeFramesSeconds = 1.5f;
    [SerializeField] GameObject[] lifeIcons;

    public static LivesManager manager;

    GameObject _currentPlayer;

    private void Awake()
    {
        if (manager == null)
        {
            manager = this;
        }
        else if (manager != this)
        {
            Destroy(gameObject);
        }

        SpawnPlayer();
    }

    private void Start()
    {
        updateLivesHUD();
    }

    private void SpawnPlayer()
    {
        _currentPlayer = Instantiate(PlayerObject, PlayerObject.transform.position, PlayerObject.transform.rotation);
        var hs = _currentPlayer.GetComponent<HealthSystem>();
        hs.WasKilled += PlayerKilled;
        hs.SetTempInvul(spawnEyeFramesSeconds);
    }

    private void PlayerKilled()
    {
        lives--;
        updateLivesHUD();

        if(lives > 0)
        {
            StartCoroutine(DelaySpawn());
        }
        else
        {
            IngameMenuManager.current.ShowDeathScreen();
        }
    }

    private void updateLivesHUD()
    {
        for (int i = 0; i < lifeIcons.Length; i++)
        {
            lifeIcons[i].SetActive(lives > i);
        }
    }

    IEnumerator DelaySpawn()
    {
        yield return new WaitForSeconds(delayRespawnTime);
        SpawnPlayer();
    }
}
