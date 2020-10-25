using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterUtil : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public static bool IsCloseToPlayer(float x, float y)
    {

        Rigidbody2D player_rb = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();
        float playerX = player_rb.position.x;
        float playerY = player_rb.position.y;

        float distance = Mathf.Sqrt(Mathf.Pow((x - playerX), 2) + Mathf.Pow((y - playerY), 2));
        return (distance < 10.0f);
    }
}
