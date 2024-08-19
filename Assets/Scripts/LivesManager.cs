using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivesManager : MonoBehaviour
{
    [SerializeField] int lives = 3;
    [SerializeField] float delayRespawnTime = 1f;
    [SerializeField] GameObject PlayerObject;
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
        _currentPlayer.GetComponent<HealthSystem>().WasKilled += PlayerKilled;
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
            // Start fail state
        }
    }

    IEnumerator DelaySpawn()
    {
        yield return new WaitForSeconds(delayRespawnTime);
        SpawnPlayer();
    }
}
