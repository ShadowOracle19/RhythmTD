using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEcho : Projectile
{
    public int direction = 0;

    // Start is called before the first frame update
    public override void Start()
    {
        nextPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z + direction);

    }

    // Update is called once per frame
    public override void Update()
    {
        if (!canMove) return;

        timer += Time.deltaTime * speed;
        if (gameObject.transform.position != nextPosition)
        {

            gameObject.transform.position = Vector3.Slerp(gameObject.transform.position, nextPosition, timer);
        }
        else
        {
            nextPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z + direction);
            canMove = false;
        }
    }
}
