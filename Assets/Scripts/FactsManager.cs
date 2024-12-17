using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;

[System.Serializable]
public class Fact
{
    public string fact;
}

public class FactsManager : MonoBehaviour
{
    public TMP_Text factText;
    private List<Fact> facts = new List<Fact>();

    // Start is called before the first frame update
     public AudioSource audioSource;             // AudioSource component
    public AudioClip backgroundMusic;           // Background music clip
    
    void Start()
    {
        PlayBackgroundMusic();
        LoadFacts();
        DisplayRandomFact();
    }

    void LoadFacts()
    {
        // Define the path to the JSONL file
        string filePath = Path.Combine(Application.dataPath, "json_data/facts.jsonl");

        if (File.Exists(filePath))
        {
            // Read the JSONL file line by line
            string[] lines = File.ReadAllLines(filePath);

            foreach (string line in lines)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    // Parse each line into a Fact object
                    try
                    {
                        Fact fact = JsonUtility.FromJson<Fact>(line);
                        facts.Add(fact);
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError($"Error parsing line: {line}. Exception: {e.Message}");
                    }
                }
            }

            Debug.Log("Facts loaded successfully.");
        }
        else
        {
            Debug.LogError($"Error: Could not find the JSONL file at {filePath}!");
        }
    }

    void DisplayRandomFact()
    {
        if (facts.Count > 0)
        {
            // Pick a random fact from the list
            int randomIndex = Random.Range(0, facts.Count);
            string randomFact = facts[randomIndex].fact;

            // Display the fact in the TextMeshPro text component
            Debug.Log($"Random Geometry Fact: {randomFact}");
            factText.text = randomFact;
        }
        else
        {
            Debug.LogWarning("No facts available to display.");
        }
    }

    void PlayBackgroundMusic()
    {
        if (audioSource != null && backgroundMusic != null)
        {
            audioSource.loop = true; // Loop the background music
            audioSource.clip = backgroundMusic;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("AudioSource or Background Music clip is missing.");
        }
    }

    
}
