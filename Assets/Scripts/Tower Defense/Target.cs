using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private ScreenShake shake;

    private void Start()
    {
        
    }

    private void OnTriggerStay(Collider collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            GameManager.Instance.Damage();
            collision.gameObject.GetComponent<Enemy>().Kill();
            shake.CamShake();
        }
    }
}
