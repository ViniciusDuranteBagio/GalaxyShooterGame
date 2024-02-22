using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public float score;
    public int phase;
    public delegate void UpdateScoreHandler(float score);
    public static event UpdateScoreHandler ScoreUpdated;
    public delegate void UpdateLivesHandler(int life);
    public static event UpdateLivesHandler LivesUpdated;
    
    public delegate void ShowDeadScreenHandler();
    public static event ShowDeadScreenHandler ShowedDeadScreen;

    private void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UiManager>();
        _uiManager.actualScreen = "Title";
        phase = 1;
        score = 0;
        enemyAi._speed = 2.35f;
        enemyAi.health = 1;
        spawnManager.timeToSpawnEnemys = 2.5f;
        _uiManager.UpdatePhaseText(phase);
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
        StopSpawnRoutine();
        DestroyEnemiesAndPowerUpsObjects();
    }

    public void StopSpawnRoutine()
    {
        spawnManager.StopSpawnRoutine();
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

    public void DestroyEnemiesAndPowerUpsObjects()
    {
        DestroyEnemies();
        DestroyPowerUps();
    }

    private void DestroyEnemies()
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
    
    private void DestroyPowerUps()
    {
        GameObject[] powerUps = GameObject.FindGameObjectsWithTag("PowerUps");
        if (powerUps != null)
        {
            foreach (GameObject powerUp in powerUps)
            {
                Destroy(powerUp);
            }
        }
    }

    public void IncreasePhase()
    {
        spawnManager.bossFight = false;
        spawnManager.StartSpawnRoutine();
        IncreaseDificult(_uiManager.phase);
    }

    public void InitiateBossFight()
    {
        // achar uma musica mais epica scifi para a boss fight
        // ao chamar a instacia do boss mostrar o slider do lado da tela, apenas na boss fight
        //melhorar o slider que esta horrivel
        DestroyEnemies();
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
    
    public void OnEnemyDeath()
    {
        score += 250;
        UpdateScore(score);
    }
    
    public void OnPlayerDamaged(int life)
    {
        if (LivesUpdated != null)
        {
            LivesUpdated(life);
        }

        if (score >= 100)
        {
            score -= 100;
        }
        UpdateScore(score);
    }
    
    public void UpdateScore(float updatedScore)
    {
        if (ScoreUpdated != null)
        {
            ScoreUpdated(updatedScore);
        }
        if (IsScoreEnableToIncreasePhase(updatedScore, phase)) 
        {
            InitiateBossFight();
        }
    }
    
    private bool IsScoreEnableToIncreasePhase(float score, int actualPhase)
    {
        int allowedScoreToNewPhase = actualPhase * 1000;
        if ( score >= allowedScoreToNewPhase)
        {
            return true;
        }
        return false;
    }
    
    public void OnPlayerDeath()
    {
        StopGame();
        //passar para envent
        if (ShowedDeadScreen != null)
        {
            ShowedDeadScreen();
        }
    }
}
