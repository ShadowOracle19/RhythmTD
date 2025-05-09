using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    public EnemyCreator enemy;
    public EnemyEffect enemyEffect;
    public AudioSource enemyDeathSFX;

    public List<Vector3> path;
    private float speed = 1;
    float timer;
    public Vector3 currentPositionHolder;

    public bool dontMove;
    private bool otherBeatMove = false;
    public int barTracker = 0;

    public int currentHealth;

    public UnityEvent trigger;

    [SerializeField] private Vector3 nextPosition;

    float time = 1;
    [SerializeField] private SpriteRenderer _renderer;

    public Tile tileInFront;

    bool playOnce = false;

    public int burnDamage = 0;
    public bool burnt = false;
    public bool isStunned = false;
    public int isStunnedCounter = 0;

    // PFX
    [SerializeField] private ParticleSystem burnParticles;
    private ParticleSystem burnParticlesInstance;

    [SerializeField] private ParticleSystem clashParticles;
    private ParticleSystem clashParticlesInstance;


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = enemy.maxHealth;

        dontMove = true;

        nextPosition = new Vector3(transform.position.x - 1f, transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime * 5;
        _renderer.color = Color.Lerp(_renderer.color, Color.white, Time.deltaTime / time);

        

        Movement();

    }

    public void OnTick()
    {
        if(burnt)
        {
            Damage(burnDamage);
            burnParticlesInstance = Instantiate(burnParticles, this.transform, worldPositionStays:false); // Create instance of the burn particle effect
            burnDamage -= 1;

            if(burnDamage == 0)
            {
                burnt = false;
            }
        }

        if(isStunned)
        {
            isStunnedCounter += 1;
            if(isStunnedCounter > 2)
            {
                isStunnedCounter = 0;
                isStunned = false;
            }
            return;
        }

        switch (enemy.movementPattern)
        {
            case EnemyMovementPattern.everyBeat:
                dontMove = false;
                break;

            case EnemyMovementPattern.everyOtherBeat:
                otherBeatMove = !otherBeatMove;
                dontMove = otherBeatMove;
                break;

            case EnemyMovementPattern.random:
                otherBeatMove = Random.value < 0.5f;

                if(otherBeatMove)
                {
                    int randYPos = Random.Range(0, 2) * 2 - 1;
                    float _rand;
                    if(randYPos == -1)
                    {
                        _rand = -1.2f;
                    }
                    else
                    {
                        _rand = 1.2f;
                    }

                    nextPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z + _rand);

                    if(nextPosition.z >= 2.5f || nextPosition.z <= -3)//if hit top of bottom of the map
                    {
                        nextPosition = new Vector3(transform.position.x - 1f, transform.position.y, transform.position.z);
                    }
                    dontMove = false;
                }
                else
                {
                    nextPosition = new Vector3(transform.position.x - 2f, transform.position.y, transform.position.z);
                    dontMove = false;
                }
                break;

            case EnemyMovementPattern.moveThenCast:
                if(ConductorV2.instance.beatTrack == 2 || ConductorV2.instance.beatTrack == 4)
                {
                   dontMove = false;
                }
                if (ConductorV2.instance.beatTrack == 4)
                {
                    Debug.Log("Effect");
                    enemyEffect.UseEffect();
                }
                break;

            case EnemyMovementPattern.dontMove:
                if(!playOnce)
                {
                    playOnce = true;
                    int rand = Random.Range(0, GridManager.Instance.tiles.Count - 1);
                    transform.position = GridManager.Instance.tiles[rand].transform.position;

                }
                dontMove = true;
                break;

            case EnemyMovementPattern.everyTwoBeats:
                if (ConductorV2.instance.beatTrack == 2 || ConductorV2.instance.beatTrack == 4)
                {
                    if(tileInFront != null && tileInFront.placedTower != null)
                    {
                        dontMove = true;
                        enemyEffect.UseEffect();
                    }
                    else
                    {
                        dontMove = false;

                    }
                }
                break;

            case EnemyMovementPattern.oncePerBar:
                if (ConductorV2.instance.beatTrack == 4)
                {
                    dontMove = false;
                }
                break;

            case EnemyMovementPattern.onceEveryTwoBars:
                if(ConductorV2.instance.beatTrack == 4)
                {
                    barTracker += 1;
                    if(barTracker == 2)
                    {
                        dontMove = false;
                        barTracker = 0;
                    }
                }
                break;

            default:
                break;
        }

    }

    /*
     * TODO: Update clash system to hit a tower each beat instead of killing itself on a tower
     */ 

    public void Clash(ClashStrength clashStrength)
    {
        switch (clashStrength)
        {
            case ClashStrength.Weak:
                tileInFront.placedTower.GetComponent<Tower>().Damage(1);
                clashParticlesInstance = Instantiate(clashParticles, this.transform.position, Quaternion.identity); // Create instance of the enemy clash particle effect
                Kill();
                break;
            case ClashStrength.Medium:
                tileInFront.placedTower.GetComponent<Tower>().Damage(tileInFront.placedTower.GetComponent<Tower>().towerInfo.towerHealth);
                clashParticlesInstance = Instantiate(clashParticles, this.transform.position, Quaternion.identity); // Create instance of the enemy clash particle effect
                Kill();
                break;
            case ClashStrength.High:
                tileInFront.placedTower.GetComponent<Tower>().Damage(tileInFront.placedTower.GetComponent<Tower>().towerInfo.towerHealth);
                break;
            case ClashStrength.Immune:
                break;
            default:
                break;
        }
    }

    #region pathing
    //Pathing Function
    //voiddontMovement()
    //{
    //    timer += Time.deltaTime * speed;
    //    if (gameObject.transform.position != currentPositionHolder)
    //    {
    //        gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, currentPositionHolder, timer);
    //    }
    //    else
    //    {
    //        if (currentNode < path.Count - 1)
    //        {
    //           dontMove = false;
    //            currentNode++;
    //            CheckNode();
    //        }
    //    }
    //}

    //void CheckNode()
    //{
    //    timer = 0;
    //    currentPositionHolder = path[currentNode];
    //}
    #endregion

    public void Movement()
    {
        
        timer += Time.deltaTime * speed;
        if (gameObject.transform.position != nextPosition && !dontMove)
        {
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, nextPosition, timer);
        }
        else
        {
           dontMove = true;
            timer = 0;
            nextPosition = new Vector3(transform.position.x - 1f, transform.position.y, transform.position.z);
        }
        if (tileInFront != null && tileInFront.placedTower != null)
        {
            Debug.Log("Clash!");
            Clash(enemy.clashStrength);
            dontMove = true;
            return;
        }
    }

    public void Damage(int damage)
    {
        _renderer.color = Color.red;
        time = 1;
        currentHealth -= damage;
        enemyDeathSFX.Play();
        if (currentHealth <= 0)
        {
            Kill();
        }
    }

    public void Kill()
    {
        if(enemy.onDeathEffect)
        {
            enemyEffect.UseEffect();
        }

        RemoveEnemy();
    }


    public void RemoveEnemy()
    {
        if (playOnce) return;
        playOnce = true;
        CombatManager.Instance.enemyTotal -= 1;
        ConductorV2.instance.triggerEvent.Remove(trigger);
        EnemySpawner.Instance.enemies.Remove(this);
        Destroy(gameObject);
    }
}
