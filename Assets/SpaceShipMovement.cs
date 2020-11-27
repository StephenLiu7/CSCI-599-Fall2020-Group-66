﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipMovement : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D player_rb;

    void Start()
    {
        player_rb = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 playerPos = player_rb.position;
        RotateTowards(playerPos);
    }

    private void RotateTowards(Vector2 target)
    {
        var offset = 90f;
        Vector2 direction = target - (Vector2)transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        angle += 90f;
        transform.rotation = Quaternion.Euler(Vector3.forward * (angle + offset));
    }
}
