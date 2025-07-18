using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Boss_1_Commander : Enemy
{
    [Header("Boss Info")]
    [SerializeField] private CommanderStates currentState;
    [SerializeField] private PhaseIndex currentStateIndex;
    [SerializeField] private CommanderStates previousState;
    [SerializeField] private int damageThreshold;
    [SerializeField] private GameObject lifter, runner, sorter;
    private bool phaseStarted = false;

    //movement
    private bool movementInProgress = false;
    private Vector3 finalPos;

    //phase one
    int measureCounter;

    //phase two
    bool lanesFilled = false;

    // Start is called before the first frame update
    public override void Start()
    {
        //set current state
        currentState = CommanderStates.phase1;
        currentStateIndex = PhaseIndex.A;
        previousState = currentState;
    }

    // Update is called once per frame
    public override void Update()
    {
        
    }

    public override void OnTick()
    {
        switch (currentState)
        {
            case CommanderStates.phase1:
                PhaseOne();
                break;
            case CommanderStates.phase2:
                break;
            case CommanderStates.phase3:
                break;
            case CommanderStates.phase4:
                break;
            case CommanderStates.damagePhase:
                break;
            case CommanderStates.winPhase:
                GameManager.Instance.WinLevel();
                break;
            case CommanderStates.lossPhase:
                GameManager.Instance.GameOver();
                break;
            default:
                break;
        }
    }

    private void PhaseOne()
    {
        switch (currentStateIndex)
        {
                //Spawn enemy at nearest tower
                //Spawn enemy on random tile every 2 measures for X measures
            case PhaseIndex.A:
                if(!phaseStarted)
                {
                    Debug.Log("Phase 1 A");
                    float yPos = GetClosestTower().position.y;
                    EnemySpawner.Instance.ForceEnemySpawnDynamic(yPos, lifter);
                    phaseStarted = true;
                }
                //check if measure is an even number
                if(ConductorV2.instance.measureTrack % 2 == 0 && ConductorV2.instance.beatTrack == 4)
                {
                    EnemySpawner.Instance.SpawnUnitOnRandomTile(lifter);
                    measureCounter += 1;
                    //change to phase 1B
                    if (measureCounter == 4)
                    {
                        currentStateIndex = PhaseIndex.B;
                        phaseStarted = false;
                        Debug.Log("Phase 1 B");
                    }

                }
                
                break;

                //finds lane with least amount of towers
            case PhaseIndex.B:
                if(!phaseStarted)
                {
                    measureCounter = 0;
                    phaseStarted = true;
                }
                //if not all lanes filled and its every 2 measure
                if(!lanesFilled && ConductorV2.instance.measureTrack % 2 == 0 && ConductorV2.instance.beatTrack == 1)
                {
                    EnemySpawner.Instance.ForceEnemySpawnDynamic(FindEmptiestLane(), lifter);
                    measureCounter += 1;
                }
                //if all lanes filled and its every 2 measures
                else if(lanesFilled && ConductorV2.instance.measureTrack % 2 == 0 && ConductorV2.instance.beatTrack == 1)
                {
                    EnemySpawner.Instance.SpawnUnitOnRandomTile(lifter);
                    EnemySpawner.Instance.SpawnUnitOnRandomTile(lifter);
                    measureCounter += 1;
                }
                //change to phase 1B
                if (measureCounter == 4)
                {
                    Debug.Log("Phase 1 C");
                    currentStateIndex = PhaseIndex.C;
                    phaseStarted = false;
                }
                break;

                //spawns 6 enemies in each tile
                //if all enemies die it begins damage phase
            case PhaseIndex.C:
                if (!phaseStarted)
                {
                    measureCounter = 0;
                    EnemySpawner.Instance.ForceEnemySpawnDynamic(1.5f, lifter);
                    EnemySpawner.Instance.ForceEnemySpawnDynamic(0.5f, lifter);
                    EnemySpawner.Instance.ForceEnemySpawnDynamic(-0.5f, lifter);
                    EnemySpawner.Instance.ForceEnemySpawnDynamic(-1.5f, lifter);
                    EnemySpawner.Instance.ForceEnemySpawnDynamic(-2.5f, lifter);
                    EnemySpawner.Instance.ForceEnemySpawnDynamic(-3.5f, lifter);
                    phaseStarted = true;
                    
                }

                //player lives
                if(CombatManager.Instance.enemyTotal == 1 && phaseStarted)
                {
                    Debug.Log("Damage Phase");
                    currentStateIndex = PhaseIndex.A;
                    currentState = CommanderStates.damagePhase;
                    previousState = CommanderStates.phase1;
                }
                
                break;

            default:
                break;
        }
    }

    public override void Movement()
    {
        
    }

    public void MoveTo(Vector3 finalPos)
    {
        Vector3 nextPosition = Vector3.zero;

        if (!movementInProgress)
        {
            movementInProgress = true;
            nextPosition = transform.position + GetDirection(transform.position, finalPos);
        }
        

        if (transform.position != nextPosition)
        {
            timer += Time.deltaTime * 1;
            gameObject.transform.position = Vector3.Lerp(transform.position, nextPosition, timer);
        }
        else
        {
            nextPosition = transform.position + GetDirection(transform.position, finalPos);
            timer = 0;

            //if reached final position
            if (transform.position == finalPos)
            {
                movementInProgress = false;
            }
        }
    }

    Vector3 GetDirection(Vector3 point_a, Vector3 point_b)
    {
        Vector3 direction = point_b - point_a;
        direction.Normalize();
        return direction;
    }

    float FindEmptiestLane()
    {
        float emptiestLane = 0;
        float[] array = new float[6];
        LayerMask layer = LayerMask.GetMask("Tower");

        RaycastHit[] laneCheck;

        laneCheck = Physics.RaycastAll(new Vector3(9.5f, 0.5f, 1.5f), Vector3.left, Mathf.Infinity, layer);
        array[0] = laneCheck.Length;

        laneCheck = Physics.RaycastAll(new Vector3(9.5f, 0.5f, 0.5f), Vector3.left, Mathf.Infinity, layer);
        array[1] = laneCheck.Length;

        laneCheck = Physics.RaycastAll(new Vector3(9.5f, 0.5f, -0.5f), Vector3.left, Mathf.Infinity, layer);
        array[2] = laneCheck.Length;

        laneCheck = Physics.RaycastAll(new Vector3(9.5f, 0.5f, -1.5f), Vector3.left, Mathf.Infinity, layer);
        array[3] = laneCheck.Length;

        laneCheck = Physics.RaycastAll(new Vector3(9.5f, 0.5f, -2.5f), Vector3.left, Mathf.Infinity, layer);
        array[4] = laneCheck.Length;

        laneCheck = Physics.RaycastAll(new Vector3(9.5f, 0.5f, -3.5f), Vector3.left, Mathf.Infinity, layer);
        array[5] = laneCheck.Length;

        emptiestLane = EnemySpawner.Instance.spawnTiles[GetIndexOfLowestValue(array)].GetComponent<Tile>().zPos;

        if (array[GetIndexOfLowestValue(array)] > 0)
        {
            lanesFilled = true;
        }

        return emptiestLane;
    }

    public int GetIndexOfLowestValue(float[] arr)
    {
        float value = float.PositiveInfinity;
        int index = -1;
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] < value)
            {
                index = i;
                value = arr[i];
            }
        }
        return index;
    }

    Transform GetClosestTower()
    {
        Transform towerParent = CombatManager.Instance.towersParent;
        Transform tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;

        foreach (Transform tower in towerParent)
        {
            float dist = Vector3.Distance(tower.position, currentPos);
            if (dist < minDist)
            {
                tMin = tower;
                minDist = dist;
            }
        }

        return tMin;
    }
}

public enum CommanderStates
{
    phase1, phase2, phase3, phase4, damagePhase, winPhase, lossPhase
}

public enum PhaseIndex
{
    A, B, C
}
