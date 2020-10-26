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
    public Text hp_counter;
    //=====================================================

    //==================switch weapon=====================
   
    public Image[] weapons; //UI weapon image array
    bool[] got_weapons; // weapon have now
    int sec_weapon; //second hand weapon
    public Text num_bullets;
    private string infinity = "\u221e";
    int cur_weapon;
    //======================================================
    // Start is called before the first frame update

    
    void Start()
    {
        cur_weapon = 0;
        hp_counter.text = "1";
        num_bullets.text = infinity;
        abilityImage1.fillAmount = 1;
        player = GameObject.Find("Player");
        playerScript = player.GetComponent<PlayerMovement>();
        got_weapons = new bool[weapons.Length];
        got_weapons[0] = true;
        for(int i = 1; i < weapons.Length; i++)
        {
            got_weapons[i] = false;
            weapons[i].enabled = false;
        }
}

    // Update is called once per frame
    void Update()
    {
        //==============UI Health Potion Image Fill===========================================
        if (isCooldown)
        {
            abilityImage1.fillAmount += 1 / cooldown1 * Time.deltaTime;
            if (abilityImage1.fillAmount >= 1)
            {
                abilityImage1.fillAmount = 1;
                isCooldown = false;
            }
        }
        //=====================================================================================
        update_bullets();

    }

    public void drinkPotion()
    {
        if (isCooldown == false && playerScript.hpAmount >= 1 && playerScript.player_dead == false)
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
       

        hp_counter.text = playerScript.hpAmount.ToString();
    }







    public void get_new_weapon(int new_weapon)
    {
        for(int i = 1; i < got_weapons.Length; i++)
        {
            if (got_weapons[i]== true){
                got_weapons[i] = false;
                weapons[i].enabled = false;
            }
        }
        got_weapons[new_weapon] = true;
        sec_weapon = new_weapon;
        cur_weapon = new_weapon;
        
        weapons[new_weapon].enabled = true;
        weapons[0].enabled = false;
        update_bullets();


    }

    public void switch_weapon_icon()
    {
        if (weapons[0].enabled == false)
        {
            weapons[sec_weapon].enabled = false;
            weapons[0].enabled = true;
            cur_weapon = 0;
            update_bullets();
        }
        else
        {
            weapons[sec_weapon].enabled = true;
            weapons[0].enabled = false;
            cur_weapon = sec_weapon;
            update_bullets();
        }
    }


     void update_bullets()
    {
     num_bullets.text = playerScript.bullet_array[cur_weapon].ToString();
        
        
    }


    

}
