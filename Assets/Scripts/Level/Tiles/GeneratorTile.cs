using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorTile : Tile
{
    public bool resourceCanGenerate = true;
    public GameObject charge;
    public int chargeResourceGain;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //reset resource can generate on beat 1
        if (ConductorV2.instance.beatTrack == 1 && !resourceCanGenerate)
        {
            resourceCanGenerate = true;
        }
        //place charge on beat 4
        else if(ConductorV2.instance.beatTrack == 4 && resourceCanGenerate)
        {
            PlaceCharge(chargeResourceGain);
            resourceCanGenerate = false;
        }
    }

    public void PlaceCharge(int chargeValue)
    {
        GameObject _charge = Instantiate(charge, transform.position, transform.rotation, CombatManager.Instance.chargesParent);
        _charge.GetComponent<Charges>().initalizeCharge(chargeValue, new Vector3(transform.position.x, 0.5f, transform.position.z), null, false);
    }
}
