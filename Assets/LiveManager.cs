using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LiveManager : MonoBehaviour
{
    public int asunaLives = 5;
    public TMP_Text lifeText;          // Reference to the UI Text element for lives
    public GameObject gameOverUI;  // Reference to the Game Over UI panel
    public Button playAgain;
    public Button levels;
    private string sceneName;
    public AudioSource audioSource;    // AudioSource for playing sounds
    public AudioClip loseLifeSound; 

    void Start()
    {
        // Attempt to find the lifeText GameObject
        Scene currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;
        lifeText = GameObject.Find("LivesLeft")?.GetComponent<TMP_Text>();
        if (lifeText == null)
        {
            Debug.LogError("LifeText reference is missing!");
        }

        playAgain.onClick.AddListener(OnPlayAgain);
        levels.onClick.AddListener(LoadLevels);

        gameOverUI.SetActive(false);
        UpdateLifeText();  // Initialize the UI with Claire's lives
    }

    private void OnPlayAgain()
    {
        SceneManager.LoadScene(sceneName);
    }

    private void LoadLevels()
    {
        SceneManager.LoadScene("Levels");
    }

    public void ReduceLives()
    {
        if (asunaLives > 0) // Check if lives are greater than 0
        {
            asunaLives--;  // Reduce lives
            UpdateLifeText();  // Update UI

            // Play sound when losing a life
            if (audioSource != null && loseLifeSound != null)
            {
                audioSource.PlayOneShot(loseLifeSound);
            }
            else
            {
                Debug.LogWarning("AudioSource or loseLifeSound is missing!");
            }

            if (asunaLives == 0)
            {
                Debug.Log("No more lives left! Game Over!");
                ShowGameOverUI(); // Uncomment to show the Game Over screen
            }
        }
        else
        {
            Debug.Log("Asuna already has no more lives left!");
        }
    }


    private void UpdateLifeText()
    {
        lifeText.text = "Lives Left: " + asunaLives.ToString();
    }

    private void ShowGameOverUI()
    {
        gameOverUI.SetActive(true);
    }
}
