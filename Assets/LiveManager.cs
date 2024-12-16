using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LiveManager : MonoBehaviour
{
    public int asunaLives = 5;   
    public TMP_Text lifeText;          // Reference to the UI Text element for lives
    //public GameObject gameOverUI;  // Reference to the Game Over UI panel

    void Start()
    {
        // Attempt to find the lifeText GameObject
        lifeText = GameObject.Find("LivesLeft")?.GetComponent<TMP_Text>();
        if (lifeText == null)
        {
            Debug.LogError("LifeText reference is missing!");
        }

        //gameOverUI.SetActive(false); 
        UpdateLifeText();  // Initialize the UI with Claire's lives
    }

    public void ReduceLives()
{
    if (asunaLives > 0) // Check if lives are greater than 0
    {
        asunaLives--;  // Reduce lives
        UpdateLifeText();  // Update UI

        if (asunaLives == 0)
        {
            Debug.Log("No more lives left! Game Over!");
            //ShowGameOverUI(); // Uncomment to show the Game Over screen
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
        //gameOverUI.SetActive(true); 
        Time.timeScale = 0; 
    }
}
