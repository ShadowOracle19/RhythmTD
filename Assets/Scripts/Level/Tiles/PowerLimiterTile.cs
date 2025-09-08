using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerLimiterTile : Tile
{
    public TowerResourceCost costLimiter;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool CanPlaceTower(TowerResourceCost cost)
    {
        switch (cost)
        {
            case TowerResourceCost.one:
                return false;
                //if cost limiter is set to two allow two cost tower
            case TowerResourceCost.two:
                if(costLimiter == TowerResourceCost.two)
                    return true;
                break;
                //if cost limiter is set to two or three allow three cost towers
            case TowerResourceCost.three:
                if( costLimiter == TowerResourceCost.two || costLimiter == TowerResourceCost.three)
                    return true;
                break;
            case TowerResourceCost.four:
                if (costLimiter == TowerResourceCost.two || costLimiter == TowerResourceCost.three || costLimiter == TowerResourceCost.four)
                    return true;
                break;
            default:
                return false;
        }

        return false;
    }
}
