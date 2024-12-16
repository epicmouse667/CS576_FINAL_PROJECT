using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class QuestionManager : MonoBehaviour
{
    public QuestionLoader questionLoader;      // Reference to the QuestionLoader
    public string difficulty = "easy";         // Difficulty level to load questions
    private List<Question> gameQuestions;      // Holds the first 6 questions
    private int currentQuestionIndex = 0;      // Tracks the current question

    [Header("Scene Game Objects")]
    public Transform[] questionObjects;        // 6 GameObjects for floating questions
    public GameObject[] answerZones;           // 6x4 = 24 AnswerZone GameObjects

    [Header("Game Settings")]
    public int playerLives = 3;                // Number of lives for the player

    void Start()
    {
        // Load questions from the QuestionLoader
        List<Question> allQuestions = questionLoader.GetQuestions(difficulty);

        if (allQuestions == null || allQuestions.Count == 0)
        {
            Debug.LogError("No questions found for the specified difficulty!");
            return;
        }

        // Get the first 6 questions and shuffle them
        gameQuestions = allQuestions.GetRange(0, Mathf.Min(6, allQuestions.Count));
        ShuffleQuestions();
        LoadQuestionsAndAnswers();
    }

    void LoadQuestionsAndAnswers()
    {
        if (questionObjects.Length < 6 || answerZones.Length < 24)
        {
            Debug.LogError("Not enough Question Objects or Answer Zones assigned in the Inspector.");
            return;
        }

        // Loop through 6 questions and populate the floating question text and answer zones
        for (int i = 0; i < gameQuestions.Count; i++)
        {
            Question question = gameQuestions[i];

            // Set the question text (3D floating TextMeshPro)
            TMP_Text questionText = questionObjects[i].GetComponentInChildren<TMP_Text>();
            if (questionText != null)
            {
                questionText.text = question.question;
            }
            else
            {
                Debug.LogError($"Question Object {i} is missing a TMP_Text component!");
            }

            // Set answer options dynamically for the 4 answer zones
            for (int j = 0; j < 4; j++)
            {
                int answerZoneIndex = (i * 4) + j; // Get index in the AnswerZones array
                GameObject optionZone = answerZones[answerZoneIndex];

                TMP_Text optionText = optionZone.GetComponentInChildren<TMP_Text>();
                if (optionText != null)
                {
                    optionText.text = question.options[j];
                }

                // Mark the correct answer
                AnswerZone answerZone = optionZone.GetComponent<AnswerZone>();
                if (answerZone != null)
                {
                    answerZone.isCorrect = (question.options[j] == question.answer);
                }
                else
                {
                    Debug.LogError($"AnswerZone script missing on Answer Zone {answerZoneIndex}!");
                }
            }
        }
    }

    public void OnAnswerSelected(bool isCorrect)
    {
        if (isCorrect)
        {
            Debug.Log("Correct Answer!");
            Vector3 spawnPosition = player.transform.position + player.transform.forward * 10.0f;
            spawnPosition.y = 4;

            // Instantiate the puzzle prefab at the calculated position
            Instantiate(puzzlePrefab[puzzleCount % 4], spawnPosition, Quaternion.identity);
            puzzleCount++;
            currentQuestionIndex++;
            CheckForCompletion();
        }
        else
        {
            Debug.Log("Wrong Answer! Lose a life.");
            playerLives--;

            if (playerLives <= 0)
            {
                Debug.Log("Game Over! Player has no lives left.");
                // TODO: Add Game Over Logic Here
            }
        }
    }

    private void ShuffleQuestions()
    {
        // Shuffle the questions list
        for (int i = 0; i < gameQuestions.Count; i++)
        {
            int randomIndex = Random.Range(0, gameQuestions.Count);
            Question temp = gameQuestions[i];
            gameQuestions[i] = gameQuestions[randomIndex];
            gameQuestions[randomIndex] = temp;
        }
    }

    void CheckForCompletion()
    {
        if (currentQuestionIndex >= gameQuestions.Count)
        {
            Debug.Log("All questions answered! You win!");
            // TODO: Add Game Win Logic Here
        }
    }
}

