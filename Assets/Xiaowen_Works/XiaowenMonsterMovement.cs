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

    private readonly float PAUSED_SHOOTING_INTERVAL = 0.3f;
    private readonly float PAUSED_SHOOTING_FORCE = 5.0f;
    private readonly float UNPAUSED_SHOOTING_INTERVAL = 1.5f;
    private readonly float UNPAUSED_SHOOTING_FORCE = 2.0f;
    public float bullet_force;

    private float unpausedShootTimer;
    private float pausedShootTimer;

    private float attack_remain;

    // Layer Info
    private readonly int DEFAULT_LAYER = 0;
    private readonly int MONSTER_BULLET_LAYER = 8;
    private readonly int MONSTER_LAYER = 9;
   
    private void OnCollisionEnter2D(Collision2D collision)
    {
        print("entered collider");
        if (collision.gameObject.CompareTag("player_bullet"))
        {
            Destroy(collision.gameObject);
            attack_remain--;
            if (attack_remain == 0)
            {
                Destroy(gameObject);
                print("died");
            }
        }
    }

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

        attack_remain = 10;

        //Physics2D.IgnoreLayerCollision(MONSTER_LAYER, MONSTER_BULLET_LAYER);

    }

    // Update is called once per frame
    void Update()
    {
        

        // there are two stages for this monster: moving, and standing still. Paused means standing still.
        if (! isPaused)
        {

            // update movement variables and change states (moving -> static or static -> moving)
            movementTimer += Time.deltaTime;
            if (movementTimer > 7.0f)
            {
                movementTimer = 0.0f;
                direction.Set(-movement.x, -movement.y);
                movement.Set(0.0f, 0.0f);
                staticTimer = 3.0f;
                isPaused = true;
            }

            // update shooting variable and shoot when shooting timer reaches threshold
            unpausedShootTimer -= Time.deltaTime;
            if (unpausedShootTimer < 0)
            {
                Shoot(unpausedBulletPrefab, UNPAUSED_SHOOTING_FORCE);
                unpausedShootTimer = UNPAUSED_SHOOTING_INTERVAL;
            }
            
        }
        else
        {

            // update movement variables and change states (moving -> static or static -> moving)
            staticTimer -= Time.deltaTime;
            if (staticTimer < 0.0f)
            {
                isPaused = false;
                movement.Set(direction.x, direction.y);
            }

            // update shooting variable and shoot when shooting timer reaches threshold
            pausedShootTimer -= Time.deltaTime;
            if (pausedShootTimer < 0)
            {
                Shoot(pausedBulletPrefab, PAUSED_SHOOTING_FORCE);
                pausedShootTimer = PAUSED_SHOOTING_INTERVAL;
            }
            
       
        }

        // for animation usages. "Speed" paramter works as a threshold for running/idle
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
        // Movement of Player
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        
    }
}
