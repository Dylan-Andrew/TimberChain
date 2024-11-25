using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBranch : MonoBehaviour
{
    // Default duration before the branch is destroyed (modifiable by LevelGenerator)
    private static float baseDestructionDelay = 5f;
    private static float minimumDestructionDelay = 0.75f;

    // This method is called when another collider makes contact with this object's collider (2D)
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Hook"))
        {
            // Ensure the delay respects the minimum destruction time
            float destructionDelay = Mathf.Max(baseDestructionDelay, minimumDestructionDelay);
            Destroy(gameObject, destructionDelay);
        }
    }

    // Method to update the destruction delay (static)
    public static void UpdateDestructionDelay(float newDelay)
    {
        baseDestructionDelay = Mathf.Max(newDelay, minimumDestructionDelay);
    }
}
