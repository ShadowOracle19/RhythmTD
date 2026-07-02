using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Unity.Mathematics;
using Random=UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    public EnemyCreator enemy;
    public EnemyEffect enemyEffect;

    public EnemyState enemyState;

    public List<Vector3> path;
    private float speed = 1;
    public float timer;
    public Vector3 currentPositionHolder;

    public bool dontMove;
    private bool otherBeatMove = false;
    public int barTracker = 0;

    public int currentHealth;

    public int _currentDamage = 1;

    public UnityEvent trigger;

    public Vector3 nextPosition;

    //Adds a random amount of jitter to sprite movement.
    [SerializeField] private float defeatSpinSpeed = UnityEngine.Random.Range(-240f, 240f); 

    float time = 1;
    [SerializeField] private SpriteRenderer _renderer;

    //Controls damage SFX.
   // public float soundCooldown = 0.5f;
   // private float nextPlayTime = 0f;
    private bool deathSoundTriggered = false;

    public Tile tileInFront;

    bool playOnce = false;

    public int burnDamage = 0;
    public bool burnt = false;
    public bool isStunned = false;
    public int isStunnedCounter = 0;

    public bool teleported = false;

    //confusing order
    public bool confusingOrderActive = false;

    // dynamic mover
    LayerMask tileMask;
    LayerMask obstacleMask;
    public bool obstacleFound = false;

    //Animation
    public Animator animator;

    //Death
    private bool isDead = false;
    private bool deathParticleTriggered = false;
    int deathCount = 0;

    [Header("SFX")]
    public AudioClip enemyHurtSfx;
    public AudioClip enemyDeathSfx;

    [Header("PFX")]
    [SerializeField] private ParticleSystem burnParticles;
    private ParticleSystem burnParticlesInstance;

    [SerializeField] private ParticleSystem clashParticles;
    private ParticleSystem clashParticlesInstance;

    public ParticleSystem hitParticles;
    [SerializeField] private ParticleSystem damageEffect;

    [Header("Sound Timing")]
    private float soundTimer = 0.0f;
    //NOTE: It would be great to make a custom property drawer for this at some point that only snaps between powers of 2 (but it should also include 1 if the crotchet isn't being divided)
    //NOTE: It might also be good to put this in a manager elsewhere where we can control multiple sound delays
    [Tooltip("1 = Nearest quarter note. It's recommended you set this value to a power of 2. Ex. 4 = Nearest sixteenth note")]
    [Range(1,64)] 
    public int crotchetDivisor = 4;

    public virtual void Start()
    {
        tileMask = LayerMask.GetMask("Stage");
        obstacleMask = LayerMask.GetMask("Obstacle");
        currentHealth = enemy.maxHealth;

        dontMove = true;

        nextPosition = new Vector3(transform.position.x - 1f, transform.position.y, transform.position.z);

        //animator = GetComponent<Animator>();
        nextPosition = new Vector3(transform.position.x - 1f, 0.5f, transform.position.z);

        //Set Animation BPM
        AnimationManager.instance.SetAnimSpeed(animator, 80);

        soundTimer = 0.0f;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (isDead)
        {
            gameObject.transform.DOKill();
            if(deathCount == 6)
            {
                RemoveEnemy();
            }
            GetComponent<BoxCollider>().enabled = false;
            dontMove = true;
            return;
        }
        time -= Time.deltaTime * 5;
        _renderer.color = Color.Lerp(_renderer.color, Color.white, Time.deltaTime / time);

        //Moves the sprite by adding a small random movement to its z-position.
        //this.transform.Rotate(0, 0, -defeatSpinSpeed * Time.deltaTime);

        Movement();

    }

    void OnDisable()
    {
        Debug.Log("PrintOnDisable: script was disabled");
    }

    public virtual void OnTick()
    {
        
        if (burnt)
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
            //temp stun effect
            clashParticlesInstance = Instantiate(clashParticles, this.transform.position, Quaternion.identity);
            isStunnedCounter += 1;
            if(isStunnedCounter > 2)
            {
                isStunnedCounter = 0;
                isStunned = false;
            }
            return;
        }

        if(isDead)
        {
            deathCount += 1;
        }

        if(confusingOrderActive)
        {
            int rand = Random.Range(0, 4);
            
            switch(rand)
            {
                case 0:

                    nextPosition = new Vector3(transform.position.x, 0.5f, transform.position.z + 1);

                    if (nextPosition.z >= 2.5f)//if hit top of the map
                    {
                        nextPosition = new Vector3(transform.position.x, 0.5f, transform.position.z - 1);
                    }
                    break;

                case 1:

                    nextPosition = new Vector3(transform.position.x, 0.5f, transform.position.z - 1);

                    if (nextPosition.z <= -3.5f)//if hit bottom of the map
                    {
                        nextPosition = new Vector3(transform.position.x, 0.5f, transform.position.z + 1);
                    }
                    break;

                case 2:
                    nextPosition = new Vector3(transform.position.x + 1, 0.5f, transform.position.z);
                    break;

                case 3:
                    nextPosition = new Vector3(transform.position.x - 1, 0.5f, transform.position.z);
                    break;
            }
            confusingOrderActive = false;
            dontMove = false;
            return;
        }

        switch (enemyState)
        {
            case EnemyState.Walk:
                EnemyPathingPatterns();
                break;
            case EnemyState.Attack:
                Clash();
                break;
            default:
                break;
        }

    }

    public void EnemyPathingPatterns()
    {
        //enemy movement pattern handler
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

                if (otherBeatMove)
                {
                    int randYPos = Random.Range(0, 2) * 2 - 1;
                    float _rand;
                    if (randYPos == -1)
                    {
                        _rand = -1f;
                    }
                    else
                    {
                        _rand = 1f;
                    }

                    nextPosition = new Vector3(transform.position.x, 0.5f, transform.position.z + _rand);

                    if (nextPosition.z >= 1.5f || nextPosition.z <= -2.5f)//if hit top of bottom of the map
                    {
                        nextPosition = new Vector3(transform.position.x - 1f, 0.5f, transform.position.z);
                    }
                    dontMove = false;
                }
                else
                {
                    nextPosition = new Vector3(transform.position.x - 2f, 0.5f, transform.position.z);
                    dontMove = false;
                }
                break;

            case EnemyMovementPattern.moveThenCast:
                if (ConductorV2.instance.beatTrack == 2 || ConductorV2.instance.beatTrack == 4)
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
                if (!playOnce)
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
                    if (tileInFront != null && tileInFront.placedTower != null)
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
                if (ConductorV2.instance.beatTrack == 4)
                {
                    barTracker += 1;
                    if (barTracker == 2)
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

    //Enemies eyes to see if a tile or obstacle is around them
    public void EnemyDetection()
    {
        //if enemy is infront of a obstacle tile
        if(obstacleFound)
        {
            RaycastHit hit;
            RaycastHit diagHit;

            Debug.Log("Obstacle in front");

            //If Tile above is a valid stage tile and the next tile in front isnt an obstacle
            if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward ), out hit, 1, tileMask) 
                && !(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward + Vector3.left), out diagHit, 1, obstacleMask)))
            {
                
                nextPosition = new Vector3(transform.position.x, 0.5f, transform.position.z + 1);
                dontMove = true;
                return;
            }
            //If tile below is a valid stage tile
            else if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.back), out hit, 1, tileMask) &&
                !(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.back + Vector3.left), out diagHit, 1, obstacleMask)))
            {
                nextPosition = new Vector3(transform.position.x, 0.5f, transform.position.z - 1);
                dontMove = true;
                return;

            }
        }
    }

    //public void Clash(ClashStrength clashStrength)
    //{
    //    switch (clashStrength)
    //    {
    //        case ClashStrength.Weak:
    //            tileInFront.placedTower.GetComponent<Tower>().Damage(_currentDamage);
    //            clashParticlesInstance = Instantiate(clashParticles, this.transform.position, Quaternion.identity); // Create instance of the enemy clash particle effect
    //            Kill();
    //            break;
    //        case ClashStrength.Medium:
    //            tileInFront.placedTower.GetComponent<Tower>().Damage(tileInFront.placedTower.GetComponent<Tower>().towerInfo.towerHealth);
    //            clashParticlesInstance = Instantiate(clashParticles, this.transform.position, Quaternion.identity); // Create instance of the enemy clash particle effect
    //            Kill();
    //            break;
    //        case ClashStrength.High:
    //            tileInFront.placedTower.GetComponent<Tower>().Damage(tileInFront.placedTower.GetComponent<Tower>().towerInfo.towerHealth);
    //            break;
    //        case ClashStrength.Immune:
    //            break;
    //        default:
    //            break;
    //    }
    //}

    public void Clash()
    {
        tileInFront.placedTower.GetComponent<Tower>().Damage(1);

        if (tileInFront != null && tileInFront.placedTower == null)
        {
            enemyState = EnemyState.Walk;
            dontMove = true;
            return;
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

    public virtual void Movement()
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

            EnemyDetection();
            //gameObject.transform.DOMove(nextPosition, ConductorV2.instance.crotchet)
            //    .SetEase(Ease.OutSine)
            //    .onComplete = CallNextPosition;
            if (tileInFront != null && tileInFront.placedTower != null)
            {
                //Clash(enemy.clashStrength);
                enemyState = EnemyState.Attack;
                dontMove = true;
                return;
            }
            
        }
        
    }
    

    public virtual void Damage(int damage)
    {
        _renderer.color = Color.red;
        time = 1;
        currentHealth -= damage;   

        if (currentHealth <= 0)
        {
            if (currentHealth <= 0 && deathParticleTriggered == false)
            {
                damageEffect = Instantiate(damageEffect, this.transform, worldPositionStays: false); 
                deathParticleTriggered = true;
            }
            
            Kill();
        }
    }

    //waits for a duration after a projectile makes contact with an enemy to ensure it aligns with the music
    public IEnumerator PlaySoundOnBeat(float timeImpacted, float timeFired)
    {
        //NOTE: Maybe halve the crotchet to align the sound with the nearest half beat?
        soundTimer = (ConductorV2.instance.crotchet / crotchetDivisor) - (math.fmod((timeImpacted - timeFired), (ConductorV2.instance.crotchet/crotchetDivisor)));

        bool waiting = true;

        while (waiting)
        {
            waiting = false;
            yield return new WaitForSecondsRealtime(soundTimer);
        }

        AudioManager.instance.PlaySound(enemyHurtSfx, this.gameObject.transform, 1.0f);
        //Debug.Log("Enemy Hurt Sound Played");

        ParticleSystem particlesInstance = Instantiate(hitParticles, transform.position, Quaternion.identity, this.gameObject.transform);
        
        StopCoroutine(this.PlaySoundOnBeat(timeImpacted, timeFired));
    }

    public void Kill()
    { 
        isDead = true;
        if (enemy.onDeathEffect)
        {
            enemyEffect.UseEffect();
        }

        if (deathSoundTriggered == false)
        {
            //play death sound 
            StartCoroutine(PlaySoundOnBeat(ConductorV2.instance.songPosition, 0.0f));
            deathSoundTriggered = true;
        }    
        
        animator.SetBool("IsKilled",true); //Play death animation
    }


    public void RemoveEnemy()
    {
        if (playOnce) return;
        playOnce = true;

        if(!GameManager.Instance.currentEncounter.isBossBattle)
        {
            CombatManager.Instance.enemyTotal -= 1;
            CombatManager.Instance.enemiesDefeated += 1;
        }

        GameManager.Instance.pointHolder.Add(enemy.onDeathPoints);
        
        ConductorV2.instance.enemyEvent.Remove(trigger);
        //EnemySpawner.Instance.enemies.Remove(this);
        Spawner.Instance.enemies.Remove(this);
        GetComponent<BoxCollider>().enabled = false;
        gameObject.transform.position = new Vector3(1000, 1000, 1000);

        DestroyEnemy();
    }

    public void DestroyEnemy()
    {
        Destroy(gameObject, 1);

    }
}

public enum EnemyState
{
    Walk, Attack
}
