using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Text scoreText;
    public Text bestScoreText;
    public Transform playerTransform; // Public variable for player's transform
    public bool IsGameStarted ;


    public GameObject gameOverPanel;
    public Text gameOverBestScoreText;
    public Text gameOverYourScoreText;
    public Button playAgainButton;
    public Button exitButton;
    public GameObject loadingPanel;
    public Slider loadingSlider;

    public Button pauseButton; // New pause button

    public GameObject pausePanel;
    public Text pauseYourScoreText;
    public Text pauseBestScoreText;
    public Button pausePlayAgainButton;
    public Button continueButton;
    public Button pauseExitButton;

    private float bestScore;

    public float speed;

    private void Awake()
    {
        // Ensure only one instance of the GameManager exists
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        IsGameStarted = true;
        bestScore = PlayerPrefs.GetFloat("BestScore", 0f);
        UpdateBestScoreText();

        if (playerTransform == null)
        {
            Debug.LogError("Player Transform not assigned in the GameManager!");
        }

        UpdateScoreText();
        pauseButton.onClick.AddListener(PauseGame); // Add listener for the new pause button
    }

    void Update()
    {
        if (playerTransform != null)
        {
            // Update the score based on the player's x-direction
            float currentX = playerTransform.position.z;
            UpdateScoreText();
            RenderSettings.skybox.SetFloat("_Rotation", Time.time * speed);
        }
    }

    void UpdateScoreText() // Remove the 'float currentX' parameter here
    {
        // Update the score text based on the player's x-direction
        if (playerTransform != null)
        {
            float currentX = playerTransform.position.z;
            int score = Mathf.FloorToInt(currentX);
            scoreText.text =  score.ToString();

            // Update the best score if the current score is higher
            if (score > bestScore)
            {
                bestScore = score;
               

                // Save the best score to PlayerPrefs (persistent storage)
                PlayerPrefs.SetFloat("BestScore", bestScore);
                PlayerPrefs.Save();
            }
        }
    }

    void UpdateBestScoreText()
    {
        bestScoreText.text = bestScore.ToString();
    }
    void PlayAgain()
    {
        // Show the loading panel
        loadingPanel.SetActive(true);

        // Start the asynchronous loading of the scene
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        // While the scene is loading, update the slider value
        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f); // 0.9 is the completion value
            loadingSlider.value = progress;

            yield return null;
        }
    }

    void ExitGame()
    {
        // Quit the application (only works in a standalone build)
        Application.Quit();
    }
    public void GameOver()
    {
        // Disable player movement or perform any other game over actions
        IsGameStarted = false;

        // Show the game over panel
        gameOverPanel.SetActive(true);

        // Set the best score and current score text on the game over panel
        gameOverBestScoreText.text = "Best Score: " + bestScore.ToString();
        gameOverYourScoreText.text = "Your Score: " + scoreText.text;

        // Attach the play again and exit button click events
        playAgainButton.onClick.AddListener(PlayAgain);
        exitButton.onClick.AddListener(ExitGame);
    }
    void PauseGame()
    {
        IsGameStarted = false;
        pausePanel.SetActive(true);
        pauseYourScoreText.text = "Your Score: " + scoreText.text;
        pauseBestScoreText.text = "Best Score: " + bestScore.ToString();

        pausePlayAgainButton.onClick.AddListener(PlayAgain);
        continueButton.onClick.AddListener(ContinueGame);
        pauseExitButton.onClick.AddListener(ExitGame);

        pauseButton.gameObject.SetActive(false); // Hide the pause button
    }

    void ContinueGame()
    {
        IsGameStarted = true;
        pausePanel.SetActive(false);
        pauseButton.gameObject.SetActive(true); // Show the pause button
    }
}
