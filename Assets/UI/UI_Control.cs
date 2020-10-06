using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Control : MonoBehaviour
{

    public GameObject player;
    PlayerMovement playerScript;
    public HealthBar healthBar;
    //==================health potion=====================
    public Image abilityImage1;
    public float cooldown1 = 5;
    bool isCooldown = false;
    public KeyCode ability1;
    public Text ab_counter1;
    //=====================================================


    // Start is called before the first frame update
    void Start()
    {
        abilityImage1.fillAmount = 1;
        player = GameObject.Find("Player");
        playerScript = player.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {

        Ability_1();


    }

    void Ability_1()
    {
        if (Input.GetKey(ability1) && isCooldown == false && playerScript.hpAmount >= 1)
        {
            isCooldown = true;
            abilityImage1.fillAmount = 0;
            playerScript.currentHealth += 50;
            if(playerScript.currentHealth >= 100)
            {
                playerScript.currentHealth = 100;
            }
            healthBar.SetHealth(playerScript.currentHealth);
            playerScript.hpAmount -= 1;
            
        }
        if (isCooldown)
        {
            abilityImage1.fillAmount += 1 / cooldown1 * Time.deltaTime;
            if (abilityImage1.fillAmount >= 1)
            {
                abilityImage1.fillAmount = 1;
                isCooldown = false;
            }
        }
        
        ab_counter1.text = playerScript.hpAmount.ToString();
    }
}
