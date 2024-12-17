using UnityEngine;

public class PuzzlePickup : MonoBehaviour
{
    // Reference to the GameManager to increment puzzle count
    // public GameManager gameManager;

    // When the player collides with the puzzle
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("collided with puzzle - " + other.gameObject.name);
        // Check if the object that collided is the player
        if (other.CompareTag("Player"))
        {
            // Increment puzzle count in the GameManager
            CharacterContoller player = other.GetComponent<CharacterContoller>();

            if (player != null)
            {
                // Increment the puzzle count in the Player script
                player.puzzleCollected++;
            }

            // Destroy the puzzle object to simulate picking it up
            Destroy(gameObject);
        }
    }
}