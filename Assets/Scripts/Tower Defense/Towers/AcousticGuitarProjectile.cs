using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcousticGuitarProjectile : Projectile
{
    public bool isUp = false;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

    }

    // Update is called once per frame
    public override void Update()
    {
        if (!canMove) return;
        //transform.Translate(transform.right * 20 * Time.deltaTime);
        //gameObject.transform.DOMoveX(nextPosition.x, ConductorV2.instance.crotchet) 
        //    .SetEase(Ease.OutSine)
        //    .onComplete = CallNextPosition;
        timer += Time.deltaTime * speed;
        if (gameObject.transform.position != nextPosition)
        {

            gameObject.transform.position = Vector3.Slerp(gameObject.transform.position, nextPosition, timer);
        }
        else
        {
            //if isUp is true the bullet will move up on the z axis
            if(isUp)
            {
                nextPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1);
            }
            else
            {
                nextPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1);

                //flip sprite downwards
                spriteRenderer.gameObject.transform.localScale = new Vector3(-spriteRenderer.gameObject.transform.localScale.x,
                spriteRenderer.gameObject.transform.localScale.y,
                spriteRenderer.gameObject.transform.localScale.z);
            }
                
            canMove = false;
        }

    }
}
