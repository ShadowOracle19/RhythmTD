using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrillProjectile : Projectile
{
    public GameObject echoProjectile;

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

    public override void OnTriggerEnter(Collider collision)
    {
        //if collide with enemy spawn two echo projectiles
        if (collision.gameObject.CompareTag("Enemy")
            && towerFiredFrom.GetComponent<Tower>().upgradeIndex == 2)
        {
            Debug.Log("Echo");
            collision.GetComponent<Enemy>().Damage(damage);

            //create two echo bullets
            GameObject bullet = Instantiate(echoProjectile, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z - 1)
                , gameObject.transform.rotation, CombatManager.Instance.projectilesParent);
            bullet.GetComponent<ProjectileEcho>().InitializeProjectile(2, gameObject, damage / 2, false, towerFiredFrom.GetComponent<Tower>().attackTargetTime);
            bullet.GetComponent<ProjectileEcho>().spriteRenderer.gameObject.transform.localScale

                = new Vector3(-bullet.GetComponent<ProjectileEcho>().spriteRenderer.gameObject.transform.localScale.x,
                bullet.GetComponent<ProjectileEcho>().spriteRenderer.gameObject.transform.localScale.y,
                bullet.GetComponent<ProjectileEcho>().spriteRenderer.gameObject.transform.localScale.z);

            bullet.GetComponent<ProjectileEcho>().direction = -1;

            //bullet two
            GameObject bullet2 = Instantiate(echoProjectile, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z + 1)
                , gameObject.transform.rotation, CombatManager.Instance.projectilesParent);
            bullet2.GetComponent<ProjectileEcho>().InitializeProjectile(2, gameObject, damage / 2, false, towerFiredFrom.GetComponent<Tower>().attackTargetTime);
            bullet2.GetComponent<ProjectileEcho>().direction = 1;


            RemoveProjectile();
            return;
        }
        
        base.OnTriggerEnter(collision);

        
    }
}
