using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text timerText;                // Reference to UI Text for timer
    public GameObject heart1, heart2, heart3; // Heart objects representing player lives
    public GameObject trophyImage, trashImage; // Win and Lose images
    public GameObject playAgainButton;    // Play Again button

    private float timer = 60f;            // Game countdown timer
    private int playerLives = 3;          // Number of player lives

    void Start()
    {
        ResetUI();
    }

    void Update()
    {
        // Update the timer if it's still running
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            timerText.text = "Time: " + Mathf.Ceil(timer).ToString();
        }
        else if (playerLives > 0)
        {
            // Player wins if time runs out
            WinGame();
        }
    }

    public void DeductLife()
    {
        playerLives--;
        UpdateHearts();
        if (playerLives <= 0)
        {
            LoseGame();
        }
    }

    private void UpdateHearts()
    {
        heart3.SetActive(playerLives >= 3);
        heart2.SetActive(playerLives >= 2);
        heart1.SetActive(playerLives >= 1);
    }

    private void WinGame()
    {
        trophyImage.SetActive(true);
        playAgainButton.SetActive(true);
    }

    private void LoseGame()
    {
        trashImage.SetActive(true);
        playAgainButton.SetActive(true);
    }

    public void PlayAgain()
    {
        // Reset game state
        playerLives = 3;
        timer = 60f;
        ResetUI();
    }

    private void ResetUI()
    {
        // Reset all UI elements
        UpdateHearts();
        trophyImage.SetActive(false);
        trashImage.SetActive(false);
        playAgainButton.SetActive(false);
    }
}
