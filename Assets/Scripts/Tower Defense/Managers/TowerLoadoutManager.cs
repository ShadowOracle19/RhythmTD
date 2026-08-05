using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerLoadoutManager : MonoBehaviour
{
    [Header("<b><size=15>Tower Objects<b><size=15>")]
    [Line(255,255,255)]
    public List<GameObject> towerObjects = new List<GameObject>();

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
            towerObjects.RemoveAt(0);

            //towerObjects.Sort();

            towerObjects.Add(towerObject);
            return;
        }
    }

    public void ApplyLoadoutChanges()
    {
        GameManager.Instance.towers[0].tower = towerObjects[0];
        GameManager.Instance.towers[1].tower = towerObjects[1];
        GameManager.Instance.towers[2].tower = towerObjects[2];
        GameManager.Instance.towers[3].tower = towerObjects[3];
    }
}
