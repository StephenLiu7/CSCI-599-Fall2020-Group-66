using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class EndMenu : MonoBehaviour
{
    
    public GameObject player;
    PlayerMovement playerScript;
    public GameObject endMenu;
    public TextMeshProUGUI monsterkilled;
    public TextMeshProUGUI survivalTime;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        playerScript = player.GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (playerScript.player_dead)
        {
            Invoke("showEndMenu", 2.0f);
            
        }

       

    }
    private void showEndMenu()
    {
        endMenu.SetActive(true);
        Time.timeScale = 0f;
        monsterkilled.text = "Monsters Killed: " + AnalyticsAPI.BossMonsterDeadCount;
        survivalTime.text = "Survival Time: " + PlayerMovement.survivalTimes;
    }
    

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
