using Pathfinding.Ionic.Zip;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class CursorTD : MonoBehaviour
{
    #region dont touch this
    private static CursorTD _instance;
    public static CursorTD Instance
    {
        get
        {
            if (_instance is null)
            {
                Debug.LogError("CursorTD Manager is NULL");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
        inputHandler = PlayerInputHandler.Instance;
    }
    #endregion

    // VARIABLES //
    #region Variables
    private PlayerInputHandler inputHandler;

    public bool isMoving = false;
    public Vector3 originPos, targetPos;
    public float timeToMove = 1f;

    public Vector3 desiredMovement;

    //placement menu
    [SerializeField]private bool towerSelectMenuOpened = false;
    private bool inputOnce = false;
    private bool destructMode = false;

    public Tile tile;

    [Header("Shaders Materials")]
    public Material greyscaleShader;
    
    [Header("Placement Menu")]
    public GameObject placementMenu;
    public Animator radialMenuAnimator;

    public int slotIndex = 0;
    public List<GameObject> towerSlots = new List<GameObject>();

    [Header ("Upgrade Menu")]
    public GameObject upgradeMenu;
    public bool upgradingTower = false;

    public List<GameObject> upgradeSlots = new List<GameObject>();

    [Header ("Cursor Pulse")]
    public GameObject cursorSprite;
    public Vector3 defaultSize;
    public Vector3 pulseSize;

    [Header ("Hit Judgement")]
    public GameObject beatHitResultPrefab;
    public Sprite perfectHitSprite;
    public Sprite greatHitSprite;
    public Sprite earlyHitSprite;
    public Sprite lateHitSprite;
    public Sprite missHitSprite;

    public bool pauseMovement = false;

    public bool towerSwap;

    public bool placingTower = false;

    [Header("Tutorial Objects")]
    public bool movementSequence = false;
    public bool towerPlacementMenuSequence = false;
    public bool towerPlacementMenuSequencePassed = false;
    public bool towerPlaceSequence = false ;
    public bool towerBuffSequence = false;
    public bool feverModeSequence = false;

    public int moveCounter = 0;
    public int buffCounter = 0;

    public bool beatIsHit = false;
    public bool beatHasReset = false;

    [Header("Piano resource gain")]
    public int pianoMod = 0;

    [Header("SFX")]
    public List<AudioClip> towerMenuSounds = new List<AudioClip>();
    /*
    public AudioClip upInvalidSfx;
    public AudioClip rightInvalidSfx;
    public AudioClip downInvalidSfx;
    public AudioClip leftInvalidSfx;
    */

    public AudioSource hitSoundSource;
    public List<AudioClip> hitSounds = new List<AudioClip>();

    [Header("PFX")]
    //[SerializeField] private List<ParticleSystem>() particleEffects = new List<ParticleSystem>();
    // private ParticleSystem particleInstance;
    [SerializeField] private ParticleSystem buffGreatPfx;
    private ParticleSystem buffGreatPfxInstance;
    [SerializeField] private ParticleSystem buffPerfectPfx;
    private ParticleSystem buffPerfectPfxInstance;
    [SerializeField] private ParticleSystem pianoResourceGenParticles;
    private ParticleSystem pianoResourceGenParticlesInstance;
    [SerializeField] private ParticleSystem cursorResourceGenParticles;
    private ParticleSystem cursorResourceGenParticlesInstance;

    [Header("Input Detection")]
    public float beatTimeAtInput = 0.0f; // beat progress at time of player input
    public float timeAtInput = 0.0f; // song progress at time of player input
    #endregion

    void Start()
    {
        radialMenuAnimator = placementMenu.GetComponent<Animator>();

        //beatHitResultSpriteRenderer = beatHitResultPrefab.GetComponent<SpriteRenderer>(); //get reference to the hit judgement sprite renderer
    }

    #region Update
    // Update is called once per frame
    void Update()
    {
        if (pauseMovement || ConductorV2.instance.countingIn) return;

        cursorSprite.transform.localScale = Vector3.Lerp(cursorSprite.transform.localScale, defaultSize, Time.deltaTime * 5); //return cursor sprite to origin size

        if (GameManager.Instance.winState) return;

        // TOWER MENUING FEEDBACK //
        if (towerSelectMenuOpened)
        {
            UpdateGreyscaleShader();
        }

        /*
        // TUTORIAL //
        // Make sure index is set to whichever text says "Press ARROW KEYS to place a tower"
        if (GameManager.Instance.tutorialRunning && TutorialManager.Instance.index == 6 && !towerSelectMenuOpened)
        {
            towerPlacementMenuSequence = true;
            TutorialManager.Instance.LoadPrevTutorialDialogue();
        }

        // Make sure index is set to whichever text says "Move onto a tower"
        if (GameManager.Instance.tutorialRunning && TutorialManager.Instance.index == 10 && tile.placedTower != null)
        {
            TutorialManager.Instance.LoadNextTutorialDialogue();
        }

        // Make sure index is set to whichever text says "Press Z, X, C, or V when the ring touches the center circle"
        if (GameManager.Instance.tutorialRunning && TutorialManager.Instance.index == 11 && tile.placedTower == null)
        {
            TutorialManager.Instance.LoadPrevTutorialDialogue();
        }  
        */
    }

    private void FixedUpdate()
    {
        if(ConductorV2.instance.beatDuration >= 0.500f && !beatHasReset)
        {
            beatIsHit = false;
            beatHasReset = true;
        }
        else if (ConductorV2.instance.beatDuration < 0.500f && beatHasReset)
        {
            beatHasReset = false;
        }
    }
    #endregion

    public void HandleFeverModeInput()
    {
        if (GameManager.Instance.tutorialRunning && !feverModeSequence)
            return;
        

        FeverSystem.Instance.ActivateFeverMode();
        
    }

    #region Cursor
    public void InitializeCursor()
    {
        isMoving = false;
        gameObject.transform.position = new Vector3(0.5f, 0.1f, -0.5f);

        slotIndex = 0;

        foreach (GameObject towerSlot in towerSlots)
        {
            towerSlot.GetComponent<TowerButton>().tower = GameManager.Instance.towers[slotIndex].tower;
            towerSlot.GetComponent<TowerButton>().icon.sprite = GameManager.Instance.towers[slotIndex].tower.GetComponent<Tower>().towerInfo.towerImage;

            slotIndex += 1;
        }

        /*
        towerSlotW.GetComponent<TowerButton>().tower = GameManager.Instance.towers[0].tower;
        towerSlotW.GetComponent<TowerButton>().icon.sprite = GameManager.Instance.towers[0].tower.GetComponent<Tower>().towerInfo.towerImage;

        towerSlotD.GetComponent<TowerButton>().tower = GameManager.Instance.towers[1].tower;
        towerSlotD.GetComponent<TowerButton>().icon.sprite = GameManager.Instance.towers[1].tower.GetComponent<Tower>().towerInfo.towerImage;
        
        towerSlotS.GetComponent<TowerButton>().tower = GameManager.Instance.towers[2].tower;
        towerSlotS.GetComponent<TowerButton>().icon.sprite = GameManager.Instance.towers[2].tower.GetComponent<Tower>().towerInfo.towerImage;
        
        towerSlotA.GetComponent<TowerButton>().tower = GameManager.Instance.towers[3].tower;
        towerSlotA.GetComponent<TowerButton>().icon.sprite = GameManager.Instance.towers[3].tower.GetComponent<Tower>().towerInfo.towerImage;
        */
        
        pauseMovement = false;
        towerSwap = false;

        placementMenu.SetActive(false);
    }

    public void MoveCursor(Vector2 direction)
    {
        beatTimeAtInput = ConductorV2.instance.beatDuration;
        
        if (GameManager.Instance.winState || GameManager.Instance.failState) return;

        if (beatIsHit)
        {
            SpawnBeatHitResult(_BeatResult.miss);
            ComboManager.Instance.ResetCombo();
            return;
        }

        if (isMoving) return;

        //MOVEMENT CALCULATION
        //get the angle
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        //round the angle to 90 steps
        angle = Mathf.Round(angle / 90.0f) * 90.0f;

        //cos/sin give us x/y values 
        float horizontalOut = Mathf.Round(Mathf.Cos(angle * Mathf.Deg2Rad));
        float verticalOut = Mathf.Round(Mathf.Sin(angle * Mathf.Deg2Rad));

        direction = new Vector2(horizontalOut, verticalOut);

        direction.Normalize();

        desiredMovement = direction;

        //TOWER MENUING
        if(towerSelectMenuOpened && upgradingTower) //upgrade tower
        {
            UpgradeTower(desiredMovement); 
            return;   
        }
        else if(towerSelectMenuOpened) //place tower
        {
            HighlightPlacementSlot(desiredMovement);
            return;
        }

        //JUDGEMENT BASED RESOURCE GAIN
        switch (CheckOnBeat(beatTimeAtInput))
        {
            case _BeatResult.miss:
                SpawnBeatHitResult(_BeatResult.miss);
                ComboManager.Instance.ResetCombo();
                break;
            case _BeatResult.late:
                SpawnBeatHitResult(_BeatResult.late);
                break;
            case _BeatResult.early:
                SpawnBeatHitResult(_BeatResult.early);
                break;
            case _BeatResult.great:
                SpawnBeatHitResult(_BeatResult.great);
                ComboManager.Instance.IncreaseCombo();
                //ComboManager.Instance.IncreaseScore();
                CombatManager.Instance.resourceNum += 1;
                SpawnParticles(cursorResourceGenParticles, cursorResourceGenParticlesInstance);
                break;
            case _BeatResult.perfect:
                SpawnBeatHitResult(_BeatResult.perfect);
                ComboManager.Instance.IncreaseCombo();
                //ComboManager.Instance.IncreaseScore();
                CombatManager.Instance.resourceNum += 3;
                SpawnParticles(cursorResourceGenParticles, cursorResourceGenParticlesInstance);
                break;
            default:
                break;
        }

        //MOVEMENT
        Move(desiredMovement);
    }

    public void Move(Vector2 direction)
    {
        if (desiredMovement == Vector3.zero || towerSelectMenuOpened || isMoving || GameManager.Instance.winState || GameManager.Instance.failState) 
            return;
        

        StartCoroutine(MovePlayer(direction));
    }

    private IEnumerator MovePlayer(Vector3 direction)
    {
        isMoving = true;

        float elapsedTime = 0;

        originPos = transform.position;

        targetPos = originPos + new Vector3(direction.x, 0, direction.y);


        //bounding box function
        if ((targetPos.x <= -6.5f || targetPos.x >= 10.5f) || (targetPos.z <= -4.5f || targetPos.z >= 2.5f))
        {
            isMoving = false;
            desiredMovement = Vector3.zero;
            yield break;
        }

        while (elapsedTime < timeToMove)
        {
            transform.position = Vector3.Lerp(originPos, targetPos, elapsedTime / timeToMove);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;

        //move onto piano tile
        if (tile != null && tile.placedTower != null && tile.placedTower.GetComponent<Tower>().towerInfo.isResourceTower)
        {
            CheckPianoResult(tile.placedTower.GetComponent<Tower>());
        }

        isMoving = false;
        desiredMovement = Vector3.zero;
        tile = null;

        // Make sure index is set to whichever text says "Aim for Great or Perfect"
        if (GameManager.Instance.tutorialRunning && movementSequence && TutorialManager.Instance.index == 3)
        {
            moveCounter += 1;
            if (moveCounter == 4)
            {
                movementSequence = false;
                moveCounter = 0;

                TutorialManager.Instance.LoadNextTutorialDialogue();

                CombatManager.Instance.resources.SetActive(true);
                CombatManager.Instance.resourceNum = 0;
            }
        }

        yield return null;
    }

    //check which tile cursor is on
    private void OnTriggerStay(Collider collision)
    {
        if(collision.gameObject.CompareTag("StageTile"))
        {
            if ( tile != null && tile != collision.gameObject.GetComponent<Tile>())
            {
                if(tile.placedTower != null)
                {
                    tile.placedTower.GetComponent<Tower>().towerHover = false;

                }

            }
            else if(tile != null && tile ==  collision.gameObject.GetComponent<Tile>())
            {
                if (tile.placedTower != null)
                {
                    tile.placedTower.GetComponent<Tower>().towerHover = true;

                }
            }
            tile = collision.gameObject.GetComponent<Tile>();
        }
    }

    //NOTE: I will store all the on beat tutorial stuff here
    public void Pulse()
    {
        //Debug.Log("pulse");
        cursorSprite.transform.localScale = pulseSize;
    }
    #endregion

    #region Tower buffing
    public void BuffTrigger()
    {
        timeAtInput = ConductorV2.instance.songPosition;
        
        if (towerSelectMenuOpened) return;

        TowerEmpowerment(BuffType.Normal);
    }

    public void TowerEmpowerment(BuffType buff)
    {
        //BUFFING & COMBO MANAGEMENT
        if(tile.placedTower != null) // if(tile.placedTower != null && !beatIsHit) if tile is not empty and beat is not hit already
        {
            switch (CheckOnInput(timeAtInput, tile.placedTower.GetComponent<Tower>().inputTargetTime)) //switch (CheckOnBeat(beatTimeAtInput))
            {
                case _BeatResult.miss:
                    //FEEDBACK
                    hitSoundSource.clip = hitSounds[0];
                    hitSoundSource.Play();
                    SpawnBeatHitResult(_BeatResult.miss);
                    
                    //COMBO
                    ComboManager.Instance.ResetCombo();

                    RegisterIndicatorHit(4);
                    break;
                case _BeatResult.late:
                    //FEEDBACK
                    hitSoundSource.clip = hitSounds[0];
                    hitSoundSource.Play();
                    SpawnBeatHitResult(_BeatResult.late);

                    RegisterIndicatorHit(3);
                    break;
                case _BeatResult.early:
                    //FEEDBACK
                    hitSoundSource.clip = hitSounds[0];
                    hitSoundSource.Play();
                    SpawnBeatHitResult(_BeatResult.early);

                    RegisterIndicatorHit(2);
                    break;
                case _BeatResult.great:
                    //FEEDBACK
                    hitSoundSource.clip = hitSounds[1];
                    hitSoundSource.Play();
                    SpawnBeatHitResult(_BeatResult.great);
                    //buffGreatPfxInstance = Instantiate(buffGreatPfx, tile.placedTower.transform.position, Quaternion.identity); // buff pfx

                    //COMBO & SCORE
                    ComboManager.Instance.IncreaseCombo();
                    //ComboManager.Instance.IncreaseScore();

                    //BUFFING
                    tile.placedTower.GetComponent<Tower>().ActivateBuff(buff); // activate buff

                    RegisterIndicatorHit(1);
                    break;
                case _BeatResult.perfect:      
                    //FEEDBACK
                    hitSoundSource.clip = hitSounds[2];
                    hitSoundSource.Play();
                    SpawnBeatHitResult(_BeatResult.perfect);
                    //buffPerfectPfxInstance = Instantiate(buffPerfectPfx, tile.placedTower.transform.position, Quaternion.identity); // buff pfx

                    //COMBO & SCORE
                    ComboManager.Instance.IncreaseCombo();
                    //ComboManager.Instance.IncreaseScore();

                    //BUFFING
                    tile.placedTower.GetComponent<Tower>().ActivateBuff(buff); // activate buff

                    RegisterIndicatorHit(0);
                    break;
                case _BeatResult.nohit:
                    break;
                default:
                    break;
            }
        }
        else
        {
            return;
        }
    }

    public void RegisterIndicatorHit(int index)
    {
        tile.placedTower.GetComponent<Tower>().indicators[tile.placedTower.GetComponent<Tower>().inputIndex].GetComponent<InputIndicator>().isHit = true;
        tile.placedTower.GetComponent<Tower>().indicators[tile.placedTower.GetComponent<Tower>().inputIndex].GetComponent<InputIndicator>().currentColor =  tile.placedTower.GetComponent<Tower>().indicators[tile.placedTower.GetComponent<Tower>().inputIndex].GetComponent<InputIndicator>().hitColors[index];
        tile.placedTower.GetComponent<Tower>().UpdateInputIndex();
    }

    //To-Do: Update attack & input indexes based on timing of new input list
    public void UpgradeTower(Vector2 direction)
    {
        if (!towerSelectMenuOpened || placingTower || !upgradingTower) return;

        placingTower = true;
        Tower hoveredTower = tile.placedTower.GetComponent<Tower>();

        if (direction == Vector2.left)//upgrade 1 
        {
            //checks if over tower, if sufficent resources, if upgrade hasnt already been purchased
            if (towerSelectMenuOpened && hoveredTower != null && 
                CombatManager.Instance.resourceNum >= hoveredTower.towerInfo.upgradeCost1 &&
                !hoveredTower.upgradePurchased && !hoveredTower.towerInfo.isUpgradeOneLocked)
            {
                hoveredTower.upgradePurchased = true;
                hoveredTower.upgradeIndex = 1; //hoveredTower.upgradeOneActive = true;
                hoveredTower.ResetIndicators(); //update indicator pattern
                //hoveredTower.UpdateCycleIndices();

                //play upgrade sound
                AudioManager.instance.PlaySound(hoveredTower.towerUpgradeSfx, this.gameObject.transform, 1.0f);

                //tile.placedTower.GetComponent<Tower>().nextProjectile = tile.placedTower.GetComponent<Tower>().upgradeProjectile01;
                CombatManager.Instance.resourceNum -= hoveredTower.towerInfo.upgradeCost1;

                ClosePlacementMenu();

                return;
            }

            PlacementFeedback(towerMenuSounds[0], "Upgrade Slot 01");

        }
        else if (direction == Vector2.up)//upgrade 2
        {
            //checks if over tower, if sufficent resources, if upgrade hasnt already been purchased
            if (towerSelectMenuOpened && hoveredTower != null && 
                CombatManager.Instance.resourceNum >= hoveredTower.towerInfo.upgradeCost2 &&
                !hoveredTower.upgradePurchased && !hoveredTower.towerInfo.isUpgradeTwoLocked)
            {
                hoveredTower.upgradePurchased = true;
                hoveredTower.upgradeIndex = 2; //hoveredTower.upgradeTwoActive = true;
                hoveredTower.ResetIndicators(); //update indicator pattern
                //hoveredTower.UpdateCycleIndices();
                
                //play upgrade sound
                AudioManager.instance.PlaySound(hoveredTower.towerUpgradeSfx, this.gameObject.transform, 1.0f);

                CombatManager.Instance.resourceNum -= hoveredTower.towerInfo.upgradeCost2;
                
                ClosePlacementMenu();
                
                return;
            }

            PlacementFeedback(towerMenuSounds[1], "Upgrade Slot 04");

        }
        else if (direction == Vector2.right)//upgrade 3 
        {
            //checks if over tower, if sufficent resources, if upgrade hasnt already been purchased
            if (towerSelectMenuOpened && hoveredTower != null && 
                CombatManager.Instance.resourceNum >= hoveredTower.towerInfo.upgradeCost3 &&
                !hoveredTower.upgradePurchased && !hoveredTower.towerInfo.isUpgradeOneLocked)
            {
                hoveredTower.upgradePurchased = true;
                hoveredTower.upgradeIndex = 3; //hoveredTower.upgradeThreeActive = true;
                hoveredTower.ResetIndicators(); //update indicator pattern
                //hoveredTower.UpdateCycleIndices();
                
                //play upgrade sound
                AudioManager.instance.PlaySound(hoveredTower.towerUpgradeSfx, this.gameObject.transform, 1.0f);

                CombatManager.Instance.resourceNum -= hoveredTower.towerInfo.upgradeCost3;

                ClosePlacementMenu();
                
                return;
            }

            PlacementFeedback(towerMenuSounds[2], "Upgrade Slot 03");
        }
    }
    #endregion
    
    #region Tower placement
    //SUGGESTION: Maybe we could give players the ability to swap even without the menu open and we could show the highlighted towers in the loadout UI
    public void SwapTowers()
    {
        towerSwap = !towerSwap;
        if (towerSwap)
        {
            slotIndex = 0 + towerSlots.Count;

            foreach (GameObject towerSlot in towerSlots)
            {
                towerSlot.GetComponent<TowerButton>().tower = GameManager.Instance.towers[slotIndex].tower;
                towerSlot.GetComponent<TowerButton>().icon.sprite = GameManager.Instance.towers[slotIndex].tower.GetComponent<Tower>().towerInfo.towerImage;

                slotIndex += 1;
            }
        }
        else
        {
            slotIndex = 0;

            foreach (GameObject towerSlot in towerSlots)
            {
                towerSlot.GetComponent<TowerButton>().tower = GameManager.Instance.towers[slotIndex].tower;
                towerSlot.GetComponent<TowerButton>().icon.sprite = GameManager.Instance.towers[slotIndex].tower.GetComponent<Tower>().towerInfo.towerImage;

                slotIndex += 1;
            }
        }

        //TowerManager.Instance.SwapTowers();
    }
    
    public void TryToPlaceTower(GameObject tower, AudioClip feedbackAudio, string feedbackVisual, int towerNum)
    {
        beatTimeAtInput = ConductorV2.instance.beatDuration;
        
        //checks if resource is available and if the tower is on cooldown
        if(CombatManager.Instance.resourceNum >= tower.GetComponent<Tower>().towerInfo.resourceCost 
            && !TowerManager.Instance.CheckIfOnCoolDown(towerNum) &&
            tile != null && tile.placedTower == null && !tile.cantPlaceTower) 
        {
            //if tower limit is enabled and tower limit is reached use this if statement to stop tower placement
            if (!GameManager.Instance.tutorialRunning && CombatManager.Instance.currentEncounter.enableTowerLimit && TowerManager.Instance.CheckIfTowerAtLimit(towerNum))
            {
                Debug.Log($"{tower.name} at limit cannot be placed. Please try another tower!");
                return;
            }

            TowerManager.Instance.SetTower(tower, new Vector3(transform.position.x, 0.5f, transform.position.z), tile, towerNum, CheckOnBeat(timeAtInput), false);
            CombatManager.Instance.resourceNum -= tower.GetComponent<Tower>().towerInfo.resourceCost;

            SpawnBeatHitResult(CheckOnBeat(beatTimeAtInput));
            TogglePlacementMenu();
            placingTower = false;
            return;
        }
        else //if tower can't be placed
        {
            PlacementFeedback(feedbackAudio, feedbackVisual);
            return;
        }    
    }

    public void TogglePlacementMenu()
    {
        if (destructMode || GameManager.Instance.winState || GameManager.Instance.failState || ConductorV2.instance.countingIn) return;

        towerSelectMenuOpened = true;

        if (tile != null && tile.placedTower != null)//tower on tile
        {
            upgradeSlots[0].GetComponent<TowerButton>().icon.sprite = tile.placedTower.GetComponent<Tower>().towerInfo.upgrade1;
            upgradeSlots[1].GetComponent<TowerButton>().icon.sprite = tile.placedTower.GetComponent<Tower>().towerInfo.upgrade2;
            upgradeSlots[2].GetComponent<TowerButton>().icon.sprite = tile.placedTower.GetComponent<Tower>().towerInfo.upgrade3;

            upgradingTower = true;
            upgradeMenu.SetActive(towerSelectMenuOpened);
            return;
        }
        else if(tile != null)//empty tile
        {
            placementMenu.SetActive(towerSelectMenuOpened);

            if (GameManager.Instance.tutorialRunning && towerPlacementMenuSequence && towerSelectMenuOpened)
            {
                towerPlacementMenuSequencePassed = true;
                TutorialManager.Instance.LoadNextTutorialDialogue();
                towerPlacementMenuSequence = false;
                towerPlaceSequence = true;
                Debug.Log("Do we reach this?");
            }
        }

    }

    public void ClosePlacementMenu()
    {
        upgradingTower = false;
        placingTower = false;
        towerSelectMenuOpened = false;
        placementMenu.SetActive(towerSelectMenuOpened);
        upgradeMenu.SetActive(towerSelectMenuOpened);
    }

    public void HighlightPlacementSlot(Vector2 direction)
    {
        if (!towerSelectMenuOpened || placingTower) return;

        placingTower = true;

        if(!towerSwap)
        {
            if (direction == Vector2.left) //LEFT
            {
                slotIndex = 0;
                CheckIfCanPlace(slotIndex, towerMenuSounds[3], "Check Slot Left", slotIndex);
            }
            else if (direction == Vector2.up) //UP
            {
                slotIndex = 1;
                CheckIfCanPlace(slotIndex, towerMenuSounds[0], "Check Slot Up", slotIndex);
            }
            else if (direction == Vector2.right) //RIGHT
            {
                slotIndex = 2;
                CheckIfCanPlace(slotIndex, towerMenuSounds[1], "Check Slot Right", slotIndex);
            }
            else if (direction == Vector2.down) //DOWN
            {
                slotIndex = 3;
                CheckIfCanPlace(slotIndex, towerMenuSounds[2], "Check Slot Down", slotIndex);
            }
        }
        else
        {
            if (direction == Vector2.left) //LEFT
            {
                slotIndex = 0;
                CheckIfCanPlace(slotIndex, towerMenuSounds[3], "Check Slot Left", (slotIndex + towerSlots.Count));
            }
            else if (direction == Vector2.up) //UP
            {
                slotIndex = 1;
                CheckIfCanPlace(slotIndex, towerMenuSounds[0], "Check Slot Up", (slotIndex + towerSlots.Count));
            }
            else if (direction == Vector2.right) //RIGHT
            {
                slotIndex = 2;
                CheckIfCanPlace(slotIndex, towerMenuSounds[1], "Check Slot Right", (slotIndex + towerSlots.Count));
            }
            else if (direction == Vector2.down) //DOWN
            {
                slotIndex = 1;
                CheckIfCanPlace(slotIndex, towerMenuSounds[2], "Check Slot Down", (slotIndex + towerSlots.Count));
            }
        }
    }

    private void CheckIfCanPlace(int index, AudioClip soundEffect, string animationTrigger, int towerNum)
    {
        if (towerSelectMenuOpened && tile.placedTower == null && !tile.cantPlaceTower)
        {
            TryToPlaceTower(towerSlots[index].GetComponent<TowerButton>().tower, soundEffect, animationTrigger, towerNum);
            return;
        }
    }

    private void PlacementFeedback(AudioClip feedbackSound, string feedbackAnimation)
    {
        placingTower = false;
        //Debug.Log("try to place tower");

        radialMenuAnimator.SetTrigger(feedbackAnimation); //play the sound & animation on the corresponding tower slot when the tower cannot be placed
        AudioManager.instance.PlaySound(feedbackSound, this.gameObject.transform, 1.0f); //play feedback sound
    }
    #endregion

    #region Tower removal
    public void DestroyMode()
    {
        //
        if(!towerSelectMenuOpened)
        {
            destructMode = !destructMode;
            if (destructMode)
            {
                cursorSprite.GetComponent<SpriteRenderer>().color = Color.red;
                if (tile != null && tile.placedTower != null && inputHandler.DestructTrigger)
                {
                    tile.placedTower.GetComponent<Tower>().RemoveTower();


                }
            }
            else
            {
                cursorSprite.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
    }

    public void DestroyTower()
    {
        if (destructMode)
        {
            if (tile != null && tile.placedTower != null)
            {
                tile.placedTower.GetComponent<Tower>().RemoveTower();
            }
        }
    }
    #endregion

    #region Hit judgement
    public void SpawnBeatHitResult(_BeatResult result)
    {
        if (GameManager.Instance.winState || GameManager.Instance.failState || GameManager.Instance.isGamePaused || beatIsHit) return;
        
        beatIsHit = true;

        GameObject beatResult = Instantiate(beatHitResultPrefab, new Vector3(transform.position.x, transform.position.y + 0.6f, transform.position.z), Quaternion.identity);
        SpriteRenderer beatJudgementSpriteRenderer = beatResult.GetComponent<SpriteRenderer>();

        switch (result)
        {
            case _BeatResult.late:               
                beatJudgementSpriteRenderer.sprite = lateHitSprite; // Show LATE sprite
                break;
            case _BeatResult.miss:               
                beatJudgementSpriteRenderer.sprite = missHitSprite; // Show MISS sprite
                break;
            case _BeatResult.early:                
                beatJudgementSpriteRenderer.sprite = earlyHitSprite; // Show EARLY sprite
                break;
            case _BeatResult.great:               
                beatJudgementSpriteRenderer.sprite = greatHitSprite; // Show GREAT sprite
                break;
            case _BeatResult.perfect:
                beatJudgementSpriteRenderer.sprite = perfectHitSprite; // Show PERFECT sprite
                break;
            default:
                beatJudgementSpriteRenderer.sprite = missHitSprite; //consider disabling the sprite here instead
                break;
        }
    }

    public _BeatResult CheckOnBeat(float inputTime)
    {
        if ((ConductorV2.instance.beatDuration >= ConductorV2.instance.perfectBeatThreshold) || ConductorV2.instance.beatDuration < ConductorV2.instance.lateGreatBeatThreshold) {
            return _BeatResult.perfect; 
        }
        else if (ConductorV2.instance.beatDuration >= ConductorV2.instance.earlyGreatBeatThreshold) {
            return _BeatResult.great;
        }
        else if (ConductorV2.instance.beatDuration >= ConductorV2.instance.earlyBeatThreshold) {
            return _BeatResult.early;
        }
        else if (ConductorV2.instance.beatDuration >= ConductorV2.instance.missBeatThreshold) {
            return _BeatResult.miss;
        }
        else if (ConductorV2.instance.beatDuration >= ConductorV2.instance.lateBeatThreshold) {
            return _BeatResult.late;
        }
        else if (ConductorV2.instance.beatDuration >= ConductorV2.instance.lateGreatBeatThreshold) {
            return _BeatResult.great;
        }
        else {
            return _BeatResult.nohit;
        }
    }

    // TO DO: Double check threshold calculations to ensure they're exact on both sides of the input timing
    public _BeatResult CheckOnInput(float inputTime, float inputTargetTime)
    {
        /*
        if (inputTime >= inputTargetTime + ConductorV2.instance.missBeatThreshold) //miss+ (>= .375)
        {
            return _BeatResult.miss;
        }
        */
        if (inputTime > inputTargetTime + ConductorV2.instance.maxBeatThreshold || inputTime < inputTargetTime - ConductorV2.instance.maxBeatThreshold)
        {
            return _BeatResult.nohit;
        }
        else if (inputTime >= inputTargetTime + ConductorV2.instance.lateBeatThreshold) //late (>= .250)
        {
            return _BeatResult.late;
        }
        else if (inputTime >= inputTargetTime + ConductorV2.instance.lateGreatBeatThreshold) //great+ (>= .125)
        {
            return _BeatResult.great;
        }
        else if (inputTime > inputTargetTime - (1 - ConductorV2.instance.perfectBeatThreshold)) //perfect (> .125)
        {
            return _BeatResult.perfect; 
        }
        else if (inputTime > inputTargetTime - (1 - ConductorV2.instance.earlyGreatBeatThreshold)) //great- (> .250)
        {
            return _BeatResult.great;
        }
        else if (inputTime > inputTargetTime - (1 - ConductorV2.instance.earlyBeatThreshold)) //early (> .375)
        {
            return _BeatResult.early;
        }
        else if (inputTime <= inputTargetTime - (1 - ConductorV2.instance.earlyBeatThreshold)) //miss- (<= .375)
        {
            return _BeatResult.miss;
        }
        else
        {
            return _BeatResult.nohit;
        }
    }

    public void CheckPianoResult(Tower tower)
    {
        switch (CheckOnBeat(beatTimeAtInput))
        {
            case _BeatResult.great:
                //FEEDBACK
                SpawnParticles(pianoResourceGenParticles, pianoResourceGenParticlesInstance); // resource gen pfx
                //RESOURCES
                CombatManager.Instance.resourceNum += tower.towerInfo.resourceGain; // increase resources  
                break;
            case _BeatResult.perfect:
                //FEEDBACK
                SpawnParticles(pianoResourceGenParticles, pianoResourceGenParticlesInstance); // resource gen pfx
                //RESOURCES
                CombatManager.Instance.resourceNum += tower.towerInfo.resourceGain; // increase resources
                break;
            default:
                break;
        }
    }
    #endregion

    #region Feedback
    private void SpawnParticles(ParticleSystem particlesSource, ParticleSystem particlesInstance)
    {
        particlesInstance = Instantiate(particlesSource, transform.position, Quaternion.identity);
    }

    void UpdateGreyscaleShader() 
    { 
        // Check if guitar tower is on cooldown or player does not have enough resources to purchase them. If so, apply greyscale shader material. Otherwise, remove.
        if (upgradingTower)
        {
            if (CombatManager.Instance.resourceNum < tile.placedTower.GetComponent<Tower>().towerInfo.upgradeCost1 || tile.placedTower.GetComponent<Tower>().upgradeIndex == 1) //upgradeOneActive
            {
                upgradeSlots[0].GetComponent<Image>().material = greyscaleShader;
            }
            else
            {
                upgradeSlots[0].GetComponent<Image>().material = null;
            }
            
            // Check if drum tower is on cooldown or player does not have enough resources to purchase them. If so, apply greyscale shader material. Otherwise, remove.
            if (CombatManager.Instance.resourceNum < tile.placedTower.GetComponent<Tower>().towerInfo.upgradeCost2 || tile.placedTower.GetComponent<Tower>().upgradeIndex == 2) //upgradeTwoActive
            {
                upgradeSlots[1].GetComponent<Image>().material = greyscaleShader;
            }
            else
            {
                upgradeSlots[1].GetComponent<Image>().material = null;
            }

            // Check if bass tower is on cooldown or player does not have enough resources to purchase them. If so, apply greyscale shader material. Otherwise, remove.
            if (CombatManager.Instance.resourceNum < tile.placedTower.GetComponent<Tower>().towerInfo.upgradeCost3 || tile.placedTower.GetComponent<Tower>().upgradeIndex == 3) //upgradeThreeActive
            {
                upgradeSlots[2].GetComponent<Image>().material = greyscaleShader;
            }
            else
            {
                upgradeSlots[2].GetComponent<Image>().material = null;
            }
        }
        else
        {
            slotIndex = 0;

            foreach (GameObject towerSlot in towerSlots)
            {
                if (GameManager.Instance.towers[slotIndex].towerCooldownInfo.towerCooldown || CombatManager.Instance.resourceNum < GameManager.Instance.towers[slotIndex].tower.GetComponent<Tower>().towerInfo.resourceCost) 
                {
                    towerSlot.GetComponent<Image>().material = greyscaleShader;
                }
                else
                {
                    towerSlot.GetComponent<Image>().material = null;
                }
                
                slotIndex += 1;
            }
        }    
    }
    #endregion
}
