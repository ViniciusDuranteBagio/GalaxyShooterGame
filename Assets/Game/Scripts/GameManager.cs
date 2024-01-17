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

    public void StartGame()
    {
        Instantiate(player, Vector3.zero, Quaternion.identity);
        gameOver = false;
        _uiManager.HideTitleScreen();
    }

    public void StopGame()
    {
        gameOver = true;
        spawnManager.StopSpawnRoutine();
        DestroyEnemysAndPowerUpsObjects();
    }

    public void GoToTitleFromTutorialScreen() 
    {
        _uiManager.HideTutorialScreen();
    }
    public void GoToTutorialFromTitleScreen()
    {
        _uiManager.ShowTutorialScreen();
    }

    public void GoToTitleFromLoseScreen()
    {
        _uiManager.HideDeadScreenAndShowTitleScreen();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
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
        spawnManager.StartEnemySpawnRoutine();
        IncreaseDificult(_uiManager.phase);
    }

    public void InitiateBossFight()
    {
        // achar uma musica mais epica scifi para a boss fight
        // ao chamar a instacia do boss mostrar o slider do lado da tela, apenas na boss fight
        //melhorar o slider que esta horrivel
        DestroyEnemysAndPowerUpsObjects();
        _uiManager.ShowBossFightPannel();
        spawnManager.bossFight = true;
        spawnManager.InstantiateBoss();
    }

    private void IncreaseDificult(int phase)
    {
        switch (phase)
        {
            case 2:
                DecreaseEnemyTimeToSpawn();
                break;
            case 3:
                IncreaseEnemyHealth();
                DecreaseEnemyTimeToSpawn();
                break;
            case 4:
                IncreaseEnemySpeed();
                IncreaseEnemyHealth();
                DecreaseEnemyTimeToSpawn();
                break;
            default:
                break;
        }
    }
    private void IncreaseEnemySpeed() 
    {
        enemyAi._speed++;
    }
    private void IncreaseEnemyHealth()
    {
        enemyAi.health++;
    }

    private void DecreaseEnemyTimeToSpawn() 
    {         
        spawnManager.timeToSpawnEnemys -= 0.5f;        
    }
    public int addCristalToPlayer()
    {
        Player playerScript = player.GetComponent<Player>();
        playerScript.addCristal();
        return playerScript.cristals;
    }

    public void PurchaseItem(string item)
    {
        Player playerScript = player.GetComponent<Player>();
        playerScript.SubCristal();
    }

    public void AddItemToPlayer(string item)
    {
        Player playerScript = player.GetComponent<Player>();
        switch (item)
        {
            case "Damage":
                playerScript.IncreaseDamage();
                break;
            case "Speed":
                playerScript.IncreaseSpeed();
                break;
            case "Attack Speed":
                playerScript.IncreaseAttackSpeed();
                break;
            default:
                break;
        }
    }

}
