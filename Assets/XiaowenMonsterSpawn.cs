using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XiaowenMonsterSpawn : MonoBehaviour
{
    // Start is called before the first frame update

    private float spawn_timer;
    private Rigidbody2D player_rb;
    public GameObject xiaowenMonsterPrefab;
    void Start()
    {
        spawn_timer = 0.0f;
        player_rb = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        spawn_timer -= Time.deltaTime;
        if (spawn_timer < 0)
        {
            spawn_timer = 7.0f;
            Vector2 randomPos = player_rb.position + new Vector2(Random.value, Random.value).normalized*3;
            Instantiate(xiaowenMonsterPrefab, randomPos, Quaternion.identity);
        }
    }
}
