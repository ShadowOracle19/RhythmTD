using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursedEffect : EnemyEffect
{
    private RaycastHit2D[] colliders;
    public override void UseEffect()
    {
        base.UseEffect();
        colliders = Physics2D.BoxCastAll(transform.position, Vector2.one * 3, 0, transform.forward);

        foreach (var item in colliders)
        {
            if (item.transform.CompareTag("Tower"))
            {
                item.transform.GetComponent<Tower>().Damage(5);
            }
            else if (item.transform.CompareTag("StageTile"))
            {
                item.transform.GetComponent<Tile>().Pulse(Color.green);
            }
        }
        colliders = null;
        return;
    }
}
