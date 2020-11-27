using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipMovement : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D player_rb;
    private float shoot_timer = 2.0f;
    private enum STATE { A, B };
    private STATE curr_state = STATE.A;

    public Transform left_firepoint;
    public Transform right_firepoint;
    public GameObject space_bulletPrefab;

    private bool isLeft = true;
    void Start()
    {
        player_rb = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 playerPos = player_rb.position;
        RotateTowards(playerPos);
        shoot_timer -= Time.deltaTime;
        if (shoot_timer < 0.0f)
        {
            shoot_timer = 1.5f;
            Shoot(curr_state == STATE.A ? 3.0f : 6.0f);
        }
    }

    private void RotateTowards(Vector2 target)
    {
        var offset = 90f;
        Vector2 direction = target - (Vector2)transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        angle += 90f;
        transform.rotation = Quaternion.Euler(Vector3.forward * (angle + offset));
    }

    void Shoot(float force)
    {
        Vector2 target = player_rb.position;
        var offset = 90f;
        Vector2 direction = target - (Vector2)transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

     
        Quaternion bullet_rotation = Quaternion.Euler(Vector3.forward * (angle + offset));
        
        if (curr_state == STATE.A)
        {
            Transform firePoint = isLeft ? left_firepoint : right_firepoint;
            isLeft = !isLeft;

            GameObject bullet = Instantiate(space_bulletPrefab, firePoint.position, bullet_rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

            Vector2 lookDir = target - new Vector2(firePoint.position.x, firePoint.position.y);
            rb.AddForce(lookDir.normalized * force, ForceMode2D.Impulse);
        }
    }
}
