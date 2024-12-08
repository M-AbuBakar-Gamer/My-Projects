using UnityEngine;

public class Bird : MonoBehaviour
{
    public float minSpeed = 5f;
    public float maxSpeed = 10f;
    public float tiltSpeed = 5f;
    public float changeDirectionInterval = 5f;

    private float terrainWidth;
    private float terrainLength;
    private Vector3 direction;
    private float speed;
    private Terrain terrain;

    void Start()
    {
        SetRandomDirection();
    }

    void Update()
    {
        MoveBird();

        // Check if it's time to change direction
        if (Time.time % changeDirectionInterval < 0.1f)
        {
            SetRandomDirection();
        }
    }

    void MoveBird()
    {
        Vector3 newPosition = transform.position + direction * speed * Time.deltaTime;

        // Use Raycast to check for collisions with the terrain
        RaycastHit hit;
        if (Physics.Raycast(newPosition, Vector3.down, out hit, 5f)) // Adjust the ray length as needed
        {
            // If a collision is detected, change direction
            Vector3 newDirection = Vector3.Reflect(direction, hit.normal);
            direction = newDirection.normalized;
        }

        transform.Translate(direction * speed * Time.deltaTime, Space.World);

        // Tilt the bird based on its movement direction
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, tiltSpeed * Time.deltaTime);
        }
    }

    public void SetTerrainDimensions(float width, float length)
    {
        terrainWidth = width;
        terrainLength = length;
    }
    public void SetTerrain(Terrain newTerrain)
    {
        terrain = newTerrain;
    }
    void SetRandomDirection()
    {
        speed = Random.Range(minSpeed, maxSpeed);
        float randomAngle = Random.Range(0f, 360f);
        direction = Quaternion.Euler(0f, randomAngle, 0f) * Vector3.forward;
        direction.Normalize();
    }
}
