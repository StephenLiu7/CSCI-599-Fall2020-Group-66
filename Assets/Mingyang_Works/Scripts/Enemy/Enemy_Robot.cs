using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection.Emit;
using System.Security.Cryptography;
using UnityEngine;

public class Enemy_Robot : MonoBehaviour
{
    private Transform playerPos;
    private Rigidbody2D rb;
    public float speed = .3f;
    public float health_remain;

    public Transform bulletPos1, bulletPos2;
    public GameObject bullet;

    private bool isInRange = false;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Start()
    {
         StartCoroutine(Shoot());
    }
    void FixedUpdate()
    {
        Rotation();

            
    }
    void Update()
    {
        if (Vector2.Distance(transform.position, playerPos.position) > 5f)
        {
            transform.position = Vector2.MoveTowards(transform.position, playerPos.position, speed * Time.deltaTime);
            isInRange = false;
        }
        else
        {
            isInRange = true;
        }

    }

    void Rotation()
    {
        Vector2 direction = (playerPos.gameObject.GetComponent<Rigidbody2D>().position - rb.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;
        rb.rotation = angle;
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
                Destroy(gameObject);
                print("died");
            }
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, playerPos.position, speed * Time.deltaTime);
        }
    }

    IEnumerator Shoot()
    {
        if (isInRange)
            Instantiate(bullet, bulletPos1.position, transform.rotation);
            yield return new WaitForSeconds(0.3f);
        if (isInRange)
            Instantiate(bullet, bulletPos2.position, transform.rotation);
            yield return new WaitForSeconds(0.3f);
        
            StartCoroutine(Shoot());
            yield return new WaitForSeconds(0.3f);
        
        

    }
}
