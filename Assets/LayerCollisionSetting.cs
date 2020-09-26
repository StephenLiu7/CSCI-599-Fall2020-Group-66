using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerCollisionSetting : MonoBehaviour
{

    private readonly int DEFAULT_LAYER = 0;
    private readonly int MONSTER_BULLET_LAYER = 8;
    private readonly int MONSTER_LAYER = 9;
    private readonly int PLAYER_BULLET_LAYER = 10;
    private readonly int PLAYER_LAYER = 11;
    private readonly int MAP_LAYER = 20;
    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreLayerCollision(MONSTER_BULLET_LAYER, MONSTER_BULLET_LAYER);
        Physics2D.IgnoreLayerCollision(MONSTER_LAYER, MONSTER_BULLET_LAYER);
        Physics2D.IgnoreLayerCollision(PLAYER_LAYER, PLAYER_BULLET_LAYER);
        Physics2D.IgnoreLayerCollision(PLAYER_BULLET_LAYER, PLAYER_BULLET_LAYER);
        Physics2D.IgnoreLayerCollision(PLAYER_BULLET_LAYER, MONSTER_BULLET_LAYER);
        Physics2D.IgnoreLayerCollision(MAP_LAYER, PLAYER_BULLET_LAYER);
        Physics2D.IgnoreLayerCollision(MAP_LAYER, MONSTER_BULLET_LAYER);
    }
}
