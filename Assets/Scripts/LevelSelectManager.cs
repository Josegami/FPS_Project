using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LevelSelectManager : MonoBehaviour
{

    private void Start()
    {
        //Get children length of Button Container --> Number levels
        GameObject buttonContainer = GameObject.Find("ButtonContainer");

        //dinamicaly put on every button the listener loadLevelSelection
        for (int i = 0; i < buttonContainer.transform.childCount; i++)
        {
            int index = i;
            //if the level button is lower or equal than the maxlevel reached
            if (index + 1 <= PlayerPrefs.GetInt(GameConstants.MAXLEVEL_KEY))
            {
                buttonContainer.transform.GetChild(index).GetComponent<Button>().interactable = true;
                buttonContainer.transform.GetChild(index).GetComponent<Button>().onClick.AddListener(() => LoadLevelSelection(index + 1));
            }
            else
            {
                buttonContainer.transform.GetChild(index).GetComponent<Button>().interactable = false;
            }

        }
    }

    /// <summary>
    /// Load the scene with LevelIndex + 1 to get the actual level name
    /// </summary>
    /// <param name="index">the index of the button in the array</param>
    void LoadLevelSelection(int index)
    {
        string levelName = "Level" + index;
        SceneManager.LoadScene(levelName);
    }

    public void ExitToMenu()
    {
        // Resume the game and return to main menu
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
