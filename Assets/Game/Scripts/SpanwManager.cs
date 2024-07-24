using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpanwManager : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyShipPrefab;
    [SerializeField]
    private GameObject Boss;
    [SerializeField]
    private GameObject[] powerups;
    [SerializeField]
    private GameObject asteroid;

    private GameManager _gameManager;
    private UiManager _uiManager;

    public bool bossFight = false;

    public float timeToSpawnEnemys;
    public float timeToSpawnPowerUp;

    void Start()
    {
        timeToSpawnPowerUp = 7.0f;
        _uiManager = GameObject.Find("Canvas").GetComponent<UiManager>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        StartSpawnRoutine();
    }

    public void StartSpawnRoutine()
    {
        StartCoroutine(EnemySpawnCorotine());
        StartCoroutine(PowerUpSpawnCorotine());
        StartCoroutine(AsteroidSpawnCorotine());
    }

    public void StopSpawnRoutine()
    {
        StopCoroutine(EnemySpawnCorotine());
        StopCoroutine(PowerUpSpawnCorotine());
        StopCoroutine(AsteroidSpawnCorotine());

    }
    public void StartEnemySpawnRoutine()
    {
        StartCoroutine(EnemySpawnCorotine());
    }
    public void StopEnemySpawnRoutine()
    {
        StopCoroutine(EnemySpawnCorotine());
    }

    public void InstantiateBoss()
    {
        Instantiate(Boss, transform.position + new Vector3(-0.03f, 8.53f, 0), Quaternion.identity);
        StopEnemySpawnRoutine();
    }

    // ReSharper restore Unity.ExpensiveCode
    IEnumerator EnemySpawnCorotine()
    {
        float randomX = Random.Range(-8.03f, 8.3f);
        Instantiate(enemyShipPrefab, transform.position + new Vector3(randomX, 6.16f, 0), Quaternion.identity);
        yield return new WaitForSeconds(timeToSpawnEnemys);
    }

    // ReSharper restore Unity.ExpensiveCode
    IEnumerator PowerUpSpawnCorotine()
    {
        // fazer object pooling
        //transformar essa validação em um evento para trocar o valor de timeToSpawnPowerUp
        if (_uiManager.phase >= 2)
        {
            timeToSpawnPowerUp = 4.0f;
        }
        int randomPowerup = Random.Range(0, 3);
        Instantiate(powerups[randomPowerup], new Vector3(Random.Range(-8.03f, 8.3f), 6.16f, 0), Quaternion.identity);
        yield return new WaitForSeconds(timeToSpawnPowerUp);
    }

    // ReSharper restore Unity.ExpensiveCode
    IEnumerator AsteroidSpawnCorotine()
    {
        // fazer object pooling
        Instantiate(asteroid, new Vector3(Random.Range(-8.03f, 8.3f), 6.16f, 0), Quaternion.identity);
        yield return new WaitForSeconds(10.0f);
    }
}
