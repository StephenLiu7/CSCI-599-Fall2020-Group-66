﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatingBullet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            PlayerMovement pm = collision.gameObject.GetComponent<PlayerMovement>();

            pm.bullet_array[0] += 10;
            pm.bullet_array[1] += 5;
            pm.bullet_array[2] += 3;
        }
    }


}
