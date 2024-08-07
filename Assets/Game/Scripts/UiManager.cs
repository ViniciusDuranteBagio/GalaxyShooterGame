﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public Sprite[] _life;
    public Image livesImageDisplay;
    public Text scoreText;
    public Text phaseText;
    public Text phaseChangeText;
    public Text cristalText;
    public GameObject titleScreen;
    public GameObject BossFightPanel;
    public GameObject tutorialScreen;
    public GameObject deadScreen;
    public GameObject winScreen;
    public GameObject changePhaseScreen;
    public GameObject dangerScreen;
    public GameObject shopScreen;
    public GameManager gameManager;
    public List<GameObject> Items;
    public Slider slider;
    private string ActualScreen;

    public int phase;
    public float score;


    public string actualScreen
    {
        get { return ActualScreen; }
        set { ActualScreen = value; }
    }

    public void UpdateLives(int currentLives)
    {
        currentLives = currentLives < 0 ? 0 : currentLives;

        livesImageDisplay.sprite = _life[currentLives];
    }

 
    public void OnScoreUpdate(float score)
    {
        UpdateScoreText(score);
    }

    private void UpdateScoreText(float score)
    {
        scoreText.text = "Score: " + score;
    }

 

    public void KilledTheBoss()
    {
        
        HideBossFightPannel();
        gameManager.StopSpawnRoutine();
        gameManager.DestroyEnemiesAndPowerUpsObjects();

        if (phase == 4)
        {
            winScreen.SetActive(true);
            return;
        }
        
        int cristals = gameManager.addCristalToPlayer();
        OpenShop(cristals);
    }

    public void ExitShop()
    {
        CloseShop();
        phase++;
        ShowPhaseChangeScreen(phase);
        gameManager.IncreasePhase();
        UpdatePhaseText(phase);
    }

    public void UpdatePhaseText(int phase)
    {
        //passar para evento
        phaseText.text = "Fase : " + phase;
    }
    public void ShowBossFightPannel()
    {
        BossFightPanel.SetActive(true);

    }
    public void HideBossFightPannel()
    {
        BossFightPanel.SetActive(false);
    }

    public void ShowTitleScreen ()
    {
        titleScreen.SetActive(true);
    }

    public void HideTitleScreen()
    {
        titleScreen.SetActive(false);
        scoreText.text = "Score: ";
    }

    public void ManageBetweenTitleAndOtherScreens()
    {
        if (ActualScreen == "Title") {
            ShowTutorialScreen();
        } else if (ActualScreen == "Tutorial") {
            HideTutorialScreen();
        } else if (ActualScreen == "Dead") {
            HideDeadScreenAndShowTitleScreen();
        }
    }


    public void ShowTutorialScreen()
    {
        titleScreen.SetActive(false);
        tutorialScreen.SetActive(true);
        ActualScreen = "Tutorial";
    }

    public void HideTutorialScreen()
    {
        tutorialScreen.SetActive(false);
        titleScreen.SetActive(true);
        ActualScreen = "Title";
    }

    public void ShowDeadScreen()
    {
        deadScreen.SetActive(true);
        StartCoroutine(FadeInDeadScreen());
        ActualScreen = "Dead";
    }

    public void HideDeadScreenAndShowTitleScreen()
    {
        deadScreen.SetActive(false);
        titleScreen.SetActive(true);
        ActualScreen = "Title";
    }

    public void ShowPhaseChangeScreen(int phase)
    {
        changePhaseScreen.SetActive(true);
        string textPhaseChange = GenerateTextPhase(phase);
        phaseChangeText.text = textPhaseChange;
        StartCoroutine(FadeOutPhaseScreen());
    }

    public string GenerateTextPhase(int phase) 
    {
        switch (phase)
        {
            case 2:
                return "Fase 2 Iniciada\r\nMais inimigos irão nascer\r\nBoa Sorte";
            case 3:
                return "Fase 3 Iniciada\r\nMais inimigos irão nascer\r\nEles terão mais vida\r\nBoa Sorte";
            case 4:
                return "Fase 4 Iniciada\r\nMais inimigos irão nascer\r\nEles terão mais vida\r\nEles serão mais rápidos\r\nBoa Sorte";
            default:
                return "";
        }
    }

    public void UpdateHealthSlider(float health)
    {
        slider.value = health;
    }

    IEnumerator FadeInDeadScreen()
    {
        CanvasGroup canvasGroup = deadScreen.GetComponent<CanvasGroup>();
        while (canvasGroup.alpha < 1) 
        {
            canvasGroup.alpha += Time.deltaTime / 2;
            yield return null;
        }
        yield return null;
    }

    IEnumerator FadeOutPhaseScreen()
    {
        //passar o canvas group para variavel global do script
        CanvasGroup canvasGroup = deadScreen.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 1;
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime / 2;
           
            yield return null;
        }
        if (canvasGroup.alpha == 0)
        {
            changePhaseScreen.SetActive(false);
        }
        yield return null;
    }

    public void SetDangerScreenActive()
    {
        dangerScreen.SetActive(true);
    }

    public void SetDangerScreenUnactive()
    {
        dangerScreen.SetActive(false);
    }

    public bool IsDangerScreenActive()
    {
        return dangerScreen.activeInHierarchy;
    }

    public void OpenShop(int cristals) 
    {
        shopScreen.SetActive(true);
        cristalText.text = "Player Cristals: " + cristals;
    }

    public void PurchaseItem(string Item) 
    { 
        GameObject item = GameObject.FindWithTag(Item);
        gameManager.AddItemToPlayer(Item);
        Player playerScript = GameObject.FindWithTag("Player").GetComponent<Player>();
        playerScript.SubCristal();
        Destroy(item);
    }

    public void CloseShop()
    {
        shopScreen.SetActive(false);
    }

}
