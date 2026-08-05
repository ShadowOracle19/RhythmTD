using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcousticGuitarProjectile : Projectile
{
    public bool isUp = false;
    public float startZPosition;
    public float nextZPosition;

    // Start is called before the first frame update
    public override void Start()
    {
        startZPosition = gameObject.transform.position.z;

        if (isUp)
        {
            nextZPosition = transform.position.z - bulletRange;
        }
        else
        {
            nextZPosition = transform.position.z + bulletRange;
        }
        
        timeAtEnd = timeAtFire + (ConductorV2.instance.crotchet * bulletRange);

        movementProgress = 0.0f;
    }

    // Update is called once per frame
    public override void Update()
    {
        songProgress = ConductorV2.instance.songPosition;

        if (movementProgress >= 1.0f)
        {
            RemoveProjectile();
            return;
        }

        movementProgress = (((songProgress - timeAtFire) * speed) / (timeAtEnd - timeAtFire));
        nextPosition = new Vector3(transform.position.x, transform.position.y, Mathf.Lerp(startZPosition, nextZPosition, movementProgress));
        gameObject.transform.position = nextPosition;
    }
}
