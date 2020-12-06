using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XiaowenMonsterSpawn : MonoBehaviour
{
    // Start is called before the first frame update

    private float spawn_timer;
    private Rigidbody2D player_rb;
    public GameObject xiaowenMonsterPrefab;
    public GameObject xiaowenBossPrefab;
    private (float, float) xRange = (50.1f, 155.7f);
    private (float, float) yRange = (-42.1f, 42.6f);
    private float final_circle_timer = 2.5f;
    public int MonsterKilledCount = 0;
    public int BossMaxHealth = 10;
    private bool addedSpaceShip = false;
    public GameObject temp_spaceShipPrefab;

    // used for other fils actually... for tutorial purposes..
    public bool controlled = false;
    public bool hasShooted = false;

    void Start()
    {
        spawn_timer = 0.0f;
        player_rb = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();
        float playerX = player_rb.position.x;
        float playerY = player_rb.position.y;
        bool isInRange = (playerX < xRange.Item2 && playerX > xRange.Item1) && (playerY < yRange.Item2 && playerY > yRange.Item1);
        if(isInRange){
            spawnBoss();
        }

        // one time spawn of spaceship 


    }

    // Update is called once per frame
    void Update()
    {
        //GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().currentHealth++;
        float playerX = player_rb.position.x;
        float playerY = player_rb.position.y;

        bool isInRange = (playerX < xRange.Item2 && playerX > xRange.Item1) && (playerY < yRange.Item2 && playerY > yRange.Item1);
        
        spawn_timer -= Time.deltaTime;
        final_circle_timer -= Time.deltaTime;
        if (DamageCircle.circle_stage > 1 && !controlled)
        {
            if (!addedSpaceShip)
            {
                addedSpaceShip = true;
                if (GameMode.Difficulty == "medium")
                {
                    Instantiate(temp_spaceShipPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                }else if (GameMode.Difficulty == "hard")
                {
                    Instantiate(temp_spaceShipPrefab, new Vector3(-6.29f, 5.31f, 0), Quaternion.identity);
                    Instantiate(temp_spaceShipPrefab, new Vector3(6.67f, 5.36f, 0), Quaternion.identity);
                }
            }
            if (final_circle_timer <= 0.0f)
            {
                final_circle_timer = 2.5f;
                Vector2 randomPos = player_rb.position + new Vector2(Random.value, Random.value).normalized * 3;
                Instantiate(xiaowenMonsterPrefab, randomPos, Quaternion.identity);
            }
            return;
        }

        if (spawn_timer < 0 && isInRange)
        {
            spawn_timer = 6.0f;
            Vector2 randomPos = player_rb.position + new Vector2(Random.value, Random.value).normalized*3;
            Instantiate(xiaowenMonsterPrefab, randomPos, Quaternion.identity);
        }
    }

    public void spawnBoss()
    {
        BossMaxHealth += 2;
        Rigidbody2D prb = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();
        Vector2 randomPos = prb.position - new Vector2(Random.value, Random.value).normalized * 6;
        GameObject g = Instantiate(xiaowenBossPrefab, randomPos, Quaternion.identity);
    }
}
