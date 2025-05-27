using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatIndicatorManager : MonoBehaviour
{
    public int x1, x2;

    public List<GameObject> leftIndicators = new List<GameObject>();
    public List<GameObject> rightIndicators = new List<GameObject>();

    public float time;
    public float lerpTime;

    

    // Update is called once per frame
    void Update()
    {
        

        if(time < 3)
        {
            time += Time.deltaTime;
            lerpTime = time / 3;



            leftIndicators[0].GetComponent<RectTransform>().localPosition =
                Vector3.Lerp(leftIndicators[0].GetComponent<RectTransform>().localPosition,
                new Vector3(0, leftIndicators[0].GetComponent<RectTransform>().localPosition.y, 0), lerpTime);


            rightIndicators[0].GetComponent<RectTransform>().localPosition =
                Vector3.Lerp(rightIndicators[0].GetComponent<RectTransform>().localPosition,
                new Vector3(0, rightIndicators[0].GetComponent<RectTransform>().localPosition.y, 0), lerpTime);



        }
        else
        {
            time = 0;
            leftIndicators[0].GetComponent<RectTransform>().localPosition = new Vector3(x1, leftIndicators[0].GetComponent<RectTransform>().localPosition.y, 0);
            rightIndicators[0].GetComponent<RectTransform>().localPosition = new Vector3(x2, rightIndicators[0].GetComponent<RectTransform>().localPosition.y, 0);
        }

    }
}
