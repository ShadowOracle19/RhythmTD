using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MajorTower : Tower
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
    }

    public override void Fire(float yPos)
    {
        if (!enemyInRange)
            return;

        base.Fire(yPos);

        if(upgradeIndex == 3)
        {
            int rand = Random.Range(0, 3);

            switch (rand)
            {
                case 0:
                    CreateBullet(currentDamage, new Vector3(gameObject.transform.position.x + 1f, gameObject.transform.position.y, gameObject.transform.position.z));
                    break;

                case 1:
                    CreateBullet(currentDamage, new Vector3(gameObject.transform.position.x + 1f, gameObject.transform.position.y, gameObject.transform.position.z + 1));
                    CreateBullet(currentDamage, new Vector3(gameObject.transform.position.x + 1f, gameObject.transform.position.y, gameObject.transform.position.z - 1));
                    break;

                case 2:
                    CreateBullet(currentDamage, new Vector3(gameObject.transform.position.x + 1f, gameObject.transform.position.y, gameObject.transform.position.z + 1));
                    CreateBullet(currentDamage, new Vector3(gameObject.transform.position.x + 1f, gameObject.transform.position.y, gameObject.transform.position.z));
                    CreateBullet(currentDamage, new Vector3(gameObject.transform.position.x + 1f, gameObject.transform.position.y, gameObject.transform.position.z - 1));
                    break;

                default:
                    break;
            }

            return;
        }

        CreateBullet(currentDamage, new Vector3(gameObject.transform.position.x + 1f, gameObject.transform.position.y, gameObject.transform.position.z + yPos));
    }
}
