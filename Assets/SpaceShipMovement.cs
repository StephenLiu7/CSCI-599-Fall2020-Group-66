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

    private float statusTimer = 0.0f;
    private bool isLeft = true;
    void Start()
    {
        player_rb = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("player_bullet"))
        {
            Destroy(collision.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 playerPos = player_rb.position;
        RotateTowards(playerPos);
        shoot_timer -= Time.deltaTime;
        statusTimer += Time.deltaTime;
        if (statusTimer > 25.0f && curr_state == STATE.A)
        {
            curr_state = STATE.B;
        }
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
        float monsterX = gameObject.transform.position.x;
        float monsterY = gameObject.transform.position.y;
        if (!MonsterUtil.IsCloseToPlayer(monsterX, monsterY))
        {
            return;
        }
        if (curr_state == STATE.A)
        {
            Transform firePoint = isLeft ? left_firepoint : right_firepoint;
            isLeft = !isLeft;

            GameObject bullet = Instantiate(space_bulletPrefab, firePoint.position, bullet_rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

            Vector2 lookDir = target - new Vector2(firePoint.position.x, firePoint.position.y);
            rb.AddForce(lookDir.normalized * force, ForceMode2D.Impulse);

            Destroy(bullet, 10.0f);
        }
        else
        {
            GameObject bulletLeft = Instantiate(space_bulletPrefab, left_firepoint.position, bullet_rotation);
            GameObject bulletRight = Instantiate(space_bulletPrefab, right_firepoint.position, bullet_rotation);

            Rigidbody2D rb = bulletLeft.GetComponent<Rigidbody2D>();

            Vector2 lookDir = target - new Vector2(left_firepoint.position.x, left_firepoint.position.y);

            rb.AddForce(lookDir.normalized * force, ForceMode2D.Impulse);

            Rigidbody2D rb2 = bulletRight.GetComponent<Rigidbody2D>();
            Vector2 lookDir2 = target - new Vector2(right_firepoint.position.x, right_firepoint.position.y);
            rb2.AddForce(lookDir2.normalized * force, ForceMode2D.Impulse);

            Destroy(bulletLeft, 10.0f);
            Destroy(bulletRight, 10.0f);
        }
    }
}
