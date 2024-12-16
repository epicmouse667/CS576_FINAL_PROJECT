using UnityEngine;

public class AnswerZone : MonoBehaviour
{
    public bool isCorrect;
    private QuestionManager questionManager;

    void Start()
    {
        questionManager = FindObjectOfType<QuestionManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            questionManager.OnAnswerSelected(isCorrect);
        }
    }
}
