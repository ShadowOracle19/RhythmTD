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
    private int currentDamage;
    [SerializeField] private GameObject lifter, runner, sorter;
    private bool phaseStarted = false;

    private Vector3 originPos;

    //movement
    private bool movementInProgress = false;
    private Vector3 finalPos;

    //phase one
    int measureCounter;

    //phase two
    bool lanesFilled = false;

    //phase three
    [SerializeField] private GameObject comandeerCursor;
    [SerializeField]private bool phase3Start = false;
    [SerializeField]private bool cursorMovementInProgress = false;
    [SerializeField]private bool cursorDontMove = false;
    [SerializeField]private Vector3 cursorFinalPos;
    [SerializeField]private Vector3 cursorNextPos;
    [SerializeField] private float cursorTimer = 0;

    // Start is called before the first frame update
    public override void Start()
    {
        //set current state
        currentState = CommanderStates.phase1;
        currentStateIndex = PhaseIndex.A;
        previousState = currentState;

        originPos = transform.position;
    }

    // Update is called once per frame
    public override void Update()
    {
        Movement();

        CursorMovement();

        if(currentState == CommanderStates.damagePhase)
        {
            GetComponent<BoxCollider>().enabled = true;
        }
        else
        {
            GetComponent<BoxCollider>().enabled = false;
        }
    }

    public override void OnTick()
    {
        dontMove = false;
        cursorDontMove = false;
        switch (currentState)
        {
            case CommanderStates.phase1:
                PhaseOne();
                break;
            case CommanderStates.phase2:
                PhaseTwo();
                break;
            case CommanderStates.phase3:
                PhaseThree();
                break;
            case CommanderStates.phase4:
                PhaseFour();
                break;
            case CommanderStates.damagePhase:
                DamagePhase();
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

    private void DamagePhase()
    {
        //once reached end of phase reset position back to origin
        //if reached damage threshold auto go to next phase and to origin
        switch (currentStateIndex)
        {
            //move to (6.5f, 0.5f, 1.5f)
            case PhaseIndex.A:
                MoveTo(new Vector3(6.5f, 0.5f, 1.5f));

                //next phase
                if(transform.position == new Vector3(6.5f, 0.5f, 1.5f))
                {
                    if (ConductorV2.instance.measureTrack % 2 == 0 && ConductorV2.instance.beatTrack == 4)
                    {
                        measureCounter += 1;
                        //change to phase 1B
                        if (measureCounter == 2)
                        {
                            measureCounter = 0;
                            currentStateIndex = PhaseIndex.B;
                            return;
                        }

                    }
                }
                break;
            //move to (6.5f, 0.5f, -3.5f)
            case PhaseIndex.B:

                MoveTo(new Vector3(6.5f, 0.5f, -3.5f));

                //next phase
                if (transform.position == new Vector3(6.5f, 0.5f, -3.5f))
                {
                    if (ConductorV2.instance.measureTrack % 2 == 0 && ConductorV2.instance.beatTrack == 4)
                    {
                        measureCounter += 1;
                        //change to phase 1B
                        if (measureCounter == 2)
                        {
                            measureCounter = 0;
                            currentStateIndex = PhaseIndex.C;
                            return;
                        }

                    }
                }
                break;
            //move to (10.5f, 0.5f, -3.5f)
            case PhaseIndex.C:

                MoveTo(new Vector3(10.5f, 0.5f, -3.5f));

                //next phase and if damage threshold didnt get reached
                if (transform.position == new Vector3(10.5f, 0.5f, -3.5f))
                {
                    MoveTo(originPos);
                    measureCounter = 0;
                    currentState = previousState;
                    currentStateIndex = PhaseIndex.A;
                    
                }
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
                    //float yPos = GetClosestTower().position.y;
                    //EnemySpawner.Instance.ForceEnemySpawnDynamic(yPos, lifter);
                    EnemySpawner.Instance.SpawnUnitOnRandomTile(lifter);
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
                        measureCounter = 0;
                        currentStateIndex = PhaseIndex.B;
                        phaseStarted = false;
                        Debug.Log("Phase 1 B");
                        return;
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

                    //change to phase 1B
                    if (measureCounter == 4)
                    {
                        Debug.Log("Phase 1 C");
                        currentStateIndex = PhaseIndex.C;
                        phaseStarted = false;
                        return;
                    }
                }
                //if all lanes filled and its every 2 measures
                else if(lanesFilled && ConductorV2.instance.measureTrack % 2 == 0 && ConductorV2.instance.beatTrack == 1)
                {
                    EnemySpawner.Instance.SpawnUnitOnRandomTile(lifter);
                    EnemySpawner.Instance.SpawnUnitOnRandomTile(lifter);
                    measureCounter += 1;

                    //change to phase 1B
                    if (measureCounter == 4)
                    {
                        measureCounter = 0;
                        Debug.Log("Phase 1 C");
                        currentStateIndex = PhaseIndex.C;
                        phaseStarted = false;
                        return;
                    }
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
                    MoveTo(new Vector3(10.5f, 0.5f, 1.5f));
                }
                Enemy[] children = CombatManager.Instance.enemiesParent.GetComponentsInChildren<Enemy>();
                //player lives
                if(children.Length == 1 && phaseStarted)
                {
                    measureCounter = 0;
                    Debug.Log("Damage Phase");
                    currentStateIndex = PhaseIndex.A;
                    currentState = CommanderStates.damagePhase;
                    previousState = CommanderStates.phase1;
                    phaseStarted = false;
                    return;
                }
                
                break;

            default:
                break;
        }
    }

    private void PhaseTwo()
    {
        switch (currentStateIndex)
        {
            //spawns 1 lifter or runner every two beats
            case PhaseIndex.A:
                if(ConductorV2.instance.beatTrack == 2)
                {
                    EnemySpawner.Instance.SpawnUnitOnRandomTile(lifter);
                }
                else if(ConductorV2.instance.beatTrack == 4)
                {
                    EnemySpawner.Instance.SpawnUnitOnRandomTile(runner);
                }
                if (ConductorV2.instance.measureTrack % 2 == 0 && ConductorV2.instance.beatTrack == 4)
                {
                    measureCounter += 1;
                    //change to phase 1B
                    if (measureCounter == 4)
                    {
                        currentStateIndex = PhaseIndex.B;
                        measureCounter = 0;
                        return;
                    }

                }
                break;
            case PhaseIndex.B:
                if (ConductorV2.instance.beatTrack == 2)
                {
                    EnemySpawner.Instance.SpawnUnitOnRandomTile(lifter);
                }
                else if (ConductorV2.instance.beatTrack == 4)
                {
                    EnemySpawner.Instance.SpawnUnitOnRandomTile(runner);
                }
                if (ConductorV2.instance.measureTrack % 2 == 0 && ConductorV2.instance.beatTrack == 4)
                {
                    EnemySpawner.Instance.SpawnUnitOnRandomTile(sorter);
                    measureCounter += 1;
                    //change to phase 1B
                    if (measureCounter == 4)
                    {
                        currentStateIndex = PhaseIndex.B;
                        measureCounter = 0;
                        return;
                    }

                }
                break;
            case PhaseIndex.C:
                if (!phaseStarted)
                {
                    measureCounter = 0;
                    phaseStarted = true;
                    MoveTo(new Vector3(10.5f, 0.5f, 1.5f));
                }

                int rand = Random.Range(0, 3);

                if(rand == 0)
                {
                    EnemySpawner.Instance.SpawnUnitOnRandomTile(lifter);
                }
                else if(rand == 1)
                {
                    EnemySpawner.Instance.SpawnUnitOnRandomTile(sorter);
                }
                else
                {
                    EnemySpawner.Instance.SpawnUnitOnRandomTile(runner);
                }

                //player lives
                if (ConductorV2.instance.measureTrack % 2 == 0 && ConductorV2.instance.beatTrack == 4)
                {

                    measureCounter += 1;
                    //change to phase 1B
                    if (measureCounter == 4)
                    {
                        Debug.Log("Damage Phase");
                        currentStateIndex = PhaseIndex.A;
                        currentState = CommanderStates.damagePhase;
                        previousState = CommanderStates.phase2;
                        phaseStarted = false;
                        measureCounter = 0;
                        return;
                    }
                }
                break;
            default:
                break;
        }
    }

    private void PhaseThree()
    {
        switch (currentStateIndex)
        {
            case PhaseIndex.A:
                if (!phase3Start)
                {
                    phase3Start = true;
                    comandeerCursor.SetActive(true);
                    CursorMoveTo(new Vector3(7.5f, -0.4f, -1.5f));
                }

                if(!cursorMovementInProgress)
                {
                    EnemySpawner.Instance.ForceEnemySpawnDynamic(comandeerCursor.transform.position.z, lifter);
                    currentStateIndex = PhaseIndex.B;
                }
                    break;
            case PhaseIndex.B:
                CursorMoveTo(new Vector3(7.5f, -0.4f, 1.5f));
                break;
            case PhaseIndex.C:
                break;
            default:
                break;
        }
    }

    private void PhaseFour()
    {
        switch (currentStateIndex)
        {
            case PhaseIndex.A:
                break;
            case PhaseIndex.B:
                break;
            case PhaseIndex.C:
                break;
            default:
                break;
        }
    }

    public void CursorMovement()
    {
        if(cursorMovementInProgress && !cursorDontMove)
        {
            if(comandeerCursor.transform.position != cursorNextPos)
            {
                cursorTimer += Time.deltaTime * 1;
                comandeerCursor.transform.position = Vector3.Lerp(comandeerCursor.transform.position, cursorNextPos, cursorTimer);
            }
            else
            {
                cursorDontMove = true;
                comandeerCursor.transform.position = cursorNextPos;

                cursorNextPos = comandeerCursor.transform.position + GetDirection(comandeerCursor.transform.position, cursorFinalPos);
                cursorTimer = 0;

                if(comandeerCursor.transform.position == new Vector3(cursorFinalPos.x, comandeerCursor.transform.position.y, cursorFinalPos.z))
                {
                    cursorMovementInProgress = false;
                }
            }
        }
    }

    public void CursorMoveTo(Vector3 _finalPos)
    {
        if(!cursorMovementInProgress)
        {
            cursorFinalPos = _finalPos;
            cursorMovementInProgress = true;
            cursorNextPos = comandeerCursor.transform.position + GetDirection(comandeerCursor.transform.position, _finalPos);
        }
    }

    public override void Movement()
    {
        if(movementInProgress && !dontMove)
        {
            if (transform.position != nextPosition)
            {
                timer += Time.deltaTime * 1;
                gameObject.transform.position = Vector3.Lerp(transform.position, nextPosition, timer);
            }
            else
            {
                dontMove = true;
                gameObject.transform.position = nextPosition;
                Debug.Log(GetDirection(transform.position, finalPos));
                nextPosition = transform.position + GetDirection(transform.position, finalPos);
                timer = 0;

                //if reached final position
                if (transform.position == finalPos)
                {
                    movementInProgress = false;
                }
            }
        }
    }

    public void MoveTo(Vector3 _finalPos)
    {
        if (!movementInProgress)
        {
            Debug.Log(GetDirection(transform.position, finalPos));
            finalPos = _finalPos;
            movementInProgress = true;
            nextPosition = transform.position + GetDirection(transform.position, _finalPos);
        }
        
    }

    Vector3 GetDirection(Vector3 point_a, Vector3 point_b)
    {
        Vector3 direction = point_b - point_a;
        direction.Normalize();

        //rounds the direction to -1, 0, or 1
        Vector3 roundedDirection = new Vector3(Mathf.Round(direction.x), 0, Mathf.Round(direction.z));

        //stops unit from going diagonally
        if(roundedDirection.x != 0 && roundedDirection.z != 0)
        {
            roundedDirection.z = 0;
        }

        Debug.Log(roundedDirection);
        return roundedDirection;
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

        if (towerParent.childCount == 0)
        {
            return tMin;
        }

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

    public override void Damage(int damage)
    {
        enemyDeathSFX.Play();
        currentDamage += damage;
        //if hit to current damage threshold
        if(currentDamage >= damageThreshold)
        {
            DamageThresholdMet();
        }
    }

    public void DamageThresholdMet()
    {
        currentDamage = 0;
        movementInProgress = false;
        transform.position = originPos;
        currentStateIndex = PhaseIndex.A;
        switch (previousState)
        {
            case CommanderStates.phase1:
                currentState = CommanderStates.phase2;
                break;
            case CommanderStates.phase2:
                currentState = CommanderStates.phase3;
                break;
            case CommanderStates.phase3:
                currentState = CommanderStates.phase4;
                break;
            case CommanderStates.phase4:
                currentState = CommanderStates.winPhase;
                break;
            default:
                break;
        }
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
