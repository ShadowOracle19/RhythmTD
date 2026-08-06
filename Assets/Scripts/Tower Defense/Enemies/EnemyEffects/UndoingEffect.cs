
using UnityEngine;

public class UndoingEffect : EnemyEffect
{
    
    public override void UseEffect()
    {
        base.UseEffect();
        int randSpawn = Random.Range(0, 2);
        for (int i = 0; i < randSpawn; i++)
        {
            Spawner.Instance.ForceEnemySpawn(Spawner.Instance.spawnTiles[i].transform.position.y, EnemyType.Runner);
            CombatManager.Instance.enemyTotal += 1;
            CombatManager.Instance.totalNumEnemies += 1;
        }
        
    }                                         
}                                             
                                              
                                              