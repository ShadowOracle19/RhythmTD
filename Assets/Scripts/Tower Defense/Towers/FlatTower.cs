using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlatTower : Tower
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        //reduce flat attack speed to every 2 beats
        if (upgradeOneActive)
        {
            currentAttackPattern = TowerAttackPattern.everyOtherBeat;
        }
    }

    public override void Fire()
    {
        base.Fire();

        AOE(towerInfo.damage);
    }
}
