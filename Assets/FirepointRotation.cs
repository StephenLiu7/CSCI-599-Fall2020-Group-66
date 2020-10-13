using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirepointRotation : MonoBehaviour
{
    // Start is called before the first frame update

    private Rigidbody2D player_rb;

    void Start()
    {
        GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 target = player_rb.position;

        var offset = 90f;
        Vector2 direction = target - (Vector2)transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(Vector3.forward * (angle + offset));
    }
}
