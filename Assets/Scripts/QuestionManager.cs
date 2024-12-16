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


/*
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class QuestionManager : MonoBehaviour
{
    public QuestionLoader questionLoader;
    public string difficulty = "easy";
    private List<Question> gameQuestions; // Holds the first 6 questions
    private int currentQuestionIndex = 0;

    public Transform[] questionObjects; // 6 GameObjects with floating TextMeshPro components
    public GameObject[] answerZones; // 6x4 = 24 AnswerZone GameObjects

    public int playerLives = 3;

    void Start()
    {
        // Load first 6 questions
        List<Question> allQuestions = questionLoader.GetQuestions(difficulty);
        gameQuestions = allQuestions.GetRange(0, Mathf.Min(6, allQuestions.Count));
    // Shuffle the questions
        if (allQuestions.Count > 0)
        {
            ShuffleQuestions();
            Debug.Log($"Loaded {allQuestions.Count} easy questions.");
        }
        else
        {
            Debug.LogError("No questions found for the specified difficulty.");
        }
        if (gameQuestions.Count < 6)
        {
            Debug.LogError("Not enough questions to load!");
        }

        LoadQuestionsAndAnswers();
    }

    void LoadQuestionsAndAnswers()
    {
        // Loop through 6 questions and assign text
        for (int i = 0; i < gameQuestions.Count; i++)
        {
            Question question = gameQuestions[i];

            // Set question text in 3D space
            TMP_Text questionText = questionObjects[i].GetComponentInChildren<TMP_Text>();
            questionText.text = question.question;

            // Set answer options dynamically for the 4 answer zones
            for (int j = 0; j < 4; j++)
            {
                int answerZoneIndex = (i * 4) + j; // Index in the answerZones array
                GameObject optionZone = answerZones[answerZoneIndex];

                // Set option text
                TMP_Text optionText = optionZone.GetComponentInChildren<TMP_Text>();
                optionText.text = question.options[j];

                // Assign correct answer flag to the zone
                optionZone.GetComponent<AnswerZone>().isCorrect = (question.options[j] == question.answer);
            }
        }
    }

    public void OnAnswerSelected(bool isCorrect)
    {
        if (isCorrect)
        {
            Debug.Log("Correct Answer!");
            currentQuestionIndex++;
            CheckForCompletion();
        }
        else
        {
            Debug.Log("Wrong Answer! Lose a life.");
            playerLives--;
            if (playerLives <= 0)
            {
                Debug.Log("Game Over!");
                // Add game over logic here
            }
        }
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
    void CheckForCompletion()
    {
        if (currentQuestionIndex >= gameQuestions.Count)
        {
            Debug.Log("All questions answered! You win!");
        }
    }
}

*/
/*
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

    public GameObject[] gameQuestions;


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

    public void LoadNextQuestion()
    {
        for()
        if (currentQuestionIndex < currentQuestions.Count)
        {
            Question question = currentQuestions[currentQuestionIndex];
            questionTextUI.text = question.question;

            for (int i = 0; i < answerZones.Length; i++)
            {
                if (i < question.options.Count)
                {
                    answerZones[i].GetComponent<TMP_Text>().text = question.options[i]; // Updated
                    answerZones[i].SetActive(true);

                    // Assign whether this option is correct
                    answerZones[i].GetComponent<AnswerZone>().isCorrect = (question.options[i] == question.answer);
                }
                else
                {
                    answerZones[i].SetActive(false); // Hide unused zones
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
*/
/*
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class QuestionManager : MonoBehaviour
{
    public QuestionLoader questionLoader;
    public string difficulty = "easy";
    private List<Question> gameQuestions; // First 6 questions
    private int currentQuestionIndex = 0;

    public TMP_Text[] questionTexts; // 6 TextMeshPro Text objects for questions
    public GameObject[] answerZones; // 6x4 = 24 AnswerZone GameObjects (6 sets of 4 options each)

    public int playerLives = 3; // Player lives

    void Start()
    {
        // Load questions and initialize the first 6
        List<Question> allQuestions = questionLoader.GetQuestions(difficulty);
        gameQuestions = allQuestions.GetRange(0, Mathf.Min(6, allQuestions.Count));
        if (allQuestions.Count > 0)
        {
            ShuffleQuestions();
            Debug.Log($"Loaded {allQuestions.Count} easy questions.");
        }
        else
        {
            Debug.LogError("No questions found for the specified difficulty.");
        }
        if (gameQuestions.Count < 6)
        {
            Debug.LogError("Not enough questions to load 6!");
        }

        LoadQuestionsAndAnswers();
    }

    void LoadQuestionsAndAnswers()
    {
        for (int i = 0; i < gameQuestions.Count; i++)
        {
            Question question = gameQuestions[i];
            questionTexts[i].text = question.question; // Set question text

            // Set answer options for each question's 4 zones
            for (int j = 0; j < 4; j++)
            {
                int answerZoneIndex = (i * 4) + j; // Calculate index in the answerZones array
                answerZones[answerZoneIndex].SetActive(true);
                answerZones[answerZoneIndex].GetComponentInChildren<TMP_Text>().text = question.options[j];
                answerZones[answerZoneIndex].GetComponent<AnswerZone>().isCorrect = (question.options[j] == question.answer);
            }
        }
    }

    public void OnAnswerSelected(bool isCorrect)
    {
        if (isCorrect)
        {
            Debug.Log("Correct Answer! Proceed to next question.");
            currentQuestionIndex++;
            CheckForCompletion();
        }
        else
        {
            Debug.Log("Wrong Answer! Lose a life.");
            playerLives--;
            if (playerLives <= 0)
            {
                Debug.Log("Game Over!");
            }
        }
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
    void CheckForCompletion()
    {
        if (currentQuestionIndex >= gameQuestions.Count)
        {
            Debug.Log("All questions completed! You win!");
        }
    }
}
*/