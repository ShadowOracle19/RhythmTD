using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileAOEAttack : MonoBehaviour
{

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("StageTile"))
        {
            collision.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("StageTile"))
        {
            collision.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
}
