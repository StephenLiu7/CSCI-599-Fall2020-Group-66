using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Pathfinding;

public class EnemyPinkman : MonoBehaviour
{
    // A* path finding
    public float nextWaypointDistance = 1.5f;
    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;
    Seeker seeker;

    public float speed;
    public float stoppingDistance;
    public float retreatDistance;

    private float timeBtShots;
    public float startTimeBtShots;

    public GameObject WeaponSaw;
    private Transform player;


    public Rigidbody2D monster_rb;
    public Rigidbody2D player_rb;

    // health info
    public int maxHealth = 20;
    public int currentHealth;
    public Health healthControl;

    private bool live;

    public GameObject drop;

    private float attack_range = 20;

    private bool isPaused;




    // Start is called before the first frame update
    void Start()
    {
        player_rb = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // A* component
        seeker = GetComponent<Seeker>();
        monster_rb = GetComponent<Rigidbody2D>();
        InvokeRepeating("UpdatePath", 0f, .5f);

        timeBtShots = startTimeBtShots;

        currentHealth = maxHealth;
        healthControl.SetMaxHealth(maxHealth);
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
    void FixedUpdate()
    {
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
        Vector2 force = direction * speed * Time.deltaTime;


        if (Vector2.Distance(transform.position, player.position) < attack_range)
        {
            if (Vector2.Distance(transform.position, player.position) > stoppingDistance)
            {
                // now we move
                monster_rb.AddForce(force);
                float distance = Vector2.Distance(monster_rb.position, path.vectorPath[currentWaypoint]);
                if(distance < nextWaypointDistance)
                {
                    currentWaypoint++;
                }
                //transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            }
            else if (Vector2.Distance(transform.position, player.position) < stoppingDistance && Vector2.Distance(transform.position, player.position) > retreatDistance)
            {
                transform.position = this.transform.position;
            }
            else if (Vector2.Distance(transform.position, player.position) < retreatDistance)
            {
                // still move
                monster_rb.AddForce(force);
                float distance = Vector2.Distance(monster_rb.position, path.vectorPath[currentWaypoint]);
                if(distance < nextWaypointDistance)
                {
                    currentWaypoint++;
                }
                //transform.position = Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
            }

            if (timeBtShots <= 0)
            {
                Instantiate(WeaponSaw, transform.position, Quaternion.identity);
                timeBtShots = startTimeBtShots;

            }
            else
            {

                timeBtShots -= Time.deltaTime;
            }
        }
        

    }

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
                AnalyticsAPI.BossMonsterDeadCount++;
                Destroy(gameObject, 0.1f);
                Vector2 spawnPos = gameObject.transform.position;

                if (UnityEngine.Random.Range(0, 4) >= 0)
                    Instantiate(drop, spawnPos, Quaternion.identity);

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
                AnalyticsAPI.BossMonsterDeadCount++;
                Destroy(gameObject, 0.1f);
                Vector2 spawnPos = gameObject.transform.position;

                if (UnityEngine.Random.Range(0, 4) >= 0)
                    Instantiate(drop, spawnPos, Quaternion.identity);


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
                AnalyticsAPI.BossMonsterDeadCount++;
                Destroy(gameObject, 0.1f);
                Vector2 spawnPos = gameObject.transform.position;

                if (UnityEngine.Random.Range(0, 4) >= 0)
                    Instantiate(drop, spawnPos, Quaternion.identity);

            }

        }

    }

    private void TakeDamage(int damge)
    {
        currentHealth -= damge;
        healthControl.SetHealth(currentHealth);
    }
}
