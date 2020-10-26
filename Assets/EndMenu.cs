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
    public TextMeshProUGUI toxicgas;
    public TextMeshProUGUI monsterhit;
    public TextMeshProUGUI bullets;
    public TextMeshProUGUI accuracy;
    public TextMeshProUGUI mostuse;
    public TextMeshProUGUI shooting_on_target;

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
        survivalTime.text = "Monsters Killed: " + AnalyticsAPI.BossMonsterDeadCount +
                    "\r\n" + "Survival Time: " + PlayerMovement.survivalTimes +
                    "\r\n" + "Damge from toxic gas: " + playerScript.lostbytoxic +
                    "\r\n" + "Damge from monster: " + playerScript.lostbymonster +
                    "\r\n" + "Number of bullets used: " + playerScript.ana_bullet_counting +
                    "\r\n" + "Shooting accuracy: " + playerScript.acc +
                    "\r\n" + "Most use weapon: " + playerScript.most_use +
                    "\r\n" + "Shooting on monster: " + AnalyticsAPI.BossMonsterHitCount_static;
         /*                    
        survivalTime.text = "Survival Time: " + PlayerMovement.survivalTimes;
        toxicgas.text = "Damge from toxic gas: " + playerScript.lostbytoxic;
        monsterhit.text = "Damge from monster: " + playerScript.lostbymonster;
        bullets.text = "# Bullets used: " + playerScript.ana_bullet_counting;
        accuracy.text = "Shooting accuracy: " + playerScript.acc;
        mostuse.text = "Most use weapon: " + playerScript.most_use;
        shooting_on_target.text = "Shooting on monster: " + AnalyticsAPI.BossMonsterHitCount_static;
        */
    }


    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
