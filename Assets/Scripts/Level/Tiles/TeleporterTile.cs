using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterTile : Tile
{
    public TeleporterTile connectedTile;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Projectile_Tower") || other.gameObject.CompareTag("Enemy"))
        {
            Teleport(other.gameObject);
        }
    }

    private void Teleport(GameObject teleportObject)
    {
        teleportObject.transform.position = new Vector3(connectedTile.transform.position.x, teleportObject.transform.position.y, connectedTile.transform.position.z);
    }
}
