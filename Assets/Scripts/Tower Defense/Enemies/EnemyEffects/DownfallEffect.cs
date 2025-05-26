using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownfallEffect : EnemyEffect
{
    public GameObject downfallProjectile;
    public override void UseEffect()
    {
        base.UseEffect();
        
        GameObject bullet = Instantiate(downfallProjectile, transform.position,transform.rotation, CombatManager.Instance.projectilesParent);
        ConductorV2.instance.triggerEvent.Add(bullet.GetComponent<EnemyProjectile>().trigger);
    }
}
