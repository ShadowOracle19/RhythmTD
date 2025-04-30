using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private ScreenShake shake;

    private void Start()
    {
        shake = GameObject.FindGameObjectWithTag("ScreenShaker").GetComponent<ScreenShake>();
    }
    private void OnTriggerStay(Collider collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            GameManager.Instance.Damage();
            shake.CamShake();
            collision.gameObject.GetComponent<Enemy>().Kill();
        }
    }
}
