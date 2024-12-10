using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesManager : MonoBehaviour
{
    public int claireLives = 5;   
    public Text lifeText;          // Reference to the UI Text element for lives
    public GameObject gameOverUI;  // Reference to the Game Over UI panel

    void Start()
    {
        // Attempt to find the lifeText GameObject
        lifeText = GameObject.Find("LivesLeft")?.GetComponent<Text>();
        if (lifeText == null)
        {
            Debug.LogError("LifeText reference is missing!");
        }

        gameOverUI.SetActive(false); 
        UpdateLifeText();  // Initialize the UI with Claire's lives
    }

    public void ReduceLives()
    {
        claireLives--;  // Reduce lives
        UpdateLifeText();  // Update UI

        if (claireLives <= 0)
        {
            Debug.Log("Game Over: Claire is out of lives!");
            ShowGameOverUI(); // Show the Game Over UI
        }
    }

    private void UpdateLifeText()
    {
        lifeText.text = "Lives Left: " + claireLives.ToString();
    }

    private void ShowGameOverUI()
    {
        gameOverUI.SetActive(true); 
        Time.timeScale = 0; 
    }
}
