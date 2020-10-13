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
    void Start()
    {
        gameStage = Stage.ONE;
        maxHealth = 10;
        currHealth = 10;

        player_rb = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();
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
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        int healthReduction = 0;
        if (collision.gameObject.CompareTag("player_bullet"))
        {
            Destroy(collision.gameObject);
            healthReduction = 1;
        }

        currHealth -= healthReduction;

        if (currHealth <= 0)
        {
            Destroy(gameObject);
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

        rb.rotation = angle;


        rb.AddForce(lookDir * force, ForceMode2D.Impulse);

        print("Arrived here");
    }
}
