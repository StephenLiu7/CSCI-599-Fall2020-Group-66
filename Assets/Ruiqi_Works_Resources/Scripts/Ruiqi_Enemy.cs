using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ruiqi_Enemy : MonoBehaviour
{
    // Movement Related Variables
    public float speed = 2.0f;
    public float stoppingDistance;
    public float retreatDistance;
    public float maxFollowDistance;
    
    //
    private float timeBtwShots;
    public float startTimeBtwShots;
    
    public GameObject bullet;
    private Transform player;
    public float attackRange = 7.0f;

    // Health Bar Related Variables
    public int maxLife = 5;
    private int lifeRemain = 5;
    public Ruiqi_Enemy_Health Healthbar;
    
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
        timeBtwShots = startTimeBtwShots;
        Healthbar.SetMaxHealth(maxLife);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, player.position) > stoppingDistance 
        && Vector2.Distance(transform.position, player.position) <= maxFollowDistance) {

            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
        else if (Vector2.Distance(transform.position, player.position) < stoppingDistance && Vector2.Distance(transform.position, player.position) > retreatDistance) {

            transform.position = this.transform.position;
        }
        else if (Vector2.Distance(transform.position, player.position) < retreatDistance) {

            transform.position = Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
        }
        
        // Shoot
        if (timeBtwShots <= 0 && Vector2.Distance(transform.position, player.position) <= attackRange) {
            Instantiate(bullet, transform.position, Quaternion.identity);
            timeBtwShots = startTimeBtwShots;
        }
        else {
            timeBtwShots -= Time.deltaTime;
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("player_bullet"))
        {
            Destroy(collision.gameObject);
            lifeRemain--;
            Healthbar.SetHealth(lifeRemain);
            AnalyticsAPI.BossMonsterHitCount_static++;
            if (lifeRemain == 0)
            {
                Destroy(gameObject);
                AnalyticsAPI.BossMonsterDeadCount++;
            }
        }
    }
}
