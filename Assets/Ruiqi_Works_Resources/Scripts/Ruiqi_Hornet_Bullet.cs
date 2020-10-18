using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ruiqi_Hornet_Bullet : MonoBehaviour
{
    public float speed;

    private Rigidbody2D rb;
    private Transform player;
    private Vector2 moveDirection;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player").transform;
        moveDirection = (player.position - transform.position).normalized * speed;
        rb.velocity = new Vector2(moveDirection.x, moveDirection.y);
    }
    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }
    }
}
