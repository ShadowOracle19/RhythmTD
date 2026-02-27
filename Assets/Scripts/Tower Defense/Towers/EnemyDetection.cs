using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    public Tower connectedTower;


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            connectedTower.enemyInRange = true;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            connectedTower.enemyInRange = false;
        }
    }
}
