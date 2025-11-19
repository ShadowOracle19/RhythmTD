using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerLoadoutManager : MonoBehaviour
{
    public List<GameObject> towerObjects = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectTowerToLoadout(GameObject towerObject)
    {
        //if loadout contains duplicate tower remove it and add it at the end
        if (towerObjects.Contains(towerObject))
        {
            towerObjects.Remove(towerObject);
            towerObjects.Add(towerObject);
            return;
        }
        else
        {
            towerObjects.RemoveAt(3);
            towerObjects.Add(towerObject);
            return;
        }
    }


}
