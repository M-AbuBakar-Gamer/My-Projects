using UnityEngine;

public class CameraMovemment: MonoBehaviour
{
    public Transform target; // Reference to your character's Transform
    public Vector3 offset = new Vector3(0f, 2f, -5f); // Offset from the character's position

    public float smoothSpeed = 10.0f; // Smoothing factor for camera movement

    private void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("Camera target is not set!");
            return;
        }

        // Calculate the desired position for the camera
        Vector3 desiredPosition = target.position + offset;

        // Use Lerp to smoothly move the camera towards the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;

        // Ensure the camera always looks at the character
        transform.LookAt(target);
    }
}
