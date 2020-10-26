using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Movement : MonoBehaviour
{
    public float moveSpeed = 2f;

    public Animator animator;
    private bool live;
    private bool lookleft = false;
    private bool attack = false;
    private int attack_time = 0;

    // below are movement-related variables
    private bool attacking;
    private bool isPaused;
    private Vector2 direction;
    Vector2 movement;

    public Rigidbody2D monster_rb;
    public Rigidbody2D player_rb;//= GameObject.Find("Player").GetComponent<Rigidbody2D>();

    private float attack_range = 16.73f;
    private bool out_bound = false;

    // Layer Info
    //private readonly int DEFAULT_LAYER = 0;
    //private readonly int MONSTER_LAYER = 9;

    // health info
    public int maxHealth = 20;
    public int currentHealth;
    public Health healthControl;

    // ability info
    private bool revive = true;
    private int revive_amount = 0;
    private bool reviving = false;

    // ability function
    private void Revive()
    {
        isPaused = true;
        animator.Play("tumanba_revive");
        revive = false;
        reviving = true;
    }

    private void Reviving()
    {
        if (currentHealth < maxHealth/5*3)
        {
            revive_amount++;
            if (revive_amount / 5 == 1)
            {
                currentHealth++;
                revive_amount = 0;
            }
            healthControl.SetHealth(currentHealth);
        }
        else{
            moveSpeed = moveSpeed * 3;
            reviving = false;
        }
    }
   
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //print("entered collider: "+collision.gameObject.tag);
        if (collision.gameObject.CompareTag("player_bullet") || collision.gameObject.CompareTag("player_missile") || collision.gameObject.CompareTag("player_sniper"))
        {
            AnalyticsAPI.BossMonsterHitCount_static++;
            Destroy(collision.gameObject);
            if (reviving){
                return;
            }
            if(collision.gameObject.CompareTag("player_missile"))
            {
                TakeDamage(2);
            }
            else if(collision.gameObject.CompareTag("player_sniper"))
            {
                TakeDamage(4);
            }
            else
            {
                TakeDamage(1);
            }
            if (currentHealth <= 0)
            {
                if (revive)
                {
                    Revive();
                }
                else
                {
                    live = false;
                    animator.Play("dead");
                    AnalyticsAPI.BossMonsterDeadCount++;
                    Destroy(gameObject, 1f);
                    
                    GameObject reward = GameObject.Find("ammo"); //xiaowen_pill
                    Vector2 rewardPos = gameObject.transform.position;
                    Instantiate(reward, rewardPos, Quaternion.identity);

                    /*
                    GameObject reward = GameObject.Find("ammo");
                    Vector2 rewardPos = gameObject.transform.position;
                    Instantiate(reward, rewardPos, Quaternion.identity);
                    */
                }
            }
            else if(!attack)
            {
                animator.Play("tumanba_hit");
            }
        }
        else if(collision.gameObject.CompareTag("Player"))
        {
            isPaused = true;
            attack = true;
            animator.Play("tumanba_attack");
        }
    }

    // damage control
    private void TakeDamage(int damge) {
        currentHealth -= damge;
        healthControl.SetHealth(currentHealth);
    }

    // initialization
    private void Start()
    {
        player_rb = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        live = true;
        animator = gameObject.GetComponent<Animator>();

        currentHealth = maxHealth;
        healthControl.SetMaxHealth(maxHealth);

        // calculate initial direction
        float x_diff = player_rb.position.x-monster_rb.position.x;
        float y_diff = player_rb.position.y-monster_rb.position.y;
        float distance = (float)Math.Sqrt(x_diff * x_diff + y_diff * y_diff);
        direction = new Vector2(x_diff / distance, y_diff / distance);
        if(direction.x < 0) {
            Flip();
        }
        movement = direction;
        if (distance < attack_range) {
            isPaused = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!live) {
            return;
        }
        if (reviving) {
            Reviving();
            return;
        }
        
        float x_diff = player_rb.position.x-monster_rb.position.x;
        float y_diff = player_rb.position.y-monster_rb.position.y;
        float distance = (float)Math.Sqrt(x_diff * x_diff + y_diff * y_diff);
        direction = new Vector2(x_diff / distance, y_diff / distance);
        movement = direction;
        
        // check if out of region
        if (out_bound)
        {
            isPaused = true;
            animator.Play("tumanba_idle");
            return;
        }

        if(distance < 1.2 && !attacking)
        {
            attacking = true;
            isPaused = true;
            attack = true;
            animator.Play("tumanba_attack");
        }
        if(attack && attack_time < 50){
            attack_time++;
            return;
        }
        else{
            attacking = false;
            attack = false;
            attack_time = 0;
        }

        if(distance < attack_range && !attacking) {
            isPaused = false;
            animator.Play("tumanba_run");
            direction = new Vector2(x_diff / distance, y_diff / distance);
            if(direction.x < 0 && !lookleft) {
                Flip();
            }
            else if(direction.x > 0 && lookleft) {
                Flip();
            }
            movement = direction;
        }
        else {
            isPaused = true;
            animator.Play("tumanba_idle");
        }

    }

    void FixedUpdate()
    {
        Vector2 next_position = monster_rb.position + movement * moveSpeed * Time.fixedDeltaTime;
        if(next_position.x > 0 || next_position.y > -48)
        {
            out_bound = true;
            return;
        }
        // Movement of Player
        if(out_bound)
        {
            isPaused = false;
        }
        out_bound = false;
        if(live && !isPaused){
            monster_rb.MovePosition(monster_rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }    
    }

    // direction fixed
    void Flip (){
         lookleft = !lookleft;
         Vector3 charscale = transform.localScale;
         charscale.x *= -1;
         transform.localScale = charscale;
     }
}
