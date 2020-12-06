using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixHealth : MonoBehaviour
{
    // Start is called before the first frame update
    private PlayerMovement pm;
    private bool initialized = false;
    void Start()
    {
        pm = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();

        XiaowenMonsterSpawn xms = GameObject.FindWithTag("MainCamera").GetComponent<XiaowenMonsterSpawn>();
        xms.controlled = true;
    }

    // Update is called once per frame
    void Update()
    {
        pm.currentHealth = 150;
        if (pm.initialized && !initialized)
        {
            initialized = true;
            GameObject.FindWithTag("Player").transform.position = new Vector3(2f, 2f, 0);
        }
    }
}
