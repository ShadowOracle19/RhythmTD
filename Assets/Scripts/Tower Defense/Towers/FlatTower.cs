using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlatTower : Tower
{
    //upgrade 2
    bool firstTimeShieldPurchase = false;
    int upgradeTwoRecharge;

    public AudioSource FlatsDamage;
    public AudioSource FlatsDeath;
    public AudioSource FlatsUpgrade;
    public AudioSource FlatsPlacement;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        //upgrade two purchased for the first time
        if (!firstTimeShieldPurchase)
        {
            firstTimeShieldPurchase = true;
            isShielded = true;
        }

        //shield recharged
        if(upgradeTwoRecharge > 10)
        {
            isShielded = true;
            upgradeTwoRecharge = 0;
        }

        //reduce flat attack speed to every 2 beats
        if (upgradeOneActive)
        {
            currentAttackPattern = TowerAttackPattern.everyOtherBeat;
        }
    }

    public override void Fire()
    {
        //increase shield recharge time
        if(upgradeTwoActive && !isShielded)
        {
            upgradeTwoRecharge += 1;
        }
        base.Fire();

        AOE(towerInfo.damage);
    }

    public override void Damage(int damage)
    {
        //if hit while upgrade two is active and shield isnt up reduce shield cooldown to 0
        if(upgradeTwoActive && !isShielded)
        {
            upgradeTwoRecharge = 0;
        }

        base.Damage(damage);
    }
}
