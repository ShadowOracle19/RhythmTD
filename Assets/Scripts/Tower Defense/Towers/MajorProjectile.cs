using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MajorProjectile : Projectile
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

    public override void OnTriggerEnter(Collider collision)
    {
        //confusing orders
        if(collision.gameObject.CompareTag("Enemy")
            && towerFiredFrom.GetComponent<Tower>().upgradeTwoActive)
        {
            collision.GetComponent<Enemy>().confusingOrderActive = true;
        }

            base.OnTriggerEnter(collision);

        //are you feeling it now comandeer
        //if collide with friendly tower and are you feeling it now is active
        if(collision.gameObject.CompareTag("Tower") 
            && towerFiredFrom.GetComponent<Tower>().upgradeOneActive)
        {
            collision.GetComponent<Tower>().feelingItNow = true;
            RemoveProjectile();
        }
    }
}
