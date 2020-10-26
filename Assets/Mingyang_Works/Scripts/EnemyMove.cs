using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyMove : MonoBehaviour
{
    public float moveSpeed = .2f;

    public Animator animator;
    private bool live;
    private bool lookleft = true;
    private bool attack = false;
    private int attack_time = 0;

    // below are movement-related variables
    private bool isPaused;
    private Vector2 direction;
    Vector2 movement;

    public Rigidbody2D monster_rb;
    public Rigidbody2D player_rb;

    private float attack_range = 20;

    // Layer Info
    //private readonly int DEFAULT_LAYER = 0;
    //private readonly int MONSTER_LAYER = 9;

    // health info
    public int maxHealth = 20;
    public int currentHealth;
    public Health healthControl;

    // ability info

    private int jump = 5;

    // ability function

    private Transform playerPos;

    public GameObject drop;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        //print("entered collider: "+collision.gameObject.tag);

        if (collision.gameObject.CompareTag("player_bullet"))
        {
            Destroy(collision.gameObject);
            AnalyticsAPI.BossMonsterHitCount_static++;
            print(AnalyticsAPI.BossMonsterHitCount_static);
            TakeDamage(1);
            if (currentHealth <= 0)
            {

                    live = false;
                    animator.Play("dead");
                AnalyticsAPI.BossMonsterDeadCount++;
                Destroy(gameObject);
                Vector2 spawnPos = gameObject.transform.position;

                if (UnityEngine.Random.Range(0, 4) > 2)
                    Instantiate(drop, spawnPos, Quaternion.identity);

            }
            else if(!attack)
            {
            }
        }else if (collision.gameObject.CompareTag("player_missile"))
        {
            Destroy(collision.gameObject);
            AnalyticsAPI.BossMonsterHitCount_static++;
            print(AnalyticsAPI.BossMonsterHitCount_static);
            TakeDamage(3);
            if (currentHealth <= 0)
            {

                live = false;
                animator.Play("dead");
                AnalyticsAPI.BossMonsterDeadCount++;
                Destroy(gameObject);
                Vector2 spawnPos = gameObject.transform.position;

                if (UnityEngine.Random.Range(0, 4) > 2)
                    Instantiate(drop, spawnPos, Quaternion.identity);

            }
            else if (!attack)
            {
            }
        }
        else if (collision.gameObject.CompareTag("player_sniper"))
        {
            Destroy(collision.gameObject);
            AnalyticsAPI.BossMonsterHitCount_static++;
            print(AnalyticsAPI.BossMonsterHitCount_static);
            TakeDamage(4);
            if (currentHealth <= 0)
            {

                live = false;
                animator.Play("dead");
                AnalyticsAPI.BossMonsterDeadCount++;
                Destroy(gameObject);
                Vector2 spawnPos = gameObject.transform.position;
                if(UnityEngine.Random.Range(0, 4) > 2)
                    Instantiate(drop, spawnPos, Quaternion.identity);

            }
            else if (!attack)
            {
            }
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
        player_rb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
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

        float x_diff = player_rb.position.x-monster_rb.position.x;
        float y_diff = player_rb.position.y-monster_rb.position.y;
        float distance = (float)Math.Sqrt(x_diff * x_diff + y_diff * y_diff);
        if(attack && attack_time < 100){
            attack_time++;
            return;
        }
        else{
            attack = false;
            attack_time = 0;
        }

        if(distance < attack_range) {
            isPaused = false;
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
        }

    }

    void FixedUpdate()
    {
        // Movement of Player
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
