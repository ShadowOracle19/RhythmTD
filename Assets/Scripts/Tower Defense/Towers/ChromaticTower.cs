using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChromaticTower : Tower
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
        if(upgradeThreeActive)
        {
            currentAttackPattern = TowerAttackPattern.everyMeasure;
        }
    }

    public override void Fire()
    {
        //passive income
        if(upgradeTwoActive)
        {
            CombatManager.Instance.resourceNum += towerInfo.resourceGain;
            return;
        }

        

        base.Fire();

        int chargeValue = towerInfo.resourceGain;

        //Power Charge
        if (upgradeThreeActive)
        {
            chargeValue = 15;
        }

        PlaceCharge(chargeValue, this);
    }
}
