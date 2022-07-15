using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public Sprite[] _life;
    public Image livesImageDisplay;
    public float score;
    public Text scoreText;
    public GameObject titleScreen;
    public void UpdateLives(int currentLives)
    {
        Debug.Log("Player Lives:  " + currentLives);
        livesImageDisplay.sprite = _life[currentLives];
        if (score > 100)
        {
            UpdateScoreDamage();
        }
    }

    public void UpdateScore()
    {
        score += 20;

        scoreText.text = "Score: " + score;

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
   
}
