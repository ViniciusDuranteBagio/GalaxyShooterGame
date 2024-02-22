using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    [SerializeField]
    private UiManager uiManager;
    [SerializeField]
    private GameManager gameManager;
    private void Start()
    {
        EnemyAI.EnemyDied += gameManager.OnEnemyDeath;
        GameManager.ScoreUpdated += uiManager.OnScoreUpdate;
        Player.DamagedPlayer += gameManager.OnPlayerDamaged;
        GameManager.LivesUpdated += uiManager.UpdateLives;
        Player.PlayerDied += gameManager.OnPlayerDeath;
        GameManager.ShowedDeadScreen += uiManager.ShowDeadScreen;
    }
}
