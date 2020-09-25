using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ruiqi_Enemy_Shot : MonoBehaviour
{
    public float speed;

    private Transform player;
    private Vector2 target;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
        target = new Vector2(player.position.x, player.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        if (transform.position.x == target.x && transform.position.y == target.y) {
            DestroyProjectTile();
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            DestroyProjectTile();
        }
    }

    void DestroyProjectTile() {
        Destroy(gameObject);
    }
}
