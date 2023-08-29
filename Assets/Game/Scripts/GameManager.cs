using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.EventSystems.EventTrigger;

public class GameManager : MonoBehaviour
{
    public bool gameOver = true;
    public GameObject player;
    public SpanwManager spawnManager;
    public EnemyAI enemyAi;
    private UiManager _uiManager;

    private void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UiManager>();
        _uiManager.actualScreen = "Title";
        _uiManager.phase = 1;
        enemyAi._speed = 2.35f;
        enemyAi.health = 1;
        spawnManager.timeToSpawnEnemys = 2.5f;
        _uiManager.UpdatePhaseText(_uiManager.phase);
    }

    private void Update()
    {
        if (gameOver == true)
        {
            if (Input.GetKeyDown(KeyCode.Space) && _uiManager.actualScreen == "Title")
            {
                Instantiate(player, Vector3.zero , Quaternion.identity);
                gameOver = false;
                _uiManager.HideTitleScreen();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _uiManager.ManageBetweenTitleAndOtherScreens();
            } 

            if (Input.GetKeyDown(KeyCode.Q))
            {
                _uiManager.HideTutorialScreen();
            }

        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }


    }

    public void StopGame() 
    {
        gameOver = true;
        spawnManager.StopSpawnRoutine();
        DestroyEnemysAndPowerUpsObjects();
    }

    protected void DestroyEnemysAndPowerUpsObjects()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies != null)
        {
            foreach (GameObject enemy in enemies)
            {
                Destroy(enemy);
            }
        }
    }

    public void IncreasePhase()
    {
        spawnManager.bossFight = false;
        _uiManager.phase++;
        _uiManager.ShowPhaseChangeScreen(_uiManager.phase);
        _uiManager.UpdatePhaseText(_uiManager.phase);
        IncreaseDificult(_uiManager.phase);
    }

    public void InitiateBossFight()
    {
        // achar uma musica mais epica scifi para a boss fight
        // ao chamar a instacia do boss mostrar o slider do lado da tela, apenas na boss fight
        //melhorar o slider que esta horrivel
        DestroyEnemysAndPowerUpsObjects();
        spawnManager.bossFight = true;
        spawnManager.InstantiateBoss();
    }

    private void IncreaseDificult(int phase)
    {
        switch (phase)
        {
            case 2:
                spawnManager.timeToSpawnEnemys -= 0.5f;
                break;
            case 3:
                enemyAi.health++;
                spawnManager.timeToSpawnEnemys -= 0.5f;
                break;
            case 4:
                enemyAi._speed++;
                enemyAi.health++;
                spawnManager.timeToSpawnEnemys -= 0.5f;
                break;
            default:
                break;
        }
    ;
    }
    
}
