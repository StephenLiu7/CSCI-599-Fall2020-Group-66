using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Pathfinding;

public class SkeletonMove : MonoBehaviour
{
    public float moveSpeed = 200f;

    // A* path finding
    public float nextWaypointDistance = 1.5f;
    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;
    Seeker seeker;

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
    private List<string> Attacks = new List<string> { "SkeletonAttack", "SkeletonAttack2" };
    // ability info

    private int jump = 5;

    // ability function
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
                animator.Play("SkeletonDeath");
                AnalyticsAPI.BossMonsterDeadCount++;
                Destroy(gameObject, 0.2f);
                Vector2 spawnPos = gameObject.transform.position;

                if (UnityEngine.Random.Range(0, 4) >= 1)
                    Instantiate(drop, spawnPos, Quaternion.identity);

            }
            else if (!attack)
            {
            }
        }
        else if (collision.gameObject.CompareTag("player_missile"))
        {
            Destroy(collision.gameObject);
            AnalyticsAPI.BossMonsterHitCount_static++;
            print(AnalyticsAPI.BossMonsterHitCount_static);
            TakeDamage(3);
            if (currentHealth <= 0)
            {

                live = false;
                animator.Play("SkeletonDeath");
                AnalyticsAPI.BossMonsterDeadCount++;
                Destroy(gameObject, 0.2f);
                Vector2 spawnPos = gameObject.transform.position;

                if (UnityEngine.Random.Range(0, 4) >= 1)
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
                animator.Play("SkeletonDeath");
                AnalyticsAPI.BossMonsterDeadCount++;
                Destroy(gameObject, 0.2f);
                Vector2 spawnPos = gameObject.transform.position;

                if (UnityEngine.Random.Range(0, 4) >= 1)
                    Instantiate(drop, spawnPos, Quaternion.identity);

            }
            else if (!attack)
            {
            }
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            isPaused = true;
            attack = true;
            //animator.Play("GhostIdle");
            animator.Play(Attacks[UnityEngine.Random.Range(0, 2)]);
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
        player_rb = GameObject.Find("Player").GetComponent<Rigidbody2D>();

        // A* component
        seeker = GetComponent<Seeker>();
        monster_rb = GetComponent<Rigidbody2D>();
        InvokeRepeating("UpdatePath", 0f, .5f);

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

    void UpdatePath()
    {
        float x_diff = player_rb.position.x-monster_rb.position.x;
        float y_diff = player_rb.position.y-monster_rb.position.y;
        float distance = (float)Math.Sqrt(x_diff * x_diff + y_diff * y_diff);
        if(distance > attack_range)
            return;
        if(seeker.IsDone())
            seeker.StartPath(monster_rb.position, player_rb.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!live)
        {
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
            animator.Play("SkeletonWalk");
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
            animator.Play("SkeletonIdle");
        }

    }

    void FixedUpdate()
    {
        if(!live || isPaused)
            return;

        // A* path finding
        if(path == null)
            return;
        if(currentWaypoint > path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - monster_rb.position).normalized;
        Vector2 force = direction * moveSpeed * Time.deltaTime;
        monster_rb.AddForce(force);
        float distance = Vector2.Distance(monster_rb.position, path.vectorPath[currentWaypoint]);
        if(distance < nextWaypointDistance)
        {
            currentWaypoint++;
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
