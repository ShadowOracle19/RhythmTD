using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmplifierTile : Tile
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Projectile_Tower"))
        {
            other.gameObject.GetComponent<Projectile>().damage *= 2;
        }
        
    }
}
