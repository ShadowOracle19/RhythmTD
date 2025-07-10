using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BeatResult : MonoBehaviour
{
    public float fade = 1;
    public Vector3 originPos;
    // Start is called before the first frame update
    void Start()
    {
        originPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        fade -= Time.deltaTime * 2;
        fade = Mathf.Clamp(fade, 0, 1);

        GetComponent<SpriteRenderer>().color = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b, fade);
        transform.position = Vector3.Lerp(new Vector3(originPos.x, originPos.y, originPos.z + 0.5f), transform.position, fade);
        if(fade == 0)
        {
            Destroy(gameObject);
        }
    }
}
