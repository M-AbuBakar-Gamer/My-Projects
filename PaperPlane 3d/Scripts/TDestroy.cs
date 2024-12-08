using UnityEngine;

public class TDestroy : MonoBehaviour
{
    public float offset = 0.0f; // Set the offset value as needed

    void Update()
    {
        // Assuming the player is the main camera, adjust accordingly
        float playerZPosition = Camera.main.transform.position.z;

        // Check if the object's (z position + offset) is less than the player's z position
        if (transform.position.z + offset < playerZPosition)
        {
            // Destroy the object
            Destroy(gameObject,2f);
        }
    }
}
