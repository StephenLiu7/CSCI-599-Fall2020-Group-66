using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossMove : MonoBehaviour
{
    public float moveSpeed = .2f;

    public Animator animator;
    private bool live;
    private bool lookleft = false;
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
    private bool revive = true;
    private int revive_amount = 0;
    private bool reviving = false;
    private int jump = 5;

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
        if (currentHealth < maxHealth / 5 * 3)
        {
            revive_amount++;
            if (revive_amount / 10 == 1)
            {
                currentHealth++;
                revive_amount = 0;
            }
            healthControl.SetHealth(currentHealth);
        }
        else
        {
            moveSpeed = moveSpeed * 5;
            reviving = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //print("entered collider: "+collision.gameObject.tag);
        if (reviving)
        {
            Destroy(collision.gameObject);
            return;
        }
        if (collision.gameObject.CompareTag("player_bullet"))
        {
            Destroy(collision.gameObject);
            GameObject g = GameObject.Find("Main Camera");
            g.GetComponent<AnalyticsAPI>().BossMonsterHitCount++;
            print(g.GetComponent<AnalyticsAPI>().BossMonsterHitCount);
            TakeDamage(1);
            if (currentHealth == 0)
            {
                if (revive)
                {
                    Revive();
                }
                else
                {
                    live = false;
                    animator.Play("dead");
                    Destroy(gameObject, 1f);
                }
            }
            else if (!attack)
            {
                animator.Play("hit");
            }
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            isPaused = true;
            attack = true;
            //animator.Play("idle");
            //attack = false;

        }
    }

    // damage control
    private void TakeDamage(int damge)
    {
        currentHealth -= damge;
        healthControl.SetHealth(currentHealth);
    }

    // initialization
    private void Start()
    {
        live = true;
        animator = gameObject.GetComponent<Animator>();

        currentHealth = maxHealth;
        healthControl.SetMaxHealth(maxHealth);

        // calculate initial direction
        float x_diff = player_rb.position.x - monster_rb.position.x;
        float y_diff = player_rb.position.y - monster_rb.position.y;
        float distance = (float)Math.Sqrt(x_diff * x_diff + y_diff * y_diff);
        direction = new Vector2(x_diff / distance, y_diff / distance);
        if (direction.x < 0)
        {
            Flip();
        }
        movement = direction;
        if (distance < attack_range)
        {
            isPaused = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!live)
        {
            return;
        }
        if (reviving)
        {
            Reviving();
            return;
        }
        float x_diff = player_rb.position.x - monster_rb.position.x;
        float y_diff = player_rb.position.y - monster_rb.position.y;
        float distance = (float)Math.Sqrt(x_diff * x_diff + y_diff * y_diff);
        if (attack && attack_time < 100)
        {
            attack_time++;
            return;
        }
        else
        {
            attack = false;
            attack_time = 0;
        }

        if (distance < attack_range)
        {
            isPaused = false;
            animator.Play("tumanba_run");
            direction = new Vector2(x_diff / distance, y_diff / distance);
            if (direction.x < 0 && !lookleft)
            {
                Flip();
            }
            else if (direction.x > 0 && lookleft)
            {
                Flip();
            }
            movement = direction;
        }
        else
        {
            isPaused = true;
            animator.Play("tumanba_idle");
        }

    }

    void FixedUpdate()
    {
        // Movement of Player
        if (live && !isPaused)
        {
            monster_rb.MovePosition(monster_rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }

    // direction fixed
    void Flip()
    {
        lookleft = !lookleft;
        Vector3 charscale = transform.localScale;
        charscale.x *= -1;
        transform.localScale = charscale;
    }
}
