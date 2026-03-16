using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    public Tower connectedTower;
    private string enemyTag = "Enemy";



    public List<GameObject> detectedEnemies = new List<GameObject>();
    private float checkTimer = 0.0f;
    float checkTime = 0.5f;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag(enemyTag))
        {
            detectedEnemies.Add(other.gameObject);
        }
        
    }

    public bool EnemyDetected()
    {
        return detectedEnemies.Count > 0;
    }

    private void Update()
    {
        if(detectedEnemies != null && detectedEnemies.Count > 0)
        {
            checkTimer += Time.deltaTime;

            if(checkTimer >= checkTime)
            {
                foreach(GameObject obj in detectedEnemies.ToArray())
                {
                    if(obj == null)
                    {
                        detectedEnemies.Remove(obj);
                    }
                }

                checkTimer = 0.0f;
            }
        }

        connectedTower.enemyInRange = EnemyDetected();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(enemyTag))
        {
            detectedEnemies.Remove(other.gameObject);
        }
    }
}
