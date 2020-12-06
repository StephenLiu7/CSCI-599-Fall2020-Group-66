using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TutorialControl : MonoBehaviour
{
    // Start is called before the first frame update
    private XiaowenMonsterSpawn xms;
    private GameObject mPlayer;
    public GameObject pill;
    public GameObject optional_gun;
    private FixHealth fh;
    private int index = 0;

    private bool inTransition = false;
    public TMPro.TextMeshProUGUI TextPro;
    private float transitionTimer = -1.0f;
    private string[] popUps = new string[]
    {
        "Lecture 1",
        "Lecture 2: Shooting \n Now, explore on the lower right corner, where you will find another" +
        " joystick. Follow the same procedure to shoot as the previous step!",
        "Lecture 3: Items. \n Now find a pill on the map and Eat it. Remember this is only one of many items you will see throughout the game",
        "Lecture 4: other weapons. \n now find an different weapon on the map and pick it. Except for this one, there is one more in the game!",
        "Your tutorial has completed. You can now try the easy mode of our game to get more familiar!"
    };

    private string transitionString;

    void Start()
    {
        xms = GameObject.FindWithTag("MainCamera").GetComponent<XiaowenMonsterSpawn>();
        mPlayer = GameObject.FindWithTag("Player");
        fh = GameObject.FindWithTag("MainCamera").GetComponent<FixHealth>();
        transitionString = "Congratulations, you managed to finish the previous step!";
    }

    private void changeText(string newText)
    {
        TextPro.color = inTransition ? new Color32(0, 0, 128, 255) : new Color32(255, 0, 0, 255);
        TextPro.fontSize = inTransition ? 32 : 18;
        TextPro.text = newText;
    }

    public void backToMainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 3);
    }

    public bool haveMoved()
    {
        if (!fh.initialized)
        {
            return false;
        }
        if (Mathf.Abs(mPlayer.transform.position.x - 2.0f) > 0.7f)
        {
            return true;
        }else if (Mathf.Abs(mPlayer.transform.position.y - 2.0f) > 0.7f)
        {
            return true;
        }
        return false;
    }
    // Update is called once per frame
    void Update()
    {
        if (inTransition)
        {
            transitionTimer -= Time.deltaTime;
            if (transitionTimer < 0.0f)
            {
                index += 1;
                inTransition = false;
                changeText(popUps[index]);

                if (index == 1)
                {
                    mPlayer.GetComponent<PlayerMovement>().hasShooted = false;
                    
                }
                else if (index == 2)
                {
                    mPlayer.GetComponent<PlayerMovement>().hasEatenPill = false;
                    Vector2 spawnPos = new Vector2(0.0f, 0.0f);
                    Instantiate(pill, spawnPos, Quaternion.identity);
                    GameObject.FindWithTag("Player").transform.position = new Vector3(6.15f, -4.7f, 0);
                }else if (index == 3)
                {
                    mPlayer.GetComponent<PlayerMovement>().hasEatenGun = false;
                    Vector2 spawnPos = new Vector2(0.0f, 0.0f);
                    Instantiate(optional_gun, spawnPos, Quaternion.identity);
                    GameObject.FindWithTag("Player").transform.position = new Vector3(6.15f, -4.7f, 0);
                    print("I am spawning weappon..................");
                }
            }
        }
        else
        {
            if (index == 0)
            {
                if (haveMoved())
                {
                    inTransition = true;
                    transitionTimer = 3.0f;
                    changeText(transitionString);
                }
            }else if (index == 1)
            {
                if (mPlayer.GetComponent<PlayerMovement>().hasShooted)
                {
                    inTransition = true;
                    transitionTimer = 3.0f;
                    changeText(transitionString);
                }
            }else if (index == 2)
            {
                if (mPlayer.GetComponent<PlayerMovement>().hasEatenPill)
                {
                    inTransition = true;
                    transitionTimer = 3.0f;
                    changeText(transitionString);
                }
            }else if (index == 3)
            {
                if (mPlayer.GetComponent<PlayerMovement>().hasEatenGun)
                {
                    inTransition = true;
                    transitionTimer = 3.0f;
                    changeText(transitionString);
                }
            }
        }
    }
}
