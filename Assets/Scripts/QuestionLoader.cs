using UnityEngine;
using System.Collections.Generic;
using System.IO;


[System.Serializable]
public class Question
{
    public string difficulty;
    public string question;
    public List<string> options;
    public string answer;
}

public class QuestionLoader : MonoBehaviour
{
    //public TextAsset questionsFile; // Drag the JSONL file into this field in the Inspector
    private List<Question> questions = new List<Question>();
    private string filePath;


    public List<Question> GetQuestions(string difficulty)
    {
        filePath = Path.Combine(Application.dataPath, "Json_Data/questions.jsonl");
        
        List<Question> allQuestions = LoadQuestions();
        

        // Filter questions based on difficulty
        return allQuestions.FindAll(q => q.difficulty == difficulty);
    }
    public List<Question> LoadQuestions()
    {
        if (File.Exists(filePath))
        {
            try
            {
                // Read all lines from the JSONL file
                string[] lines = File.ReadAllLines(filePath);

                // Parse each line into a Question object
                List<Question> questions = new List<Question>();
                foreach (string line in lines)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        Question question = JsonUtility.FromJson<Question>(line.Trim());
                        questions.Add(question);
                    }
                }

                return questions;
            }
            catch (IOException e)
            {
                Debug.LogError("Failed to read the file: " + e.Message);
            }
        }
        else
        {
            Debug.LogError("File not found: " + filePath);
        }

        return new List<Question>(); // Return empty list if loading fails
    }
}

/*
    public string LoadQuestions()
    {
        if (File.Exists(filePath))
        {
            try
            {
                // Read the file contents
                return File.ReadAllText(filePath);
            }
            catch (IOException e)
            {
                Debug.LogError("Failed to read the file: " + e.Message);
            }
        }
        else
        {
            Debug.LogError("File not found: " + filePath);
        }

        return null;
    }*/
    /*
private void LoadQuestions()
{
    TextAsset textAsset = Resources.Load<TextAsset>("questions"); // No file extension
    if (textAsset != null)
    {
        using (StringReader reader = new StringReader(textAsset.text))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                Question question = JsonUtility.FromJson<Question>(line);
                questions.Add(question);
            }
        }
        Debug.Log("Questions loaded: " + questions.Count);
    }
    else
    {
        Debug.LogError("Failed to load questions.jsonl from Resources folder.");
    }
}*/
/*
    private void LoadQuestions()
    {
        using (StringReader reader = new StringReader(questionsFile.text))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                Question question = JsonUtility.FromJson<Question>(line);
                questions.Add(question);
            }
        }
        Debug.Log("Questions loaded: " + questions.Count);
    }*/

