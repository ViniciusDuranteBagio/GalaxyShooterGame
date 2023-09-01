using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public Sprite[] _life;
    public Image livesImageDisplay;
    public float score;
    public Text scoreText;
    public Text phaseText;
    public Text phaseChangeText;
    public GameObject titleScreen;
    public GameObject BossFightPanel;
    public GameObject tutorialScreen;
    public GameObject deadScreen;
    public GameObject changePhaseScreen;
    public GameObject dangerScreen;
    public GameManager gameManager;
    public Slider slider;
    private string ActualScreen;

    public int phase;

    public string actualScreen
    {
        get { return ActualScreen; }
        set { ActualScreen = value; }
    }

    public void UpdateLives(int currentLives)
    {
        currentLives = currentLives < 0 ? 0 : currentLives;

        livesImageDisplay.sprite = _life[currentLives];
        if (score > 100)
        {
            UpdateScoreDamage();
        }
    }

    public void UpdateScore()
    {
        score += 50;

        if (IsScoreEnableToIncreasePhase(score)) 
        {
            gameManager.InitiateBossFight();
        }

        scoreText.text = "Score: " + score;
    }

    private bool IsScoreEnableToIncreasePhase(float score)
    {
        //fazer isso funcionar mesmo quando tomar dano e os numeros ficarem quebrados
        int[] array = { 1000, 2000, 3000 };

        if (!Array.Exists(array, element => element == score))
        {
            return false;
        }

        return true;
    }

    public void KilledTheBoss()
    {
        gameManager.IncreasePhase();
        UpdatePhaseText(phase);
    }

    public void UpdatePhaseText(int phase)
    {
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

    public void UpdateScoreDamage()
    {
        score = score - 100;
        scoreText.text = "Score: " + score;
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
            HideDeadScreen();
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

    public void HideDeadScreen()
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

    public void UpdateHealthSlider(int health)
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


}
