using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Monster_Spawer : MonoBehaviour
{	

	[SerializeField]
    private float spawnRadius = 1, time = 0.1f;
    public Transform[] points;
    public int totalNumber = 50;
    public GameObject[] enemies;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnAnEnemy());
    }

    // Update is called once per frame
    IEnumerator SpawnAnEnemy()
    {
        Vector2 spawnPos = GameObject.Find("Player").transform.position;
        spawnPos += UnityEngine.Random.insideUnitCircle.normalized * spawnRadius;
        if(totalNumber > 0)
        {
            Instantiate(enemies[UnityEngine.Random.Range(0, enemies.Length)], points[UnityEngine.Random.Range(0, 15)]);
            totalNumber--;
        }
        
        yield return new WaitForSeconds(time);
        StartCoroutine(SpawnAnEnemy());
    }
}
