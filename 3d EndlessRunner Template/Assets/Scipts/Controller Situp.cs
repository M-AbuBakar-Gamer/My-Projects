using UnityEngine;

public class Controllersitup : MonoBehaviour
{
    // Character movement variables
    public float moveSpeed = 5.0f;
    public float jumpForce = 10.0f;
    private bool isJumping = false;
    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;

    // Wall-running variables
    public float wallRunDuration = 2.0f;
    public float wallRunSpeed = 7.0f;
    private float wallRunTimer = 0.0f;
    private bool isWallRunning = false;
    private Transform currentWall;

    // Gravity variables
    private float originalGravity;

    // Touch control variables
    private Vector2 touchStartPos;
    private bool isSwiping = false;
    public float swipeThreshold = 50.0f;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        originalGravity = Physics.gravity.y;
    }

    private void Update()
    {
        // Handle touch controls
        HandleTouchControls();

        // Check for wall-running input and timer
        if (isWallRunning && wallRunTimer > 0)
        {
            // Implement wall-running logic here
            // Example: moveDirection = currentWall.right * wallRunSpeed;
        }
        else
        {
            // Reset wall-running variables
            isWallRunning = false;
            wallRunTimer = 0.0f;

            // Handle regular character movement
            float horizontalInput = moveDirection.x;
            float verticalInput = moveDirection.z;

            Vector3 move = transform.right * horizontalInput + transform.forward * verticalInput;
            characterController.Move(move * moveSpeed * Time.deltaTime);

            // Jumping
            if (characterController.isGrounded)
            {
                moveDirection.y = 0;

                if (isJumping)
                {
                    isJumping = false;
                }

                if (Input.GetButtonDown("Jump") || (isSwiping && SwipeUp()))
                {
                    moveDirection.y = jumpForce;
                    isJumping = true;
                }
            }
            else
            {
                moveDirection.y += Physics.gravity.y * Time.deltaTime;
            }
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Check for wall-running surfaces (you may need to tag your wall surfaces accordingly)
        if (hit.collider.CompareTag("Wall") && !characterController.isGrounded)
        {
            // Start wall-running
            isWallRunning = true;
            wallRunTimer = wallRunDuration;
            currentWall = hit.transform;

            // Calculate a new move direction based on the wall's normal
            // This will make the character run along the wall
            moveDirection = Vector3.Cross(hit.normal, Vector3.up).normalized * wallRunSpeed;
        }
    }

    private void HandleTouchControls()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                touchStartPos = touch.position;
                isSwiping = true;
            }
            else if (touch.phase == TouchPhase.Moved && isSwiping)
            {
                Vector2 delta = touch.position - touchStartPos;

                if (delta.magnitude > swipeThreshold)
                {
                    // Detect swipe direction
                    if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
                    {
                        // Horizontal swipe
                        moveDirection.x = delta.x > 0 ? 1.0f : -1.0f;
                        moveDirection.z = 0.0f; // Reset vertical movement
                    }
                    else
                    {
                        // Vertical swipe
                        if (delta.y > 0 && characterController.isGrounded)
                        {
                            // Jump
                            moveDirection.y = jumpForce;
                            isJumping = true;
                        }
                        else if (delta.y < 0 && !characterController.isGrounded)
                        {
                            // Slide (crouch)
                            // Implement sliding logic here
                        }
                    }

                    isSwiping = false;
                }
            }
        }
    }

    private bool SwipeUp()
    {
        return (Input.touchCount > 0 && Input.GetTouch(0).deltaPosition.y > 0);
    }
}
