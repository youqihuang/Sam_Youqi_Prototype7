using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public int totalLives = 5; // Total number of lives
    public int currentLives = 5; // Current lives remaining
    public GameObject[] lifeImages; // Array to hold references to the life Images
    public GameObject gameOverOverlay; // Reference to the gray overlay UI
    public GameObject winScreen; // Reference to the win screen UI
    public CameraShake cameraShake;
    public SoundManager soundManager;

    private bool isGameOver = false; // Track if the game is already over

    void Start()
    {
        // Initialize current lives to max at the start
        currentLives = totalLives;
        gameOverOverlay.SetActive(false); // Ensure the overlay is hidden initially
        winScreen.SetActive(false); // Ensure the win screen is hidden initially
        UpdateHealthBar();
        
        StartCoroutine(CheckForWinCondition()); // Start the win condition countdown
    }

    // Coroutine to check win condition after 60 seconds
    IEnumerator CheckForWinCondition()
    {
        yield return new WaitForSeconds(60f); // Wait for 60 seconds
        
        if (!isGameOver) // If the player is still alive
        {
            WinGame();
        }
    }

    // Function to update the health bar based on remaining lives
    public void UpdateHealthBar()
    {
        for (int i = 0; i < lifeImages.Length; i++)
        {
            if (i < currentLives)
            {
                // If the life index is less than the current lives, show full life
                lifeImages[i].SetActive(true);
            }
            else
            {
                // Otherwise, show empty life
                lifeImages[i].SetActive(false);
            }
        }
    }

    // Function to reduce lives (called when the player takes damage)
    public void LoseLife()
    {
        if (currentLives > 0)
        {
            currentLives--;
            UpdateHealthBar();

            if (currentLives == 0)
            {
                GameOver();
            }
        }
    }

    // Function to gain lives (called when the player gains health)
    public void GainLife()
    {
        if (currentLives < totalLives)
        {
            currentLives++;
            UpdateHealthBar();
        }
    }

    // Function to handle game over
    void GameOver()
    {
        if (!isGameOver) // Ensure game over logic is only triggered once
        {
            isGameOver = true;
            Debug.Log("Game Over");
            soundManager.PlayLoseSound();
            cameraShake.StopShake();
            gameOverOverlay.SetActive(true); // Show the gray overlay
            Time.timeScale = 0; // Stop the game
        }
    }

    // Function to handle player win
    void WinGame()
    {
        Debug.Log("You Win!");
        soundManager.PlayWinSound();
        winScreen.SetActive(true); // Show the win screen
        Time.timeScale = 0; // Stop the game
    }
}
