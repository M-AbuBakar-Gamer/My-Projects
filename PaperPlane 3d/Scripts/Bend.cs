using UnityEngine;

public class Bend : MonoBehaviour
{
    public float bendStrength = 0.1f;
    public float speed = 1f;

    void Update()
    {
        float offset = Mathf.Sin(Time.time * speed) * bendStrength;

        Matrix4x4 perspectiveMatrix = Camera.main.projectionMatrix;
        perspectiveMatrix[0, 2] = offset;
        perspectiveMatrix[1, 2] = offset;

        Camera.main.projectionMatrix = perspectiveMatrix;
    }
}
