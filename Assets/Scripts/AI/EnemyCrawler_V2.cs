using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCrawler_V2 : EnemyScript
{
    public override void States()
    {
        switch (_state)
        {
            case EnemyState.patrol:
                Patrol();
                break;
            case EnemyState.dead:
                if (!isDead)
                    Dead();
                break;
        }
    }

    public override void Patrol()
    {
        
    }
}
