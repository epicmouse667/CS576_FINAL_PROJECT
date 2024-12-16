using UnityEngine;
using TMPro; // If using TextMeshPro

public class AttachTextOnCollision : MonoBehaviour
{
    public GameObject textObject; // Assign the text object in the Inspector
    private bool textAttached = false; // Prevent repeated activation

    void Start()
    {
        // Ensure the text is disabled initially
        if (textObject != null)
        {
            textObject.SetActive(false);
        }
        else
        {
            Debug.LogError("Text Object is not assigned!");
        }
    }

    // Collision detection (use OnTriggerEnter for trigger-based colliders)
    private void OnCollisionEnter(Collision collision)
    {
        if (!textAttached && collision.gameObject.CompareTag("Player")) // Check for Player tag
        {
            textAttached = true; // Mark text as attached
            textObject.SetActive(true); // Activate the text
            AttachTextToObject(); // Attach to the object
        }
    }

    void AttachTextToObject()
    {
        // Make the text a child of the current object
        textObject.transform.SetParent(transform);

        // Position text slightly above the object
        textObject.transform.localPosition = new Vector3(0, 1.5f, 0); // Adjust as needed
    }
}