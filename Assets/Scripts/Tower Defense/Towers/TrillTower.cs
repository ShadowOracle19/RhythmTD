using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrillTower : Tower
{
    public int chargeShotDamage = 0;

    public AudioSource TrillDamage;
    public AudioSource TrillDeath;
    public AudioSource TrillUpgrade;
    public AudioSource TrillPlacement;

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
            towerRange = 4;
            currentDamage = 10;
        }
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
        LayerMask enemyLayer = LayerMask.GetMask("Enemy");
        RaycastHit hit;

        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * towerRange, Color.yellow);
        //tower range
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out hit, towerRange, enemyLayer))
        {
            Debug.Log(hit.transform.gameObject.name);
            //if enemy detected
            if(hit.transform.CompareTag("Enemy"))
            {
                int damage = currentDamage + chargeShotDamage;
                CreateBullet(currentDamage, transform.position);
                chargeShotDamage = 0;
            }
        }
        chargeShotDamage += 1;
        chargeShotDamage = Mathf.Clamp(chargeShotDamage, 0, 7);
    }

    //Coward
    public void UpgradeFour()
    {
        colliders = Physics.OverlapSphere(transform.position, towerRange, LayerMask.GetMask("Stage"));

        while(true)
        {
            int rand = colliders.Length;

            if (!colliders[rand].GetComponent<Tile>().cantPlaceTower)
            {
                connectedTile.placedTower = null;
                connectedTile = colliders[rand].GetComponent<Tile>();

                transform.position = 
                    new Vector3(connectedTile.transform.position.x, transform.position.y, connectedTile.transform.position.z);
                return;
            }
        }
    }

    public override void CreateBullet(int damage, Vector3 position)
    {
        int tempRange = towerRange;

        //instatiate bullet
        GameObject bullet = Instantiate(nextProjectile, position, gameObject.transform.rotation, CombatManager.Instance.projectilesParent);
        bullet.GetComponent<Projectile>().InitializeProjectile(towerRange, gameObject, damage, towerInfo.projectilePiercesEnemies);

        ConductorV2.instance.projectileEvent.Add(bullet.GetComponent<Projectile>().trigger);

        if(chargeShotDamage > 2)
        {
            float redLerp = chargeShotDamage / 7;
            bullet.GetComponent<Projectile>().spriteRenderer.color = Color.Lerp(Color.white, Color.red, redLerp);
            bullet.GetComponent<Projectile>().spriteRenderer.sprite = upgradeAttackSprite01;
        }
    }
}
