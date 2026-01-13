using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcousticGuitarTower : Tower
{
    public override void Start()
    {
        base.Start();

    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        
    }

    public override void Fire()
    {
        base.Fire();

        CreateBullet(towerInfo.damage, transform.position);
    }

    public override void CreateBullet(int damage, Vector3 position)
    {
        int tempRange = towerRange;

        //instatiate bullet downwards
        GameObject bullet = Instantiate(nextProjectile, position, gameObject.transform.rotation, CombatManager.Instance.projectilesParent);


        bullet.GetComponent<Projectile>().InitializeProjectile(towerRange, gameObject, damage, towerInfo.projectilePiercesEnemies);
        bullet.GetComponent<AcousticGuitarProjectile>().isUp = false;

        ConductorV2.instance.projectileEvent.Add(bullet.GetComponent<Projectile>().trigger);

        //instatiate bullet 2 upwards
        GameObject bullet2 = Instantiate(nextProjectile, position, gameObject.transform.rotation, CombatManager.Instance.projectilesParent);


        bullet2.GetComponent<Projectile>().InitializeProjectile(towerRange, gameObject, damage, towerInfo.projectilePiercesEnemies);
        bullet2.GetComponent<AcousticGuitarProjectile>().isUp = true;

        ConductorV2.instance.projectileEvent.Add(bullet2.GetComponent<Projectile>().trigger);
        //towerUpgradeUnlocked = false;
        feelingItNow = false;
        synthBuff = false;
    }
}
