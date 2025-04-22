using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatMover : MonoBehaviour
{
    public Transform endPos;
    public Vector3 originPos;

    public float timeBetweenBeat;

    public float positionInBeats;
    public float positionInAnalog;
    public float loops;

    // Start is called before the first frame update
    void Start()
    {
        timeBetweenBeat = 60 / ConductorV2.instance.bpm;
        originPos = transform.position;

       
    }

    // Update is called once per frame
    void Update()
    {

        //positionInAnalog = ConductorV2.instance.beatDuration;

        ////transform.position = Vector3.Lerp(transform.position, endPos.position, Time.deltaTime);
        ////transform.position = Vector3.LerpUnclamped(transform.position, endPos.position, Time.deltaTime * timeBetweenBeat);
        transform.position = Vector3.MoveTowards(transform.position, endPos.position, timeBetweenBeat * Time.deltaTime);

        if(transform.position == endPos.position)
        {
            Destroy(gameObject, 4);
        }
    }

    IEnumerator Move(Vector3 beginPos, Vector3 endPos, float time)
    {
        for (float t = 0; t < 1; t += Time.deltaTime / time)
        {
            transform.position = Vector3.Lerp(beginPos, endPos, t);
            yield return null;
        }
    }
}
