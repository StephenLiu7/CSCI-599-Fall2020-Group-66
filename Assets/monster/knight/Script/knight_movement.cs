using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class knight_movement : MonoBehaviour
{
    public float moveSpeed = 1.8f;
    private bool slide = false;
    private float slideSpeed = 3f;
    private bool jump = false;
    private float jumpSpeed = 15f;

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
    public Rigidbody2D player_rb;

    private float attack_range = 16.73f;
    private bool out_bound = false;

    // Layer Info
    //private readonly int DEFAULT_LAYER = 0;
    //private readonly int MONSTER_LAYER = 9;

    // health info
    public int maxHealth = 20;
    public int currentHealth;
    public knight_health healthControl;

    // ability info
    private bool revive = true;
    private int revive_amount = 0;
    private bool reviving = false;

    // ability function
    private void Revive()
    {
        isPaused = true;
        animator.Play("Roll");
        revive = false;
        reviving = true;
    }

    private void Reviving()
    {
        if (currentHealth < maxHealth/5*3)
        {
            revive_amount++;
            if (revive_amount / 10 == 1)
            {
                currentHealth++;
                revive_amount = 0;
            }
            healthControl.SetHealth(currentHealth);
        }
        else{
            moveSpeed = moveSpeed * 2;
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
            if(reviving)
            {
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
            if (currentHealth < 0)
            {
                if (revive)
                {
                    Revive();
                }
                else
                {
                    live = false;
                    animator.Play("Death");
                    AnalyticsAPI.BossMonsterDeadCount++;
                    Destroy(gameObject, 1f);
                }
            }
            else if(!attack)
            {
                isPaused = true;
                animator.Play("TakeHit");
            }
        }
        else if(collision.gameObject.CompareTag("Player"))
        {
            isPaused = true;
            attack = true;
            animator.Play("Attack");
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

    private bool Range_Check()
    {
        float x = player_rb.position.x;
        float y = player_rb.position.y;
        if(x > -44 && y > -45)
        {
            return false;
        }
        return true;
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
            animator.Play("Idle");
            return;
        }

        if(distance < 1.7 && !attacking) // distance < 1.7
        {
            attacking = true;
            isPaused = true;
            attack = true;
            animator.Play("Attack");
        }
        if(attack && attack_time < 15){
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
            animator.Play("Run");
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
            animator.Play("Idle");
        }

    }

    void FixedUpdate()
    {
        Vector2 next_position = monster_rb.position + movement * moveSpeed * Time.fixedDeltaTime;
        if(next_position.x > 0 || next_position.y > -50)
        {
            out_bound = true;
            return;
        }
        out_bound = false;
        // Movement of Player
        if(slide && live){
            monster_rb.MovePosition(monster_rb.position + movement * slideSpeed * Time.fixedDeltaTime);
        }
        else if(jump && live){
            monster_rb.MovePosition(monster_rb.position + movement * jumpSpeed * Time.fixedDeltaTime);
        }
        else if(live && !isPaused){
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
