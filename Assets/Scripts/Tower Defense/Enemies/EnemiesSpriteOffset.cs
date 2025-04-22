using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesSpriteOffset : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        float randx, randy;

        randx = Random.Range(-0.2f, 0.2f);
        randy = Random.Range(-0.2f, 0.2f);

        gameObject.transform.localPosition = new Vector3(randx, randy);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
