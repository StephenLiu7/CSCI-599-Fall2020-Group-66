using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Movement : MonoBehaviour
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

    private float attack_remain = 20;
    private float attack_range = 20;

    // Layer Info
    private readonly int DEFAULT_LAYER = 0;
    private readonly int MONSTER_LAYER = 9;
   
    private void OnCollisionEnter2D(Collision2D collision)
    {
        print("entered collider: "+collision.gameObject.tag);
        if (collision.gameObject.CompareTag("player_bullet"))
        {
            Destroy(collision.gameObject);
            attack_remain--;
            if (attack_remain == 0)
            {
                live = false;
                animator.Play("dead");
                Destroy(gameObject, 1f);
            }
        }
        else if(collision.gameObject.CompareTag("Player"))
        {
            isPaused = true;
            attack = true;
            //animator.Play("idle");
            animator.Play("tumanba_attack");
            //attack = false;
            
        }
    }

    private void Start()
    {
        live = true;
        attack_remain = 20;
        animator = gameObject.GetComponent<Animator>();

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
        // Movement of Player
        if(live && !isPaused){
            monster_rb.MovePosition(monster_rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }    
    }

    void Flip (){
         lookleft = !lookleft;
         Vector3 charscale = transform.localScale;
         charscale.x *= -1;
         transform.localScale = charscale;
     }
}
