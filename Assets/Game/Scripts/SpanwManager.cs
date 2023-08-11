using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpanwManager : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyShipPrefab;
    [SerializeField]
    private GameObject[] powerups;

    private GameManager _gameManager;
    private UiManager _uiManager;

    public float timeToSpawnEnemys;

    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UiManager>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        StartSpawnRoutine();
    }
    public void StartSpawnRoutine()
    {
        StartCoroutine(EnemySpawnCorotine());
        StartCoroutine(PowerUpSpawnCorotine());
    }

    public void StopSpawnRoutine()
    {
        StopCoroutine(EnemySpawnCorotine());
        StopCoroutine(PowerUpSpawnCorotine());
    }

    IEnumerator EnemySpawnCorotine()
    {
        while (_gameManager.gameOver == false)
        {
                float randomX = Random.Range(-8.03f, 8.3f);
                Instantiate(enemyShipPrefab, transform.position + new Vector3(randomX, 6.16f, 0), Quaternion.identity);
                yield return new WaitForSeconds(timeToSpawnEnemys);
        }
    }

    IEnumerator PowerUpSpawnCorotine()
    {
        while (_gameManager.gameOver == false)
        {
            if (_uiManager.score < 500)
            {
                int randomPowerup = Random.Range(0, 3);
                Instantiate(powerups[randomPowerup], new Vector3(Random.Range(-8.03f, 8.3f), 6.16f, 0), Quaternion.identity);
                yield return new WaitForSeconds(7.0f);
            }

            else
            {
                int randomPowerup = Random.Range(0, 3);
                Instantiate(powerups[randomPowerup], new Vector3(Random.Range(-8.03f, 8.3f), 6.16f, 0), Quaternion.identity);
                yield return new WaitForSeconds(4.0f);
            }

        }
    }
}
