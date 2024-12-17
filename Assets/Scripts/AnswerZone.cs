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
            Destroy(transform.gameObject);
        }
    }
}



