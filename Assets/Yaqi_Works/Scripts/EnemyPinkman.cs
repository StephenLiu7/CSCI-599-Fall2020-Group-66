using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPinkman : MonoBehaviour
{
    public float speed;
    public float stoppingDistance;
    public float retreatDistance;

    private float timeBtShots;
    public float startTimeBtShots;

    public GameObject WeaponSaw;
    private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        timeBtShots = startTimeBtShots;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, player.position) > stoppingDistance)
        {

            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
        else if (Vector2.Distance(transform.position, player.position) < stoppingDistance && Vector2.Distance(transform.position, player.position) > retreatDistance)
        {
            transform.position = this.transform.position;
        }
        else if (Vector2.Distance(transform.position, player.position) < retreatDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
        }

        if (timeBtShots <= 0)
        {
            Instantiate(WeaponSaw, transform.position, Quaternion.identity);
            timeBtShots = startTimeBtShots;

        }
        else
        {

            timeBtShots -= Time.deltaTime;
        }

    }
}
