using UnityEngine;

public class Destroy : MonoBehaviour
{
    void Update()
    {
        // Assuming the player is the main camera, adjust accordingly
        float playerZPosition = Camera.main.transform.position.z;

        // Check if the object's z position is less than the player's z position
        if (transform.position.z < playerZPosition)
        {
            // Destroy the object
            Destroy(gameObject,2f);
        }
    }
}
