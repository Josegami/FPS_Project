using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;
using DG.Tweening.Core.Easing;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

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

    /// <summary>
    /// Checks the current level and saves the next level as the maximum unlocked level.
    /// </summary>
    private void UnlockNextLevel()
    {
        int currentLevel = CurrentLevelIndex;
        int nextLevelToUnlock = currentLevel + 1;

        // 1. Get the current maximum unlocked level (default is 1 if key doesn't exist)
        int currentMaxUnlocked = PlayerPrefs.GetInt(GameConstants.MAXLEVEL_KEY, 1);

        // 2. If the next level is higher than the current max, update PlayerPrefs
        if (nextLevelToUnlock > currentMaxUnlocked)
        {
            PlayerPrefs.SetInt(GameConstants.MAXLEVEL_KEY, nextLevelToUnlock);
            PlayerPrefs.Save(); // Save data to disk
            Debug.Log($"New maximum level unlocked: Level {nextLevelToUnlock}");
        }
    }

    //             BUTTONS
    public void NextLevel()
    {
        // Resume the game
        Time.timeScale = 1f;
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