using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrillTower : Tower
{
    public int chargeShotDamage = 0;

    // Start is called before the first frame update
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

        //Charge shot upgrade active
        if(upgradeOneActive)
        {
            UpgradeOne();
            return;
        }

        CreateBullet(currentDamage, transform.position);
    }

    //charge shot
    public void UpgradeOne()
    {
        RaycastHit hit;

        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * towerRange, Color.yellow);
        //tower range
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out hit, towerRange))
        {
            //if enemy detected
            if(hit.transform.CompareTag("Enemy"))
            {
                int damage = currentDamage + chargeShotDamage;
                CreateBullet(currentDamage, transform.position);
                chargeShotDamage = 0;
            }
            //charge
            else
            {

                chargeShotDamage += 1;
                chargeShotDamage = Mathf.Clamp(chargeShotDamage, 0, 7);
            }
        }
    }

    public override void CreateBullet(int damage, Vector3 position)
    {
        int tempRange = towerRange;

        //instatiate bullet
        GameObject bullet = Instantiate(projectile, position, gameObject.transform.rotation, CombatManager.Instance.projectilesParent);
        bullet.GetComponent<Projectile>().InitializeProjectile(towerRange, gameObject, damage, towerInfo.projectilePiercesEnemies);

        ConductorV2.instance.projectileEvent.Add(bullet.GetComponent<Projectile>().trigger);

        if(chargeShotDamage > 0)
        {
            bullet.GetComponent<Projectile>().spriteRenderer.sprite = increasedAttackSprite;
        }
    }
}
