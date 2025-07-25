using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_1_Cursor : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>()._currentDamage += 2;
        }
    }
}
