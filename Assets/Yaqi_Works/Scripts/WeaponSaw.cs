using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSaw : MonoBehaviour
{
    public float speed;

    private Transform player;
    private Vector2 target;

    void Start()
    {

        player = GameObject.FindWithTag("Player").transform;

        target = new Vector2(player.position.x, player.position.y);

    }

    void Update()
    {
         
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (transform.position.x == target.x && transform.position.y == target.y)
        {
            DestroyWeaponSaw(); 
        }
    }

    void OntrigerEnger2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            DestroyWeaponSaw();
        }
    }

    void DestroyWeaponSaw()
    {
        Destroy(gameObject);
    }
}
