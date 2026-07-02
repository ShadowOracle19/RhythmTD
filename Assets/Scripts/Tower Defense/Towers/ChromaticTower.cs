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
        
        /*
        if(upgradeThreeActive)
        {
            currentAttackPattern = TowerAttackPattern.everyMeasure;
        }
        */
    }

    public override void Fire(float yPos)
    {
        //passive income
        if(upgradeIndex == 2)
        {
            CombatManager.Instance.resourceNum += towerInfo.resourceGain;
            return;
        }

        base.Fire(0f);

        int chargeValue = towerInfo.resourceGain;

        //Power Charge
        if (upgradeIndex == 3)
        {
            chargeValue = 10;
        }

        PlaceCharge(chargeValue, this);
    }

    public override void Damage(int damage)
    {
        base.Damage(damage);
    }

    
}
