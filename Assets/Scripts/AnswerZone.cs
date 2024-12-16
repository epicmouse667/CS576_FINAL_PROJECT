using UnityEngine;

public class AnswerZone : MonoBehaviour
{
    public bool isCorrect; // Assigned dynamically in QuestionManager
    private QuestionManager questionManager;

    void Start()
    {
        questionManager = FindObjectOfType<QuestionManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Ensure the player tag is "Player"
        {
            if (isCorrect)
            {
                questionManager.OnAnswerSelected(true);
            }
            else
            {
                questionManager.OnAnswerSelected(false);
            }
        }
    }
}


/*
using UnityEngine;

public class AnswerZone : MonoBehaviour
{
    public bool isCorrect; // Assigned in QuestionManager
    private QuestionManager questionManager; // Reference to QuestionManager

    void Start()
    {
        // Find the QuestionManager in the scene
        questionManager = FindObjectOfType<QuestionManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the character entered the trigger zone
        if (other.CompareTag("Player")) 
        {
            if (isCorrect)
            {
                Debug.Log("Correct Answer!");
                questionManager.OnAnswerSelected(true); // Notify QuestionManager
            }
            else
            {
                Debug.Log("Wrong Answer!");
                questionManager.OnAnswerSelected(false); // Notify QuestionManager
            }
        }
    }
}
*/