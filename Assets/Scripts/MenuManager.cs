using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    public TMP_Text highScoreUI;

    public AudioClip bg_music;
    public AudioSource main_channel;

    private void Start()
    {
        main_channel.PlayOneShot(bg_music);

        int highScore = SaveLoadManager.Instance.LoadHighScore();
        highScoreUI.text = $"Top Wave Survived: {highScore}";

        //First time we save Level 1 as max level
        if (!PlayerPrefs.HasKey(GameConstants.MAXLEVEL_KEY))
        {
            PlayerPrefs.SetInt(GameConstants.MAXLEVEL_KEY, 1);
        }
    }

    public void SelectLevel()
    {
        main_channel.Stop();

        //Go to the next Scene (Index Scene)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void SelectInfinite()
    {
        main_channel.Stop();

        //Go to the next Scene (Index Scene)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 5);
    }

    public void SelectTrain()
    {
        main_channel.Stop();

        //Go to the next Scene (Index Scene)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 6);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
