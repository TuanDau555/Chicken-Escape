using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class LoadSceneByIndex : MonoBehaviour
{
    public Button[] levelsButton;

    private void Awake()
    {
        LevelBlocked();
    }

    private void LevelBlocked()
    {
        //Level 1 unlocked at default
        int unlockedLevels = PlayerPrefs.GetInt("UnlockedLevel", 1);
        //Turn off interactable 
        for(int i = 0; i < levelsButton.Length; i++)
        {
            levelsButton[i].interactable = false;
        }
        //Turn on interactable
        for(int i = 0; i < unlockedLevels; i++)
        {
            levelsButton[i].interactable = true;
        }
    }

    public void LoadLevel(int scenesIndex)
    {
        SceneManager.LoadScene(scenesIndex);
        Time.timeScale = 1; // make sure game is active
        FindObjectOfType<AudioSource>().Play(); // make sure Audio is always playing when load new level
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
