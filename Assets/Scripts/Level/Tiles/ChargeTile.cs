using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeTile : Tile
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(placedTower != null)
        {
            ChargeTower();
        }
    }

    public void ChargeTower()
    {
        placedTower.GetComponent<Tower>().ChargedUp = true;
    }
}
