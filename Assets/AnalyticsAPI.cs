using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalyticsAPI : MonoBehaviour
{
    // Start is called before the first frame update
    public int BossMonsterHitCount;
    public static int BossMonsterHitCount_static;
    public static int BossMonsterDeadCount;
    void Start()
    {
        BossMonsterHitCount = 0;
        print("analytics API Started!");
        BossMonsterHitCount_static = 0;
        BossMonsterDeadCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
