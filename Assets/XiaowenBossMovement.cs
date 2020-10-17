using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XiaowenBossMovement : MonoBehaviour
{
    // Start is called before the first frame update
    
    enum Stage { ONE, TWO }

    private Stage gameStage;
    private int maxHealth;
    private int currHealth;
    public Transform firePoint;
    public GameObject stage1Bullet;
    public GameObject stage2Bullet;
    public Animator animator;
    private float stage1ShootingTimer = 5.0f;
    private float stage2ShootingTimer = 1.0f;

    private Rigidbody2D player_rb;
    private GameObject player;

    public GameObject xiaowen_pill;
    void Start()
    {
        gameStage = Stage.ONE;
        maxHealth = 10;
        currHealth = 10;

        player = GameObject.FindWithTag("Player");
        player_rb = player.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStage == Stage.ONE)
        {
            stage1ShootingTimer -= Time.deltaTime;
            if (stage1ShootingTimer < 0.0f)
            {
                stage1ShootingTimer = 5.0f;
                Shoot(stage1Bullet, 3.0f);
            }
        }
        else
        {
            stage2ShootingTimer -= Time.deltaTime;
            if (stage2ShootingTimer < 0.0f)
            {
                stage2ShootingTimer = 1.0f;
                Shoot(stage2Bullet, 7.0f);
            }
        }

        // now, let us check rotations

        if (player.transform.position.x > gameObject.transform.position.x)
        {
            if (gameObject.transform.eulerAngles.y != 0)
            {
                gameObject.transform.eulerAngles = new Vector3(
                gameObject.transform.eulerAngles.x,
                0.0f,
                gameObject.transform.eulerAngles.z);
                
            }
        }
        else
        {
            if (gameObject.transform.eulerAngles.y != 180.0f)
            {
                gameObject.transform.eulerAngles = new Vector3(
                gameObject.transform.eulerAngles.x,
                180.0f,
                gameObject.transform.eulerAngles.z);

            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        int healthReduction = 0;
        if (collision.gameObject.CompareTag("player_bullet"))
        {
            Destroy(collision.gameObject);
            healthReduction = 1;
            /*
            GameObject g = GameObject.Find("Main Camera");
            g.GetComponent<AnalyticsAPI>().incrementBossMonsterHitCounter();
            print(g.GetComponent<AnalyticsAPI>().BossMonsterHitCount);*/
            AnalyticsAPI.BossMonsterHitCount_static++;
        }else if (collision.gameObject.CompareTag("player_missile"))
        {
            Destroy(collision.gameObject);
            healthReduction = 2;
            /*
            GameObject g = GameObject.Find("Main Camera");
            g.GetComponent<AnalyticsAPI>().incrementBossMonsterHitCounter();
            print(g.GetComponent<AnalyticsAPI>().BossMonsterHitCount);*/
            AnalyticsAPI.BossMonsterHitCount_static++;
        }

        currHealth -= healthReduction;

        if (currHealth <= 0)
        {
            Vector2 spawnPos = gameObject.transform.position;
            Destroy(gameObject);
            AnalyticsAPI.BossMonsterDeadCount++;
            Instantiate(xiaowen_pill, spawnPos, Quaternion.identity);
            
        }else if (currHealth <= 5)
        {
            gameStage = Stage.TWO;
            animator.SetFloat("Speed", 1.0f);
        }

    }

    private void Shoot(GameObject bulletPrefab, float force)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        Vector2 target = player_rb.position;

        Vector2 lookDir = (target - GetComponent<Rigidbody2D>().position).normalized;
        
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        if (player.transform.position.x < gameObject.transform.position.x)
        {
            
        }

        rb.rotation = angle;


        rb.AddForce(lookDir * force, ForceMode2D.Impulse);

        print("Arrived here");

        Destroy(bullet, 10.0f);
    }
}
