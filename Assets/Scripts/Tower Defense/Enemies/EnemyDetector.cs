using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("StageTile"))
        {
            GetComponentInParent<Enemy>().tileInFront = collision.GetComponent<Tile>();
        }
    }
}
