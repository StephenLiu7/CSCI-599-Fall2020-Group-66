using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class PlayerMovement : MonoBehaviour
{
    /// <summary>
    /// Initial parameter
    /// </summary>
    public UI_Control ui_control;
    //============================================= Character part  ===================================================================================
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator;
    Vector3 movement;
    //Vector2 moveDirec;
    //Vector2 lastMoveDirect;
    float facing = 1.0f; // -1 left, 1 right;
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;
    bool player_dead;
    // =================================================================================================================================================

    //============================================= initial Gun & shooting part  =======================================================================
    public Transform handgun;
    public Transform missile_gun;
    public Transform sniper_gun;

    public Transform cur_gun;
    public Transform cur_bullet;

    public Transform missile;
    public Transform handgun_bullet;
    public Transform sniper;

    
    double wait_time = 0.4;
    float lastClickTime;
    public float proTime = 0.0f;
    public float NextTime = 0.0f;
    /*struct gun
    {
        int bullet;
        string name;
        double split_time;
        double speed;
    };
    
    gun temp = new gun { 100, "handgun", 0.5, 11 };
    gun[] gun_array = new gun[] { temp, temp };

    Armory = [gun(100, handgun, handgun_bullet, "handgun", 0.5, 11), gun(100, handgun, handgun_bullet, "handgun", 0.5, 11), gun(100, handgun, handgun_bullet, "handgun", 0.5, 11)];
    Armory[0] = gun(int 100 , handgun,handgun_bullet,string "handgun", double 0.5,double 11);
    Armory[0] = gun(100,handgun,handgun_bullet,"handgun",0.5,11);
    */

    public int[] bullet_array = new int[] { 300, 6, 15 };      // handgun , missile , sniper
    string secondary_weapon = "";
    int ana_bullet_counting = 0;
    //bool LOR = false;       // initial facing right
    bool reported = false;

    void Start()
    {
        currentHealth = maxHealth; // set initial health
        healthBar.SetMaxHealth(maxHealth);
        player_dead = false;

        secondary_weapon = "";
    }

    //==============================================items===================================================================================================\
    public int hpAmount = 1;




    // Update is called once per frame
    // =================================================================================================================================================



    private void Awake()
    {
        missile_gun.gameObject.GetComponent<Renderer>().enabled = false;
        sniper_gun.gameObject.GetComponent<Renderer>().enabled = false;
    }

    void Update()
    {
        
        // *********test for health bar ********

        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentHealth -= 20;
            healthBar.SetHealth(currentHealth);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if(hpAmount < 5)
            {
                hpAmount += 1;
            }
        }

        //****************************************

        // =====================================================Character & shooting script =================================================================
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement.z = 0.0f;
        /*
        if((movement.x ==0 && movement.y == 0) && moveDirec.x != 0 || moveDirec.y !=0)
        {
            lastMoveDirect = moveDirec;
        }
        moveDirec = new Vector2(movement.x, movement.y).normalized;
        */
        //Vector3 theScale = cur_gun.localScale;
         
        if(movement.x > 0.001f)
        {
            
            facing = 1.0f;
        }
        else if(movement.x < -0.001f)
        {
            
            facing = -1.0f;
        }
      
        animator.SetFloat("Horizontal",facing);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Magnitude", movement.magnitude);
        animator.SetFloat("LastHori", facing);
        // =================================================================================================================================================

        // ================================================================ shooting time SEPA =============================================================
        
        if (player_dead == false)           // player need alive
        {
            //proTime = Time.fixedTime;
            FollowMouseRotate();
          
            if (Input.GetMouseButtonDown(1) && secondary_weapon.Length > 0)
            {
                ui_control.switch_weapon_icon();
                if (cur_bullet != handgun_bullet)
                {
                    wait_time = 0.7;
                    if (secondary_weapon == "sniper")
                    { sniper_gun.gameObject.GetComponent<Renderer>().enabled = false; }
                    else { missile_gun.gameObject.GetComponent<Renderer>().enabled = false; }
                    handgun.gameObject.GetComponent<Renderer>().enabled = true;
                    cur_bullet = handgun_bullet;
                    cur_gun = handgun;
                }
                else if (secondary_weapon == "sniper")
                {
                    wait_time = 1.2;
                    handgun.gameObject.GetComponent<Renderer>().enabled = false;
                    sniper_gun.gameObject.GetComponent<Renderer>().enabled = true;
                    cur_bullet = sniper;
                    cur_gun = sniper_gun;
                }
                else if (secondary_weapon == "missile")
                {
                    wait_time = 2.3;
                    missile_gun.gameObject.GetComponent<Renderer>().enabled = true;
                    handgun.gameObject.GetComponent<Renderer>().enabled = false;
                    cur_bullet = missile;
                    cur_gun = missile_gun;
                }

            }
            //Debug.Log(wait_time);
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
            {
                if (Time.time - lastClickTime >= wait_time || Time.time == lastClickTime)
                {
                    Shooting();
                    lastClickTime = Time.time;
                }
                
            }


            /*if (proTime - NextTime >= wait_time)
            {
                Shooting();
                NextTime = proTime;
            }*/
        }
        
        // =================================================================================================================================================
    }

    // =================================================================  CH update  =======================================================================
    void FixedUpdate()
    {
        //rb.MovePosition(rb.position + movement + moveSpeed * Time.fixedDeltaTime );
        if(player_dead == false)
        {
            transform.position = transform.position + movement * moveSpeed * Time.deltaTime;
        }
        
    }
    // =================================================================================================================================================

    // =============================================================== Shooting function ===================================================================
    // Below is Gun & Shooting
    private void FollowMouseRotate()
    {
        Vector3 mouse = Input.mousePosition;
       
        Vector3 obj = Camera.main.WorldToScreenPoint(cur_gun.position);
        Vector3 direction = obj - mouse;
        Vector3 theScale = cur_gun.localScale;
        if (facing == 1.0f)     // we have a flip
        {
            Debug.Log(theScale);
            if (theScale.x < 0)
            { 
                theScale.x *= -1 ;
                Vector2 diff = new Vector2(0.25f, 0);
                cur_gun.Translate(diff, Space.World);
            }
            cur_gun.localScale = theScale;
           
            direction = mouse - obj;
        }
        else if (facing == -1.0f)     // we have a flip
        {
            Debug.Log(theScale);
            if (theScale.x > 0)
            { 
                theScale.x *= -1 ;
                Vector2 diff = new Vector2(-0.25f, 0);
                cur_gun.Translate(diff, Space.World);
            }
            cur_gun.localScale = theScale;
            direction = obj - mouse;
                    // space world == absolute axis    Didn't change with object rotate
        }


        /*if (LOR == true)
        { direction = obj - mouse; }
        else { direction = mouse - obj; }*/
        direction.z = 0f;
        
        direction = direction.normalized;
        cur_gun.right = direction;
        
    }

    private void Shooting()
    {
        //if (Input.GetMouseButtonDown(0)) //|| Input.GetMouseButton(0))
        //{
        Vector3 bulletDirection = Input.mousePosition;
        Vector3 obj = Camera.main.WorldToScreenPoint(cur_gun.position);
        Vector3 direction = bulletDirection - obj;
        direction.Normalize();
        
        int speed = 0;
        bool Shoot_or_not = false;
        if (cur_bullet == missile && bullet_array[1] > 0)
        { 
            speed = 9;
            Shoot_or_not = true;
            bullet_array[1] -= 1;
        }
        else if (cur_bullet == handgun_bullet && bullet_array[0] > 0) 
        { 
            speed = 6;
            bullet_array[0] -= 1;  
            Shoot_or_not = true;
        }
        else if (cur_bullet == sniper && bullet_array[2] > 0) 
        { 
            speed = 15;
            bullet_array[2] -= 1;
            Shoot_or_not = true;
        }
        if (Shoot_or_not == true)
        {
            Transform bullet = Instantiate(cur_bullet, transform.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().velocity = direction * speed;
            bullet.transform.Rotate(0.0f, 0.0f, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
            Destroy(bullet.gameObject, 10.0f);
            ana_bullet_counting += 1;
        }
        //}
    }
    // =================================================================================================================================================

    //==========================================Health Bar Functions=======================================================================================
    void TakeDamage(int damage)
    {

        if (player_dead == false)
        {
            currentHealth -= damage;
            if (facing == 1.0f)
            {
                if (currentHealth <= 0)
                {
                    animator.SetBool("Dead_R", true);
                    player_dead = true;
                }
                else
                {
                    animator.SetTrigger("Hurt_R");
                }


            }

            else if (facing == -1.0f)
            {
                if (currentHealth <= 0)
                {
                    animator.SetBool("Dead_L", true);
                    player_dead = true;
                }
                else
                {
                    animator.SetTrigger("Hurt_L");
                }

            }
        }
        if (player_dead == true)
        {
            //Debug.Log(ana_bullet_counting);
            //Debug.Log(player_dead);
            GameObject g = GameObject.Find("Main Camera");
            int number = g.GetComponent<AnalyticsAPI>().BossMonsterHitCount;
            //Debug.Log(number);
            //print(ana_bullet_counting);
            
            double acc = (number / ana_bullet_counting) * 100;
            //Debug.Log(acc);
            if (reported == false)
            {
                reported = true;
                Analytics.CustomEvent("gameOver", new Dictionary<string, object>
                {
                    { "total bullets", ana_bullet_counting },
                    { "shooting accuracy", acc}

                });
            }

        }

    }


    //====================================================================================================================================================

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("missile_gun"))
        {
            Destroy(other.gameObject);
            
            //if (cur_gun == sniper_gun)
            //{
            wait_time = 2.3;
            sniper_gun.gameObject.GetComponent<Renderer>().enabled = false;
            handgun.gameObject.GetComponent<Renderer>().enabled = false;
            missile_gun.gameObject.GetComponent<Renderer>().enabled = true;
            cur_gun = missile_gun;
            cur_bullet = missile;
            //}
            secondary_weapon = "missile";
            ui_control.get_new_weapon(1);
            bullet_array[1] = 6;
        }
        if (other.gameObject.CompareTag("sniper_gun"))
        {
            Destroy(other.gameObject);
            
            //if (cur_gun == missile_gun)
            //{
            wait_time = 1.2;
            handgun.gameObject.GetComponent<Renderer>().enabled = false;
            sniper_gun.gameObject.GetComponent<Renderer>().enabled = true;
            missile_gun.gameObject.GetComponent<Renderer>().enabled = false;
            cur_gun = sniper_gun;
            cur_bullet = sniper;
            //}
            secondary_weapon = "sniper";
            ui_control.get_new_weapon(2);
            bullet_array[2] = 15;
        }

        if (other.gameObject.CompareTag("unpaused_bullet") || other.gameObject.CompareTag("paused_bullet") 
         || other.gameObject.CompareTag("unpaused_bullet") || other.gameObject.CompareTag("enemy_1") || other.gameObject.CompareTag("enemy_bullet_1"))
        {
            TakeDamage(10);
            healthBar.SetHealth(currentHealth);
        }

        if (other.gameObject.CompareTag("item_hp"))
        {
            if(hpAmount < 5)
            {
                hpAmount += 1;
            }
            Destroy(other.gameObject);
        }


    }






}
