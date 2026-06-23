using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PocoTower : Tower
{

    public Enemy connectedEnemy;
    public LineRenderer lineObject;

    public override void Start()
    {
        base.Start();

        lineObject.positionCount = 2;

        lineObject.useWorldSpace = true;
        lineObject.SetPosition(0, this.transform.position);
        lineObject.SetPosition(1, this.transform.position);
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if(lineObject.enabled)
            lineObject.SetPosition(1, connectedEnemy.transform.position);

    }

    public override void Fire()
    {
        base.Fire();
        lineObject.enabled = false;

        

        FindClosestEnemy();

        if(connectedEnemy == null)
        {
            return;
        }

       
        FireLaser();
        
    }

    private void FindClosestEnemy()
    {
        if (connectedEnemy != null && connectedEnemy.currentHealth > 0)
        {
            FireLaser();
            return;
        }

        Vector3 pos = this.transform.position;
        float dist = float.PositiveInfinity;
        Enemy target = null;

        //if enemy list is empty end call
        if (Spawner.Instance.enemies.Count == 0)
        {
            return;
        }

        foreach (var item in Spawner.Instance.enemies)
        {
            var d = (pos - item.transform.position).sqrMagnitude;
            if (d < dist)
            {
                target = item;
                dist = d;
            }

        }
        connectedEnemy = target;

    }

    private void FireLaser()
    {
        if (connectedEnemy.currentHealth > 0)
        {
            lineObject.enabled = true;
            lineObject.SetPosition(0, lineObject.transform.position);
            lineObject.SetPosition(1, connectedEnemy.transform.position);
            connectedEnemy.Damage(currentDamage);
        }

    }
}
