using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivesManager : MonoBehaviour
{
    [SerializeField] int lives = 3;
    [SerializeField] float delayRespawnTime = 1f;
    [SerializeField] GameObject PlayerObject;
    [SerializeField] float spawnEyeFramesSeconds = 1.5f; 
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

        if(lives > 0)
        {
            StartCoroutine(DelaySpawn());
        }
        else
        {
            IngameMenuManager.current.ShowDeathScreen();
        }
    }

    IEnumerator DelaySpawn()
    {
        yield return new WaitForSeconds(delayRespawnTime);
        SpawnPlayer();
    }
}
