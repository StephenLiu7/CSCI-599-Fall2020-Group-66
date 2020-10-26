using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eater : MonoBehaviour
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
            pm.currentHealth += 40;
            if (pm.currentHealth > pm.maxHealth)
            {
                pm.currentHealth = pm.maxHealth;
            }
            pm.healthBar.SetHealth(pm.currentHealth);
            pm.hpAmount++;

        }



    }
}
