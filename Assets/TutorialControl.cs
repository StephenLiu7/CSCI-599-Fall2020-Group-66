﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialControl : MonoBehaviour
{
    // Start is called before the first frame update
    private XiaowenMonsterSpawn xms;
    private GameObject mPlayer;
    private int index = 0;

    private bool inTransition = false;
    public TMPro.TextMeshProUGUI TextPro;
    private float transitionTimer = -1.0f;
    private string[] popUps = new string[]
    {
        "Lecture 1",
        "Lecture 2: Shooting \n Now, explore on the lower right corner, where you will find another" +
        " joystick. Follow the same procedure to shoot as the previous step!",
        "Your tutorial has completed. You can now try the easy mode of our game to get more familiar!"
    };

    private string transitionString;

    void Start()
    {
        xms = GameObject.FindWithTag("MainCamera").GetComponent<XiaowenMonsterSpawn>();
        mPlayer = GameObject.FindWithTag("Player");
        transitionString = "Congratulations, you managed to finish the previous step!";
    }

    private void changeText(string newText)
    {
        TextPro.color = inTransition ? new Color32(0, 255, 0, 200) : new Color32(255, 0, 0, 200);
        TextPro.text = newText;
    }
    public bool haveMoved()
    {
        if (Mathf.Abs(mPlayer.transform.position.x - 2.0f) > 0.1f)
        {
            return true;
        }else if (Mathf.Abs(mPlayer.transform.position.y - 2.0f) > 0.1f)
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
            }
        }
        else
        {
            if (index == 0)
            {
                if (haveMoved())
                {
                    inTransition = true;
                    transitionTimer = 2.5f;
                    changeText(transitionString);
                }
            }else if (index == 1)
            {
                
            }
        }
    }
}
