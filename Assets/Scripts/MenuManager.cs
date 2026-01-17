using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private void Start()
    {
        //First time we save Level 1 as max level
        if (!PlayerPrefs.HasKey(GameConstants.MAXLEVEL_KEY))
        {
            PlayerPrefs.SetInt(GameConstants.MAXLEVEL_KEY, 1);
        }
    }

    public void SelectLevel()
    {
        //Go to the next Scene (Index Scene)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void SelectInfinite()
    {
        //Go to the next Scene (Index Scene)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 5);
    }

    public void SelectTrain()
    {
        //Go to the next Scene (Index Scene)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 6);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
