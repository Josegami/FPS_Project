using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;
using DG.Tweening.Core.Easing;
using UnityEngine.Audio;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Escape Menu Settings")]
    public GameObject escapeMenuPanel;
    public bool isPaused = false;

    [Header("Audio Settings")]
    public AudioMixer masterMixer; 
    public Slider volumeSlider;   

    /// <summary>
    /// Gets the numerical index of the current level from the scene name (e.g., "Level3" -> 3).
    /// Returns 0 if it is not a numbered level.
    /// </summary>
    public int CurrentLevelIndex
    {
        get
        {
            // Get active scene name (e.g., "Level3")
            string sceneName = SceneManager.GetActiveScene().name;

            // Check if it starts with "Level"
            if (sceneName.StartsWith("Level") && sceneName.Length > 5)
            {
                // Try to parse the number following "Level"
                if (int.TryParse(sceneName.Substring(5), out int index))
                {
                    return index;
                }
            }
            return 0; // 0 for non-level scenes (e.g., MainMenu)
        }
    }

    private void Start()
    {
        float currentVol;
        masterMixer.GetFloat("MasterVolume", out currentVol);
        if (volumeSlider != null) volumeSlider.value = Mathf.Pow(10, currentVol / 20);
    }

    private void Awake()
    {
        // Singleton Implementation
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        // LevelManager is scene-specific, so DontDestroyOnLoad is not used here.
    }

    private void Update()
    {
        // Toggle pause menu with Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void SetVolume(float sliderValue)
    {
        masterMixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);
    }

    public void PauseGame()
    {
        isPaused = true;
        if (escapeMenuPanel != null) escapeMenuPanel.SetActive(true);
        Time.timeScale = 0f;

        // Unlock cursor for menu navigation
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        isPaused = false;
        if (escapeMenuPanel != null) escapeMenuPanel.SetActive(false);
        Time.timeScale = 1f;

        // Lock cursor back for gameplay
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /// <summary>
    /// Checks the current level and saves the next level as the maximum unlocked level.
    /// </summary>
    private void UnlockNextLevel()
    {
        int currentLevel = CurrentLevelIndex;
        int nextLevelToUnlock = currentLevel + 1;

        // 1. Get the current maximum unlocked level (default is 1 if key doesn't exist)
        int currentMaxUnlocked = PlayerPrefs.GetInt("MAXLEVEL_KEY", 1);

        // 2. If the next level is higher than the current max, update PlayerPrefs
        if (nextLevelToUnlock > currentMaxUnlocked)
        {
            PlayerPrefs.SetInt("MAXLEVEL_KEY", nextLevelToUnlock);
            PlayerPrefs.Save(); // Save data to disk
            Debug.Log($"New maximum level unlocked: Level {nextLevelToUnlock}");
        }
    }

    //              BUTTONS
    public void NextLevel()
    {
        // Resume the game
        Time.timeScale = 1f;

        UnlockNextLevel(); // Unlock before loading

        int nextLevelIndex = CurrentLevelIndex + 1;
        string nextLevelName = "Level" + nextLevelIndex;

        // Load the next level scene by name
        SceneManager.LoadScene(nextLevelName);
    }

    public void RetryLevel()
    {
        // Resume the game and reload the current level
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitToMenu()
    {
        // Resume the game and return to main menu
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}