using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharmedEffect : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Projectile_Tower"))
        {
            collision.gameObject.GetComponent<Projectile>().RemoveProjectile();
        }
    }
}
