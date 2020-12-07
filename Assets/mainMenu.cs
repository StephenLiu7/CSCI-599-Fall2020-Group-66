using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class mainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        
    }
    /*
    public void SettingGame()
    {
        SceneManager.LoadScene("gameModeMenu");
    }
    */

    public void QuitGame()
    {
        Application.Quit();
    }

    public void easyMode()
    {
        GameMode.Difficulty = "easy";
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void mediumMode()
    {
        GameMode.Difficulty = "medium";
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void hardMode()
    {
        GameMode.Difficulty = "hard";
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void backToMainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void tutorial()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 3);
    }


}
