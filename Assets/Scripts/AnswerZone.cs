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
        Debug.Log("Destroy!!");
        if (other.CompareTag("Player"))
        {
            Debug.Log("Destroy!!");
            questionManager.OnAnswerSelected(isCorrect);
        }
    }
}
