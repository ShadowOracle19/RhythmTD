using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pickup : MonoBehaviour
{
    public PickupCreator pickup;

    public List<Vector3> path;
    private float speed = 1;
    public float timer;
    public Vector3 currentPositionHolder;

    public bool dontMove;
    private bool otherBeatMove = false;
    public int barTracker = 0;

    public UnityEvent trigger;

    public Vector3 nextPosition;

    float time = 1;
    [SerializeField] private SpriteRenderer _renderer;

    public Tile tileInFront;

    bool playOnce = false;
    
    // Start is called before the first frame update
    void Start()
    {
        dontMove = true;

        nextPosition = new Vector3(transform.position.x - 1f, transform.position.y, transform.position.z);

        nextPosition = new Vector3(transform.position.x - 1f, 0.5f, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime * 5;
        _renderer.color = Color.Lerp(_renderer.color, Color.white, Time.deltaTime / time);
        
        Movement();
    }

    public virtual void OnTick()
    {
        //enemy movement pattern handler
        switch (pickup.movementPattern)
        {
            case PickupMovementPattern.everyBeat:
                dontMove = false;
                break;

            case PickupMovementPattern.everyOtherBeat:
                otherBeatMove = !otherBeatMove;
                dontMove = otherBeatMove;
                break;

            case PickupMovementPattern.dontMove:
                if(!playOnce)
                {
                    playOnce = true;
                    int rand = Random.Range(0, GridManager.Instance.tiles.Count - 1);
                    transform.position = GridManager.Instance.tiles[rand].transform.position;

                }
                dontMove = true;
                break;

            case PickupMovementPattern.everyTwoBeats:
                if (ConductorV2.instance.beatTrack == 2 || ConductorV2.instance.beatTrack == 4)
                {
                    dontMove = false;
                }
                break;

            case PickupMovementPattern.oncePerBar:
                if (ConductorV2.instance.beatTrack == 4)
                {
                    dontMove = false;
                }
                break;

            case PickupMovementPattern.onceEveryTwoBars:
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

    // Collision with cursor
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            //Pickup Resource Handling
            switch (pickup.pickupType)
            {
                case PickupType.score:
                    ComboManager.Instance.score += pickup.score;
                    break;

                case PickupType.health:
                    if (GameManager.Instance._currentHealth > GameManager.Instance._maxHealth)
                    {
                        GameManager.Instance._currentHealth += pickup.health;
                    } 
                    else
                    {
                        ComboManager.Instance.score += pickup.score;
                    }
                    break;

                case PickupType.energy:
                    CombatManager.Instance.resourceNum += pickup.energy;
                    break;

                default:
                    break;
            }

            RemovePickup();
        }
    }

    // Pickup removal
    public void RemovePickup()
    {
        if (playOnce) return;
        playOnce = true;

        //CombatManager.Instance.pickupTotal -= 1;
        
        ConductorV2.instance.pickupEvent.Remove(trigger);
        Spawner.Instance.pickups.Remove(this);
        Destroy(gameObject);
    }

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
            nextPosition = new Vector3(transform.position.x + 1f, transform.position.y, transform.position.z);
        }
    }
}
