using UnityEngine;

public class Move : MonoBehaviour
{
    private Camera mainCamera;
    public float minDistance = 5f;

    void Start()
    {
        mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found in the scene!");
        }
    }

    void Update()
    {
        if (mainCamera != null)
        {
            // Calculate the target position behind the main camera along the Z-axis
            float targetZ = Mathf.Max(mainCamera.transform.position.z + minDistance, transform.position.z);
            Vector3 targetPosition = new Vector3(transform.position.x, transform.position.y, targetZ);

            // Move the object towards the target position
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime);
        }
    }
}
