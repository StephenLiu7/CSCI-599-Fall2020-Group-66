using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TumanbaSpawn : MonoBehaviour
{	

	[SerializeField]
    private float spawnRadius = 1, time = 30f;
    public Transform[] points;
    public int totalNumber = 10;
    public GameObject enemy;
    private DateTime[] begin;
    private bool[] spwan;
    
    // Start is called before the first frame update
    void Start(){
        spwan = new bool[10];
        begin = new DateTime[10];
    }


    void FixedUpdate()
    {
        StartCoroutine(SpawnAnEnemy());
    }

    // Update is called once per frame
    IEnumerator SpawnAnEnemy()
    {
        Vector2 player_pos = GameObject.Find("Player").transform.position;

        for(int i = 0; i < points.Length; i++){
            float spawn_x = points[i].position.x;
            float spawn_y = points[i].position.y;
            float x_diff = player_pos.x - spawn_x;
            float y_diff = player_pos.y - spawn_y;
            float distance = (float)Math.Sqrt(x_diff * x_diff + y_diff * y_diff);

            if(distance < spawnRadius && totalNumber > 0){
                if(spwan[i]){
                    DateTime now = DateTime.Now;
                    TimeSpan ts =  now - begin[i];
                    if(ts.TotalMilliseconds < time * 1000){
                        break;
                    }
                    begin[i] = DateTime.Now;
                }
                else{
                    begin[i] = DateTime.Now;
                    spwan[i] = true;
                }
                Instantiate(enemy, points[i]);
                totalNumber--;
                yield return new WaitForSeconds(time);
            }
            else
            {
                yield return null;
            }
        }
    }
}
