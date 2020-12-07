using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ruiqi_Hornet : MonoBehaviour
{
    // Movement Related Variables
    public float speed = 2.0f;
    public float stoppingDistance;
    public float retreatDistance;
    public float maxFollowDistance;
    private bool leftward = true;
    private Vector2 dir;
    
    // Shooting Related Variables
    private float timeBtwShots;
    public float startTimeBtwShots;
    
    public GameObject bullet;
    private Transform player;
    public float attackRange = 10.0f;

    // Health Bar Related Variables
    public int maxLife = 20;
    private int lifeRemain = 20;
    public Ruiqi_Hornet_Health Healthbar;
    
    // Drop Related Variables
    public GameObject drop;
    
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
        var plyPosition = player.position;
        var horPosition = transform.position;
        var xDiff = plyPosition.x - horPosition.x;
        var yDiff = plyPosition.y - horPosition.y;
        var dis = (float) Math.Sqrt(xDiff * xDiff + yDiff * yDiff);
        dir = new Vector2(xDiff/dis, yDiff/dis);
        if (Vector2.Distance(transform.position, player.position) > stoppingDistance 
        && Vector2.Distance(transform.position, player.position) <= maxFollowDistance) {

            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            if (dir.x < 0 && !leftward)
            {
                Flip();
            }
            else if (dir.x > 0 && leftward)
            {
                Flip();
            }
        }
        else if (Vector2.Distance(transform.position, player.position) < stoppingDistance && Vector2.Distance(transform.position, player.position) > retreatDistance) {

            transform.position = this.transform.position;
        }
        else if (Vector2.Distance(transform.position, player.position) < retreatDistance) {

            transform.position = Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
            if (dir.x < 0 && !leftward)
            {
                Flip();
            }
            else if (dir.x > 0 && leftward)
            {
                Flip();
            }
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
                Vector2 spawnPosition = gameObject.transform.position;
                Destroy(gameObject);
                AnalyticsAPI.BossMonsterDeadCount++;
                if (UnityEngine.Random.Range(0, 100) > 49)
                {
                    Instantiate(drop, spawnPosition, Quaternion.identity);
                }
            }
        }
        else if (collision.gameObject.CompareTag("player_missile"))
        {
            Destroy(collision.gameObject);
            lifeRemain -= 2;
            Healthbar.SetHealth(lifeRemain);
            AnalyticsAPI.BossMonsterHitCount_static++;
            if (lifeRemain == 0)
            {
                Vector2 spawnPosition = gameObject.transform.position;
                Destroy(gameObject);
                AnalyticsAPI.BossMonsterDeadCount++;
                if (UnityEngine.Random.Range(0, 100) > 49)
                {
                    Instantiate(drop, spawnPosition, Quaternion.identity);
                }
            }
        }
        else if (collision.gameObject.CompareTag("player_sniper"))
        {
            Destroy(collision.gameObject);
            lifeRemain -= 3;
            Healthbar.SetHealth(lifeRemain);
            AnalyticsAPI.BossMonsterHitCount_static++;
            if (lifeRemain == 0)
            {
                Vector2 spawnPosition = gameObject.transform.position;
                Destroy(gameObject);
                AnalyticsAPI.BossMonsterDeadCount++;
                if (UnityEngine.Random.Range(0, 100) > 49)
                {
                    Instantiate(drop, spawnPosition, Quaternion.identity);
                }
            }
        }
    }
    
    // Change Direction in UI when the monster changes its direction
    private void Flip()
    {
        leftward = !leftward;
        var transform1 = transform;
        Vector3 charscale = transform1.localScale;
        charscale.x *= -1;
        transform1.localScale = charscale;
    }
}
