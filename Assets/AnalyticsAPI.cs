using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalyticsAPI : MonoBehaviour
{
    // Start is called before the first frame update
    public int BossMonsterHitCount;
    void Start()
    {
        BossMonsterHitCount = 0;
        print("analytics API Started!");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void incrementBossMonsterHitCounter()
    {
        BossMonsterHitCount++;
    }
}
