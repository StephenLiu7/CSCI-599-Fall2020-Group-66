using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private float spawnRadius = 1, time = 0.1f;

    public Transform[] points;
    public int totalNumber = 50;

    public GameObject[] enemies;
    void Start()
    {
        StartCoroutine(SpawnAnEnemy());
    }

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
