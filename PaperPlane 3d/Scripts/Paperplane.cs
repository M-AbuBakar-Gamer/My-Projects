using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Paperplane : MonoBehaviour
{
    public float initialForwardSpeed = 5f;
    public float tiltAmount = 20f;
    public float pitchSpeed = 3f;
    public float lateralSpeed = 5f;
    public float rotationAngle = 45f;
    public float initialVerticalSpeed = 5f;
    public Joystick joystick;
    public Text collisionText;
    public Slider heightSlider;
    public Slider powerSlider;

    private Rigidbody rb;
    private Quaternion originalRotation;
    private float forwardSpeed;
    private float verticalSpeed;
    private float maxHeight = 8f;
    private float powerDecreaseRate = .7f;
    private float powerIncreaseRate = .35f;

    public GameObject activationObject;
    public Text heightText;

    private bool isBlipping = false;
    private float blipThreshold = 8f;
    private float blipSpeed = 5f;


    private TerrainSpawner terrainSpawner;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalRotation = transform.rotation;
        joystick = GameObject.FindObjectOfType<Joystick>();
        collisionText.gameObject.SetActive(false);

        // Set initial speeds
        forwardSpeed = initialForwardSpeed;
        verticalSpeed = initialVerticalSpeed;

        terrainSpawner = FindObjectOfType<TerrainSpawner>();
        if (terrainSpawner == null)
        {
            Debug.LogError("TerrainSpawner script not found in the scene!");
        }

        // Set initial values for sliders
        heightSlider.minValue = 2f;
        heightSlider.maxValue = maxHeight;
        heightSlider.value = 2f;

        powerSlider.minValue = 0f;
        powerSlider.maxValue = 10f;
        powerSlider.value = 10f;
       
    }


    void Update()
    {
        if (GameManager.instance.IsGameStarted)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            if (joystick != null)
            {
                horizontalInput = joystick.Horizontal;
                verticalInput = joystick.Vertical;
            }

            if (forwardSpeed <= 8f || verticalSpeed <= 8f)
            {
                forwardSpeed += 0.025f * Time.deltaTime;
                verticalSpeed += 0.025f * Time.deltaTime;
            }

            // Update the height slider
            if (transform.position.y <= maxHeight)
            {
                heightSlider.value = transform.position.y;
            }
            else
            {
                // If the player is above the maxHeight, start decreasing power
                powerSlider.value -= powerDecreaseRate * Time.deltaTime;
            }

            // Update the power slider
            if (transform.position.y <= maxHeight)
            {
                powerSlider.value += powerIncreaseRate * Time.deltaTime;
            }

            // Update power slider color based on its value
            UpdatePowerSliderColor();
            if (powerSlider.value <= 0f)
            {
                StartCoroutine(ShowCollisionText());
                GameManager.instance.GameOver();
            }

            transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);

            float pitch = verticalInput * tiltAmount;

            float limitedRotation = Mathf.Clamp(horizontalInput, -1f, 1f) * rotationAngle;
            transform.rotation = Quaternion.Euler(-pitch, limitedRotation, 0f);

            float verticalMovement = verticalInput * verticalSpeed;
            transform.Translate(Vector3.up * verticalMovement * Time.deltaTime);

            if (verticalInput == 0 && horizontalInput == 0)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, originalRotation, pitchSpeed * Time.deltaTime);
            }

            rb.MovePosition(rb.position + transform.forward * forwardSpeed * Time.fixedDeltaTime);
            rb.MoveRotation(Quaternion.Euler(-pitch, limitedRotation, 0f) * originalRotation);
            rb.MovePosition(rb.position + transform.up * verticalMovement * Time.fixedDeltaTime);

            if (verticalInput == 0 && horizontalInput == 0)
            {
                rb.MoveRotation(Quaternion.Slerp(rb.rotation, originalRotation, pitchSpeed * Time.fixedDeltaTime));
            }

            CheckTerrainCollision();
            if (transform.position.z > 120f)
            {
                if (activationObject != null)
                {
                    activationObject.SetActive(true);
                }
            }

            // Update height text
            if (heightText != null)
            {
                heightText.text = transform.position.y.ToString("F2");

                // Check if player's y movement exceeds the blip threshold
                if (transform.position.y > blipThreshold && !isBlipping)
                {
                    StartCoroutine(StartBlipping());
                }
            }
        }
    }
    IEnumerator StartBlipping()
    {
        isBlipping = true;

        while (transform.position.y > blipThreshold)
        {
            // Change text color between red and white
            heightText.color = Color.red;
            yield return new WaitForSeconds(0.2f);
            heightText.color = Color.white;
            yield return new WaitForSeconds(0.2f);
        }

        isBlipping = false;
    }
    void UpdatePowerSliderColor()
    {
        float powerPercentage = powerSlider.value / powerSlider.maxValue;

        Color targetColor;

        // Set the target color based on its percentage value
        if (powerPercentage >= 0.8f)
        {
            targetColor = Color.white;
        }
        else if (powerPercentage >= 0.5f)
        {
            targetColor = Color.green;
        }
        else if (powerPercentage >= 0.3f)
        {
            targetColor = Color.yellow;
        }
        else if (powerPercentage > 0f)
        {
            targetColor = Color.red;
        }
        else
        {
            targetColor = Color.black;
        }

        // Smoothly interpolate the color change
        powerSlider.fillRect.GetComponent<Image>().color = Color.Lerp(
            powerSlider.fillRect.GetComponent<Image>().color, targetColor, Time.deltaTime * 2f
        );
    }


    void CheckTerrainCollision()
    {
        RaycastHit hit;

        // Raycast in the downward direction
        if (!Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, LayerMask.GetMask("Terrain")))
        {
            // If the ray doesn't hit the terrain below the player, trigger game over
            StartCoroutine(ShowCollisionText());
            GameManager.instance.GameOver();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            StartCoroutine(ShowCollisionText());
            GameManager.instance.GameOver();
        }
    }

    IEnumerator ShowCollisionText()
    {
        collisionText.gameObject.SetActive(true);

        // Display the collision text for 2 seconds
        yield return new WaitForSeconds(2f);

        // Hide the collision text
        collisionText.gameObject.SetActive(false);
    }
}
