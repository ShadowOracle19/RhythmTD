using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charges : MonoBehaviour
{
    public int resourceGain;
    public Vector3 placementLocation;
    public bool chargeActive = false;
    private bool damageCharge = false;

    private void Update()
    {
        if (!chargeActive)
        {
            transform.position = Vector3.Lerp(transform.position, placementLocation, Time.deltaTime * 5);


            if (Vector3.Distance(transform.position, placementLocation) < 0.01f)
            {
                transform.position = placementLocation;
                chargeActive = true;
            }
        }
    }

    public void initalizeCharge(int _resourceGain, Vector3 _placementLocation, Tower connectedTower, bool fromTower)
    {
        resourceGain = _resourceGain;
        placementLocation = _placementLocation;
        if(fromTower)
        {
            if (connectedTower.upgradeIndex == 1)
            {
                damageCharge = true;
            }
            else
            {
                damageCharge = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player") && !damageCharge)
        {
            CombatManager.Instance.resourceNum += resourceGain;
            RemoveCharge();
        }
        else if(other.gameObject.CompareTag("Enemy") && damageCharge)
        {
            other.gameObject.GetComponent<Enemy>().Damage(resourceGain * 2);
            RemoveCharge();
        }
    }

    public void RemoveCharge()
    {
        Destroy(gameObject);
    }
}
