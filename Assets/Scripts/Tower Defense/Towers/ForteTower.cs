using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForteTower : Tower
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

        AOE(currentDamage);
    }

    public override void AOE(int damage)
    {
        int tempRange = towerRange;

        colliders = Physics.OverlapBox(new Vector3(transform.position.x + 2f, transform.position.y, transform.position.z), new Vector3(tempRange / 2, 0.5f, 0.5f));

        foreach (var item in colliders)
        {
            if (item.transform.CompareTag("StageTile"))
            {
                //item.transform.GetComponent<Tile>().Pulse(Color.blue);
                SpawnParticles(item.transform, defaultAttackSprite, aoeAttackParticles, aoeAttackParticlesInstance, false, false);
                
            }
            else if (item.transform.CompareTag("Enemy"))
            {
                item.transform.GetComponent<Enemy>().Damage(damage);

                
            }
        }
        colliders = null;
    }
}
