using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool gameOver = true;
    public GameObject player;

    private UiManager _uiManager;

    private void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UiManager>();
    }
    //if game over is true
    // if backspace key press
    // spawn player
    //game over is false
    //hide title screen

    private void Update()
    {
        if (gameOver == true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Instantiate(player, Vector3.zero , Quaternion.identity);
                gameOver = false;
                _uiManager.HideTitleScreen();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _uiManager.ShowTutorialScreen();
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
}
