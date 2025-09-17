using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        
        if(collision.gameObject.CompareTag("StageTile"))
        {
            GetComponentInParent<Enemy>().tileInFront = collision.GetComponent<Tile>();
            GetComponentInParent<Enemy>().obstacleFound = false;
        }
        else if(collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Pitfall"))
        {
            GetComponentInParent<Enemy>().tileInFront = collision.GetComponent<Tile>();
            GetComponentInParent<Enemy>().obstacleFound = true;
        }
    }
}
