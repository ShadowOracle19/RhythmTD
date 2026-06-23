using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class Projectile : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public int speed = 1;
    public float timer;
    public float startXPosition;
    public float nextXPosition;
    public Vector3 nextPosition;
    public int bulletRange = 0;
    int activeTime;
    public bool piercing;

    public int damage = 1;

    public GameObject towerFiredFrom;

    public float songProgress = 0.0f; // progress of current song expressed in time
    public float movementProgress = 0.0f;
    public float timeAtFire;
    public float timeAtEnd;

    public UnityEvent trigger;

    public bool burningBullet = false;
    public Sprite flameAttackSprite;
    [SerializeField] private ParticleSystem burningParticles;
    private ParticleSystem burningParticlesInstance;

    public bool teleported = false;

    public void InitializeProjectile(int range, GameObject firedFrom, int _damage, bool isPiercing, float timeAtAttack)
    {
        bulletRange = range;
        towerFiredFrom = firedFrom;
        damage = _damage;
        piercing = isPiercing;
        timeAtFire = timeAtAttack;
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        startXPosition = gameObject.transform.position.x;
        nextXPosition = transform.position.x + bulletRange;

        timeAtEnd = timeAtFire + (ConductorV2.instance.crotchet * bulletRange);

        movementProgress = 0.0f;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        songProgress = ConductorV2.instance.songPosition;

        if (movementProgress >= 1.0f)
        {
            RemoveProjectile();
            return;
        }

        movementProgress = (((songProgress - timeAtFire) * speed) / (timeAtEnd - timeAtFire));
        nextPosition = new Vector3(Mathf.Lerp(startXPosition, nextXPosition, movementProgress), transform.position.y, transform.position.z);
        gameObject.transform.position = nextPosition;
    }

    public virtual void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().Damage(damage);
            collision.GetComponent<Enemy>().StartCoroutine(collision.GetComponent<Enemy>().PlaySoundOnBeat(songProgress, timeAtFire));

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

//distance = speed / time