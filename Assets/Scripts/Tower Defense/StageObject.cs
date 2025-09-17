using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageObject : MonoBehaviour
{
    public List<Tile> spawnTiles = new List<Tile>();
    public List<Tile> pickupTiles = new List<Tile>();

    public void DestroyStage()
    {
        Destroy(gameObject);
    }

}
