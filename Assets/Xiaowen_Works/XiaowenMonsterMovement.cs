using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XiaowenMonsterMovement : MonoBehaviour
{
    public float moveSpeed = 1.0f;

    public Rigidbody2D rb;
    public Animator animator;

    // below are movement-related variables
    private float movementTimer;
    private float staticTimer;
    private bool isPaused;
    private Vector2 direction;
    Vector2 movement;

    // below are shooting-related variables
    public Transform firePoint;
    public GameObject unpausedBulletPrefab;
    public GameObject pausedBulletPrefab;

    public Rigidbody2D monster_rb;
    public Rigidbody2D player_rb;

    private float PAUSED_SHOOTING_INTERVAL = 0.3f;
    private float PAUSED_SHOOTING_FORCE = 5.0f;
    private float UNPAUSED_SHOOTING_INTERVAL = 1.5f;
    private float UNPAUSED_SHOOTING_FORCE = 2.0f;
    public float bullet_force;

    private float unpausedShootTimer;
    private float pausedShootTimer;

    private void Start()
    {

        // movement updates
        movementTimer = 0.0f;
        
        staticTimer = 0.0f;
        direction = new Vector2(1.0f, 0.0f);
        movement = direction;

        // shooting updates
        unpausedShootTimer = UNPAUSED_SHOOTING_INTERVAL;
        pausedShootTimer = PAUSED_SHOOTING_INTERVAL;

    }

    // Update is called once per frame
    void Update()
    {
        

        /*
         * Input
         * (abandoned code)
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        */
        if (! isPaused)
        {
            movementTimer += Time.deltaTime;
            if (movementTimer > 7.0f)
            {
                movementTimer = 0.0f;
                direction.Set(-movement.x, -movement.y);
                movement.Set(0.0f, 0.0f);
                staticTimer = 3.0f;
                isPaused = true;
            }

            unpausedShootTimer -= Time.deltaTime;
            if (unpausedShootTimer < 0)
            {
                Shoot(unpausedBulletPrefab, UNPAUSED_SHOOTING_FORCE);
                unpausedShootTimer = UNPAUSED_SHOOTING_INTERVAL;
            }
            
        }
        else
        {
            staticTimer -= Time.deltaTime;
            if (staticTimer < 0.0f)
            {
                isPaused = false;
                movement.Set(direction.x, direction.y);
            }

            pausedShootTimer -= Time.deltaTime;
            if (pausedShootTimer < 0)
            {
                Shoot(pausedBulletPrefab, PAUSED_SHOOTING_FORCE);
                pausedShootTimer = PAUSED_SHOOTING_INTERVAL;
            }
            
       
        }

        animator.SetFloat("Speed", movement.sqrMagnitude);


    }

    void Shoot(GameObject bulletPrefab, float force)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        Vector2 lookDir = player_rb.position - monster_rb.position;
        rb.AddForce(lookDir.normalized * force, ForceMode2D.Impulse);
    }

    void FixedUpdate()
    {
        // Movement 
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

        // update the angles 
        Vector2 cameraPos = Camera.main.transform.position;
        Vector2 lookDir = cameraPos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        
    }
}
