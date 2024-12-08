using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;    // Reference to the paper plane's transform
    public Vector3 offset;      // Offset distance between the camera and the paper plane
    public float smoothSpeed = 0.125f;  // Adjustable smoothing factor

    // Optional: Adjust this to control the rotation speed of the camera
    public float rotationSpeed = 5f;

    void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("Camera target is not set!");
            return;
        }

        // Calculate the desired position for the camera
        Vector3 desiredPosition = target.position + offset;

        // Smoothly interpolate between the current camera position and the desired position using damping
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // Make the camera always look at the paper plane with some rotation smoothing
        Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }
}
