using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public float speed;
    private Transform playerPos;
    public float health_remain;
    public GameObject drop;
    void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, playerPos.position) > 0.44)
            transform.position = Vector2.MoveTowards(transform.position, playerPos.position, speed * Time.deltaTime);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        print("entered collider");
        if (collision.gameObject.CompareTag("player_bullet"))
        {
            Destroy(collision.gameObject);
            AnalyticsAPI.BossMonsterHitCount_static++;
            Debug.Log(AnalyticsAPI.BossMonsterHitCount_static);
            health_remain--;
            if (health_remain == 0)
            {
                AnalyticsAPI.BossMonsterDeadCount++;
                Destroy(gameObject);
                Vector2 spawnPos = gameObject.transform.position;

                if (UnityEngine.Random.Range(0, 4) > 2)
                    Instantiate(drop, spawnPos, Quaternion.identity);
                print("died");
            }
        }else if (collision.gameObject.CompareTag("player_missile"))
        {
            Destroy(collision.gameObject);
            AnalyticsAPI.BossMonsterHitCount_static++;
            Debug.Log(AnalyticsAPI.BossMonsterHitCount_static);
            health_remain-=3;
            if (health_remain <= 0)
            {
                AnalyticsAPI.BossMonsterDeadCount++;
                Destroy(gameObject);
                Vector2 spawnPos = gameObject.transform.position;

                if (UnityEngine.Random.Range(0, 4) > 2)
                    Instantiate(drop, spawnPos, Quaternion.identity);
                print("died");
            }
        }
        else if (collision.gameObject.CompareTag("player_sniper"))
        {
            Destroy(collision.gameObject);
            AnalyticsAPI.BossMonsterHitCount_static++;
            Debug.Log(AnalyticsAPI.BossMonsterHitCount_static);
            health_remain-=4;
            if (health_remain <= 0)
            {
                AnalyticsAPI.BossMonsterDeadCount++;
                Destroy(gameObject);
                Vector2 spawnPos = gameObject.transform.position;

                if (UnityEngine.Random.Range(0, 4) > 2)
                    Instantiate(drop, spawnPos, Quaternion.identity);
                print("died");
            }
        }
    }
}

