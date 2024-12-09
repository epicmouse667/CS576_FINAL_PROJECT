using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class GeometryFacts
{
    public List<string> facts;
}

// TODO: update facts.json with actual facts

public class FactsManager : MonoBehaviour
{
    public TMP_Text fact;
    private GeometryFacts geometryFacts;
    // Start is called before the first frame update
    void Start()
    {
        LoadFacts();
        DisplayRandomFact();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void LoadFacts()
    {
        // Define the path to the JSON file
        string filePath = Path.Combine(Application.dataPath, "json_data/facts.json");

        if (File.Exists(filePath))
        {
            // Read the JSON file
            string jsonContent = File.ReadAllText(filePath);

            // Parse the JSON content
            geometryFacts = JsonUtility.FromJson<GeometryFacts>(jsonContent);
            Debug.Log("Facts loaded successfully.");
        }
        else
        {
            Debug.LogError($"Error: Could not find the JSON file at {filePath}!");
        }
    }

    void DisplayRandomFact()
    {
        if (geometryFacts != null && geometryFacts.facts.Count > 0)
        {
            // Pick a random fact
            int randomIndex = Random.Range(0, geometryFacts.facts.Count);
            string randomFact = geometryFacts.facts[randomIndex];

            // Display the fact in the console or in the game UI
            Debug.Log($"Random Geometry Fact: {randomFact}");
            fact.text = randomFact;

            // Optionally, display it in a UI element (e.g., TextMeshPro or Unity UI Text)
            // myTextUI.text = randomFact;
        }
        else
        {
            Debug.LogWarning("No facts available to display.");
        }
    }
}
