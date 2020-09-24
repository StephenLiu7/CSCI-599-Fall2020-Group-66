using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XiaowenMonsterShooting : MonoBehaviour
{

    public Transform firePoint;
    public GameObject bulletPrefab;
    public Rigidbody2D player_rb;

    public float bullet_force;

    private float shootTimer;
    // Start is called before the first frame update
    void Start()
    {
        shootTimer = 1.0f;
        bullet_force = 4.0f;
    }

    // Update is called once per frame
    void Update()
    {
        shootTimer -= Time.deltaTime;
        if (shootTimer < 0.0f)
        {
            shootTimer = 1.0f;
            Shoot(); 
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        //rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);

        Vector2 cameraPos = Camera.main.transform.position;
        Vector2 lookDir = cameraPos - player_rb.position;
        rb.AddForce(lookDir.normalized * bullet_force, ForceMode2D.Impulse);
    }
}
