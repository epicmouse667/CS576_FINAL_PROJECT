using UnityEngine;
using TMPro; // Import TextMesh Pro namespace
using System.Collections.Generic;

public class QuestionManager : MonoBehaviour
{
    public QuestionLoader questionLoader;
    public string difficulty = "easy"; // Set difficulty (easy or medium)
    private List<Question> currentQuestions;
    private int currentQuestionIndex;

    public TMP_Text questionTextUI; // Updated to TMP_Text
    public GameObject[] answerZones;

    void Start()
    {
        currentQuestions = questionLoader.GetQuestions(difficulty);
        if (currentQuestions.Count > 0)
        {
            ShuffleQuestions();
            Debug.Log($"Loaded {currentQuestions.Count} easy questions.");
        }
        else
        {
            Debug.LogError("No questions found for the specified difficulty.");
        }
        LoadNextQuestion();
    }

    public GameObject GetChildWithTag(GameObject parent, string tag)
    {
        foreach (Transform child in parent.transform)
        {
            if (child.CompareTag(tag))
            {
                return child.gameObject;
            }
        }
        return null;
    }

    public void LoadNextQuestion()
    {
        if (currentQuestionIndex < currentQuestions.Count)
        {
            Question question = currentQuestions[currentQuestionIndex];
            questionTextUI.text = question.question;

            for (int i = 0; i < answerZones.Length; i++)
            {
                if (i < question.options.Count)
                {
                    // TextMeshProUGUI textComponents = answerZones[i].GetComponentInChildren<TextMeshProUGUI>();

                    // Debug.Log(textComponents);
                    // answerZones[i].GetChildrenByTag("AZ").GetComponent<TMP_Text>().text = question.options[i]; // Updated
                    // answerZones[i].SetActive(true);
                    GameObject child = GetChildWithTag(answerZones[i], "AZ");

                    if (child != null)
                        child.GetComponent<TMP_Text>().text = question.options[i];

                    // Assign whether this option is correct
                    answerZones[i].GetComponent<AnswerZone>().isCorrect = (question.options[i] == question.answer);
                }
                else
                {
                    // answerZones[i].SetActive(false); // Hide unused zones
                }
            }
        }
        else
        {
            Debug.Log("All questions completed!");
            // Trigger puzzle completion logic
        }
    }

    public void OnAnswerSelected(bool isCorrect)
    {
        if (isCorrect)
        {
            Debug.Log("Correct Answer! Progress!");
        }
        else
        {
            Debug.Log("Wrong Answer! Lose a Life!");
        }

        currentQuestionIndex++;
        LoadNextQuestion();
    }
    private void ShuffleQuestions()
    {
        for (int i = 0; i < currentQuestions.Count; i++)
        {
            int randomIndex = Random.Range(0, currentQuestions.Count);
            Question temp = currentQuestions[i];
            currentQuestions[i] = currentQuestions[randomIndex];
            currentQuestions[randomIndex] = temp;
        }
    }
}
