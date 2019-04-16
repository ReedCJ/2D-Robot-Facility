using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoSlipperyCHildren : EnemyControllerTemplate
{
    private void OnCollision2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Terrain") { StopSliding(); }
    }
}
