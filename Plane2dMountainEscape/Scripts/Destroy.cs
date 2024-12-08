using UnityEngine;

public class Destroy: MonoBehaviour
{
    public float delayInSeconds = 3.0f; // The number of seconds before the object is destroyed

    private void Start()
    {
        // Invoke the DestroyObject method after the specified delay
        Invoke("DestroyObject", delayInSeconds);
    }

    private void DestroyObject()
    {
        // Destroy the GameObject this script is attached to
        Destroy(gameObject);
    }
}
