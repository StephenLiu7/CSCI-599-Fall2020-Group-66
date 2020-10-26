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
    public static float survivalTimes;

    //============================================= Joystick part  ===================================================================================
    public float speed = 5f;
    //public Rigidbody2D player_joy;
    Vector2 player_moves;
    Vector2 weapon_moves;
    public FloatingJoystick player_joystick;

    public FloatingJoystick weapon_joystick;
    public RectTransform weapon_joystick_background;
    private bool weapon_fire = false;
    private Vector3 direction;
    //================================================================================================================================================




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
    public bool player_dead;
    public int lostbytoxic = 0;
    public int lostbymonster = 0;
    public int item_used = 0;
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

    public float acc = 0;
    double wait_time = 0.4;
    float lastClickTime;
    public float proTime = 0.0f;
    public float NextTime = 0.0f;

    public int hpAmount = 1;

    
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

    public int[] bullet_array = new int[] { 300, 0, 0 };      // handgun , missile , sniper
    public int[] weapon_using_time = new int[] { 0, 0, 0 };
    string secondary_weapon = "";
    public int ana_bullet_counting = 0;
    public string most_use = "";
    //bool LOR = false;       // initial facing right
    bool reported = false;

    void Start()
    {

        (float, float)[] positions = { (-100.0f, 50.0f), (-49.0f, 113.0f), (104.0f, 5.0f), (-87.3f, -94.8f) };
        (float, float) randPos = positions[Random.Range(0, positions.Length)];
        gameObject.transform.position = new Vector3(randPos.Item1, randPos.Item2, 0);
        maxHealth = 100;
        currentHealth = maxHealth; // set initial health
        healthBar.SetMaxHealth(maxHealth);
        player_dead = false;

        secondary_weapon = "";
        InvokeRepeating("circleDamage", 0.0f, 2.0f);
        survivalTimes = 0;
        lastClickTime = 0;
    }
    //==============================================items===================================================================================================\





    // Update is called once per frame
    // =================================================================================================================================================



    private void Awake()
    {
        missile_gun.gameObject.GetComponent<Renderer>().enabled = false;
        sniper_gun.gameObject.GetComponent<Renderer>().enabled = false;
    }

    void Update()
    {
        // ********* Joystick ********


        player_moves.x = player_joystick.Horizontal;
        player_moves.y = player_joystick.Vertical;

        Debug.Log("Player Move: " + player_moves);

        weapon_moves.x = weapon_joystick.Horizontal;
        weapon_moves.y = weapon_joystick.Vertical;


        // *********test for health bar ********
        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentHealth -= 20;
            healthBar.SetHealth(currentHealth);
            if (currentHealth < 0)
            {
                player_dead = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (hpAmount < 5)
            {
                hpAmount += 1;
            }
        }
        */
        //****************************************

        // =====================================================Character & shooting script =================================================================

        if (player_moves.magnitude <= 0)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
            movement.z = 0.0f;
        }

        /*
        if((movement.x ==0 && movement.y == 0) && moveDirec.x != 0 || moveDirec.y !=0)
        {
            lastMoveDirect = moveDirec;
        }
        moveDirec = new Vector2(movement.x, movement.y).normalized;
        */
        //Vector3 theScale = cur_gun.localScale;

        if (movement.x > 0.001f || player_moves.x > 0.001f)
        {
            
            facing = 1.0f;
        }
        else if (movement.x < -0.001f || player_moves.x < -0.001f)
        {
            facing = -1.0f;
        }

        if (weapon_moves.x > 0.001f)
        {

            facing = 1.0f;

        }
        if(weapon_moves.x < -0.001f){

            facing = -1.0f;

        }

        animator.SetFloat("Horizontal", facing);
        animator.SetFloat("Vertical", player_moves.y);
        animator.SetFloat("Magnitude", player_moves.magnitude);
        if (player_moves.magnitude <= 0)
        {
            animator.SetFloat("Vertical", movement.y);
            animator.SetFloat("Magnitude", movement.magnitude);
        }

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
                    wait_time = 0.5;
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
            /*if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
            {
                if (Time.time - lastClickTime >= wait_time || Time.time == lastClickTime)
                {
                    Shooting();
                    lastClickTime = Time.time;
                }
                
            }


            if (proTime - NextTime >= wait_time)
            {
                Shooting();
                NextTime = proTime;
            }
            For PC
            */

            /*
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.collider.gameObject.name);
            }
            */

            //if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))


            detectingShooting();
           
            cancelShooting();
            Debug.Log("Horizontal: " + weapon_joystick.Horizontal);
        }

        // =================================================================================================================================================
    }

    // =================================================================  CH update  =======================================================================
    void FixedUpdate()
    {
        //rb.MovePosition(rb.position + movement + moveSpeed * Time.fixedDeltaTime );
        if (player_dead == false)
        {
            transform.position = transform.position + movement * moveSpeed * Time.deltaTime;
        }

        rb.MovePosition(rb.position + player_moves * speed * Time.deltaTime);

    }
    // =================================================================================================================================================



    
    public void switch_weapon()
    {
        if (secondary_weapon.Length > 0)
        {
            ui_control.switch_weapon_icon();
            if (cur_bullet != handgun_bullet)
            {
                wait_time = 0.5;
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
                wait_time = 2.0;
                missile_gun.gameObject.GetComponent<Renderer>().enabled = true;
                handgun.gameObject.GetComponent<Renderer>().enabled = false;
                cur_bullet = missile;
                cur_gun = missile_gun;
            }

        }
    }





    // =============================================================== Shooting function ===================================================================
    // Detecting shooting inputs
    private void detectingShooting()
    {
        if (weapon_joystick.Horizontal >= 0.8 || weapon_joystick.Horizontal <= -0.8 || weapon_joystick.Vertical >= 0.8 || weapon_joystick.Vertical <= -0.8)
        {
            weapon_fire = true;
            
        }

        if (weapon_moves.magnitude > 0)
        {
            direction = new Vector3(weapon_moves.x, weapon_moves.y, Camera.main.transform.position.z);
            direction.Normalize();
        }


        if (weapon_joystick_background.gameObject.activeSelf && weapon_fire)
        {
            
            if (Time.time - lastClickTime >= wait_time || Time.time == lastClickTime)
            {
                Shooting(direction);
                lastClickTime = Time.time;
            }

        }


        
    }

    private void cancelShooting()
    {

        if (weapon_joystick.Horizontal < 0.8 || weapon_joystick.Horizontal > -0.8 && weapon_joystick.Vertical < 0.8 && weapon_joystick.Vertical > -0.8)
        {
            weapon_fire = false;
        }


    }



    // Below is Gun & Shooting
    private void FollowMouseRotate()
    {
        //Vector3 mouse = Input.mousePosition;                                                      // this only for PC
        Vector3 mouse = new Vector3(weapon_moves.x, weapon_moves.y, Camera.main.transform.position.z);
        Vector3 obj = Camera.main.WorldToScreenPoint(cur_gun.position);
        //Vector3 direction = obj - mouse;                                                          // this only for PC
        Vector3 direction = mouse;
        Vector3 theScale = cur_gun.localScale;
        if (facing == 1.0f)     // we have a flip
        {
            //Debug.Log(theScale);
            if (theScale.x < 0)
            {
                theScale.x *= -1;
                Vector2 diff = new Vector2(0.25f, 0);
                cur_gun.Translate(diff, Space.World);
            }
            cur_gun.localScale = theScale;

            //direction = mouse - obj;                                                              // this only for PC
            direction = mouse;
        }
        else if (facing == -1.0f)     // we have a flip
        {
            //Debug.Log(theScale);
            if (theScale.x > 0)
            {
                theScale.x *= -1;
                Vector2 diff = new Vector2(-0.25f, 0);
                cur_gun.Translate(diff, Space.World);
            }
            cur_gun.localScale = theScale;
            //direction = obj - mouse;                                                              // this only for PC
            direction = -mouse;
            // space world == absolute axis    Didn't change with object rotate
        }


        /*if (LOR == true)
        { direction = obj - mouse; }
        else { direction = mouse - obj; }*/
        direction.z = 0f;

        direction = direction.normalized;
        cur_gun.right = direction;

    }

    private void Shooting(Vector3 direction)
    {
        //if (Input.GetMouseButtonDown(0)) //|| Input.GetMouseButton(0))
        //{
        /* PC code
         * 
         * Vector3 bulletDirection = Input.mousePosition;
        Vector3 obj = Camera.main.WorldToScreenPoint(cur_gun.position);
        Vector3 direction = bulletDirection - obj;
         */



        int shooting_speed = 0;
        bool Shoot_or_not = false;
        if (cur_bullet == missile && bullet_array[1] > 0)
        {
            shooting_speed = 110;
            Shoot_or_not = true;
            bullet_array[1] -= 1;
            weapon_using_time[1] += 1;
        }
        else if (cur_bullet == handgun_bullet && bullet_array[0] > 0)
        {
            shooting_speed = 80;
            bullet_array[0] -= 1;
            Shoot_or_not = true;
            weapon_using_time[0] += 1;
        }
        else if (cur_bullet == sniper && bullet_array[2] > 0)
        {
            shooting_speed = 150;
            bullet_array[2] -= 1;
            Shoot_or_not = true;
            weapon_using_time[2] += 1;
        }
        if (Shoot_or_not == true)
        {
            Transform bullet = Instantiate(cur_bullet, transform.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().velocity = direction * shooting_speed;
            bullet.transform.Rotate(0.0f, 0.0f, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
            Destroy(bullet.gameObject, 1.5f);
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


            GameObject g = GameObject.Find("Main Camera");
            int number = AnalyticsAPI.BossMonsterHitCount_static;

            if (ana_bullet_counting > 0)
            { acc = (AnalyticsAPI.BossMonsterHitCount_static * 100) / ana_bullet_counting; }
            most_use = "";

            if (weapon_using_time[0] >= weapon_using_time[1] && weapon_using_time[0] >= weapon_using_time[2])
            { most_use = "handgun"; }
            else if (weapon_using_time[1] >= weapon_using_time[2])
            { most_use = "RPG"; }
            else { most_use = "Sniper"; }

            if (reported == false)
            {
                reported = true;
                float times = Time.fixedTime;
                survivalTimes = Time.fixedTime;
                Analytics.CustomEvent("char", new Dictionary<string, object>
                {
                    //{ "total bullets", ana_bullet_counting },
                    //{ "shooting accuracy", acc},
                    //{ "Monster killed" , AnalyticsAPI.BossMonsterDeadCount },
                    //{ "Shooting on target" , AnalyticsAPI.BossMonsterHitCount_static },
                    //{ "most use weapon",  most_use},
                    { "Survival Time", survivalTimes },
                    { "Health lost by toxic gas", lostbytoxic },
                    { "Health lost by monster", lostbymonster}
                });
                Analytics.CustomEvent("weapon", new Dictionary<string, object>
                {
                    { "total bullets", ana_bullet_counting },
                    { "shooting accuracy", acc},
                    //{ "Monster killed" , AnalyticsAPI.BossMonsterDeadCount },
                    //{ "Shooting on target" , AnalyticsAPI.BossMonsterHitCount_static },
                    { "most use weapon",  most_use},
                    //{ "Survival Time", times }
                });
                Analytics.CustomEvent("monster", new Dictionary<string, object>
                {
                    //{ "total bullets", ana_bullet_counting },
                    //{ "shooting accuracy", acc},
                    { "Monster killed" , AnalyticsAPI.BossMonsterDeadCount },
                    { "Shooting on target" , AnalyticsAPI.BossMonsterHitCount_static },
                    //{ "most use weapon",  most_use},
                    //{ "Survival Time", times }
                });
                string s1 = string.Format("Survival Time : {0}, Health lost by toxic gas : {1} , Health lost by monster : {2} , total bullets : {3} , shooting accuracy : {4} , most use weapon : {5} , Monster killed : {6} , Shooting on target : {7}", survivalTimes , lostbytoxic , lostbymonster , ana_bullet_counting , acc , most_use , AnalyticsAPI.BossMonsterDeadCount , AnalyticsAPI.BossMonsterHitCount_static);
            //    string s2 = string.Format("Health lost by toxic gas{0}", lostbytoxic);
            //    string s3 = string.Format("Health lost by monster{0}", lostbymonster);
            //    string s4 = string.Format("total bullets{0}", ana_bullet_counting);
            //    string s5 = string.Format("shooting accuracy{0}", acc);
            //    string s6 = string.Format("most use weapon{0}", most_use);
            //    string s7 = string.Format("Monster killed{0}", AnalyticsAPI.BossMonsterDeadCount);
            //    string s8 = string.Format("Shooting on target{0}", AnalyticsAPI.BossMonsterHitCount_static);
                print(s1);
            }
        }
    }


    private void circleDamage()
    {
        if (DamageCircle.IsOutsideCircle_Static(GameObject.Find("Player").transform.position))
        {
            currentHealth -= 10;
            lostbytoxic += 10;
            healthBar.SetHealth(currentHealth);
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
            if (cur_gun == missile_gun)
            { bullet_array[1] += 6; }
            else { bullet_array[1] = 6; }
            sniper_gun.gameObject.GetComponent<Renderer>().enabled = false;
            handgun.gameObject.GetComponent<Renderer>().enabled = false;
            missile_gun.gameObject.GetComponent<Renderer>().enabled = true;
            cur_gun = missile_gun;
            cur_bullet = missile;
            //}
            secondary_weapon = "missile";
            ui_control.get_new_weapon(1);
            
            //bullet_array[1] += 6;
        }
        if (other.gameObject.CompareTag("sniper_gun"))
        {
            Destroy(other.gameObject);

            //if (cur_gun == missile_gun)
            //{
            wait_time = 1.2;
            if (cur_gun == sniper_gun)
            { bullet_array[2] += 15; }
            else { bullet_array[2] = 15; }
            handgun.gameObject.GetComponent<Renderer>().enabled = false;
            sniper_gun.gameObject.GetComponent<Renderer>().enabled = true;
            missile_gun.gameObject.GetComponent<Renderer>().enabled = false;
            cur_gun = sniper_gun;
            cur_bullet = sniper;
            //}
            secondary_weapon = "sniper";
            ui_control.get_new_weapon(2);
            
        }

        if (other.gameObject.CompareTag("unpaused_bullet") || other.gameObject.CompareTag("paused_bullet")
         || other.gameObject.CompareTag("unpaused_bullet") || other.gameObject.CompareTag("enemy_1") || other.gameObject.CompareTag("enemy_bullet_1"))
        {
            TakeDamage(10);
            lostbymonster += 10;
            healthBar.SetHealth(currentHealth);
        }

        if (other.gameObject.CompareTag("item_hp"))
        {
            if (hpAmount < 5)
            {
                hpAmount += 1;
            }
            Destroy(other.gameObject);
        }


        if (other.gameObject.CompareTag("item_heal"))
        {
            
           
            currentHealth += 50;
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
            healthBar.SetHealth(currentHealth);
            Destroy(other.gameObject);

        }


    }





}
