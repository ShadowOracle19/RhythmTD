using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynthTower : Tower
{
    public override void Start()
    {
        base.Start();

    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if (upgradeThreeActive)
        {
            towerRange = 4;
            currentDamage = 10;
        }
    }

    public override void Fire()
    {
        base.Fire();


        AOE(towerInfo.damage);
    }

    public override void AOE(int damage)
    {
        int tempRange = towerRange;
       
        colliders = Physics.OverlapSphere(transform.position, tempRange);

        foreach (var item in colliders)
        {
            if (item.transform.CompareTag("Tower"))
            {
                item.transform.GetComponent<Tower>().synthBuff = true;

            }
        }
        colliders = null;
    }
}
