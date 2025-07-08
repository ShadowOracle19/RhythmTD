using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterTile : Tile
{
    public TeleporterTile connectedTile;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Projectile_Tower") && !other.gameObject.GetComponent<Projectile>().teleported)
        {
            other.transform.DOMove(new Vector3(connectedTile.transform.position.x, other.transform.position.y, connectedTile.transform.position.z), 0.1f);
            other.gameObject.GetComponent<Projectile>().nextPosition = new Vector3(connectedTile.transform.position.x + 1, other.transform.position.y, connectedTile.transform.position.z);
            other.gameObject.GetComponent<Projectile>().teleported = true;
            Teleport(other.gameObject);
        }
        if(other.gameObject.CompareTag("Enemy") && !other.gameObject.GetComponent<Enemy>().teleported)
        {
            other.transform.DOMove(new Vector3(connectedTile.transform.position.x, other.transform.position.y, connectedTile.transform.position.z), 0.1f);
            other.gameObject.GetComponent<Enemy>().nextPosition = new Vector3(connectedTile.transform.position.x - 1, other.transform.position.y, connectedTile.transform.position.z);
            other.gameObject.GetComponent<Enemy>().teleported = true;
            Teleport(other.gameObject);
        }
    }

    private void Teleport(GameObject teleportObject)
    {
        teleportObject.transform.position = new Vector3(connectedTile.transform.position.x, teleportObject.transform.position.y, connectedTile.transform.position.z);
    }
}
