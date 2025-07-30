
using UnityEngine;

public class UndoingEffect : EnemyEffect
{
    
    public override void UseEffect()
    {
        base.UseEffect();
        int randSpawn = Random.Range(0, 3);
        for (int i = 0; i < randSpawn; i++)
        {
            Spawner.Instance.ForceEnemySpawn(Spawner.Instance.spawnTiles[i].transform.position.y, EnemyType.Wisp);
            CombatManager.Instance.enemyTotal += 1;
        }
        
    }                                         
}                                             
                                              
                                              