using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class Projectile : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public float speed = 1;
    float timer;
    public bool canMove = false;
    public Vector3 nextPosition;
    public int bulletRange = 0;
    int activeTime;
    public bool piercing;

    public int damage = 1;

    public GameObject towerFiredFrom;

    public UnityEvent trigger;

    public bool burningBullet = false;
    public Sprite flameAttackSprite;
    [SerializeField] private ParticleSystem burningParticles;
    private ParticleSystem burningParticlesInstance;

    public bool teleported = false;

    public void InitializeProjectile(int range, GameObject firedFrom, int _damage, bool isPiercing, bool isBurning)
    {
        bulletRange = range;
        towerFiredFrom = firedFrom;
        damage = _damage;
        piercing = isPiercing;
        burningBullet = isBurning;

        if (isBurning)
        {
            //burningParticlesInstance = Instantiate(burningParticles, transform.position, Quaternion.identity);
            spriteRenderer.sprite = flameAttackSprite;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        nextPosition = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
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
            nextPosition = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z);
            canMove = false;
        }

    }

    void CallNextPosition()
    {
        nextPosition = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z);
    }

    public void OnTick()
    {
        canMove = true;
        activeTime += 1;

        if(burningBullet)
        {
            burningParticlesInstance = Instantiate(burningParticles, this.transform, worldPositionStays:false);
        }

        if (activeTime == bulletRange)
        {
            RemoveProjectile();
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().Damage(damage);

            if(burningBullet)
            {
                collision.GetComponent<Enemy>().burnDamage += 4;
                collision.GetComponent<Enemy>().burnt = true;
            }

            

            if(!piercing) RemoveProjectile();

        }
        //if a projectile hits a obstacle destroy it 
        //TODO: Make tag specifically for wall obstacle
        else if(collision.gameObject.CompareTag("Obstacle"))
        {
            RemoveProjectile();
        }
    }
    public void RemoveProjectile()
    {
        gameObject.transform.DOKill();
        ConductorV2.instance.projectileEvent.Remove(trigger);
        Destroy(gameObject);
    }
}
