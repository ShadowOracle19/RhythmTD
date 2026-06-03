
using Pathfinding.Ionic.Zip;
using System.Collections;
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

    public GameObject towerSlotW;
    public GameObject towerSlotA;
    public GameObject towerSlotS;
    public GameObject towerSlotD;

    [Header ("Upgrade Menu")]
    public GameObject upgradeMenu;
    public bool upgradingTower = false;

    public GameObject upgradeSlotW;
    public GameObject upgradeSlotA;
    public GameObject upgradeSlotS;
    public GameObject upgradeSlotD;

    public GameObject cursorSprite;
    public Vector3 defaultSize;
    public Vector3 pulseSize;

    public GameObject beatHitResultPrefab;
    //public SpriteRenderer beatHitResultSpriteRenderer;
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

    [Header("Resource Bar")]
    public Slider tower1Slider;
    public Image tower1ResourceSprite;

    public Slider tower2Slider;
    public Image tower2ResourceSprite;

    public Slider tower3Slider;
    public Image tower3ResourceSprite;

    public Slider tower4Slider;
    public Image tower4ResourceSprite;

    [Header("Piano resource gain")]
    public int pianoMod = 0;

    [Header("SFX")]
    public AudioClip upInvalidSfx;
    public AudioClip rightInvalidSfx;
    public AudioClip downInvalidSfx;
    public AudioClip leftInvalidSfx;
    public AudioClip hitSfx;

    [Header("PFX")]
    [SerializeField] private ParticleSystem buffGreatPfx;
    private ParticleSystem buffGreatPfxInstance;

    [SerializeField] private ParticleSystem buffPerfectPfx;
    private ParticleSystem buffPerfectPfxInstance;

    [SerializeField] private ParticleSystem pianoResourceGenParticles;
    private ParticleSystem pianoResourceGenParticlesInstance;

    [SerializeField] private ParticleSystem cursorResourceGenParticles;
    private ParticleSystem cursorResourceGenParticlesInstance;



    void Start()
    {
        radialMenuAnimator = placementMenu.GetComponent<Animator>();

        //beatHitResultSpriteRenderer = beatHitResultPrefab.GetComponent<SpriteRenderer>(); //get reference to the hit judgement sprite renderer
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseMovement || ConductorV2.instance.countingIn) return;

        //return cursor sprite to origin size
        cursorSprite.transform.localScale = Vector3.Lerp(cursorSprite.transform.localScale, defaultSize, Time.deltaTime * 5);

        if (GameManager.Instance.winState) return;

        /*
        if(tile != null && tile.placedTower != null)
        {
            tile.placedTower.GetComponent<Tower>().towerHover = true;
        }
        */

        //TOWER MENUING FEEDBACK
        if (towerSelectMenuOpened)
        {
            UpdateGreyscaleShader();
        }

        //TUTORIAL
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

    public void HandleFeverModeInput()
    {
        if (GameManager.Instance.tutorialRunning && !feverModeSequence)
            return;
        

        FeverSystem.Instance.ActivateFeverMode();
        
    }

    public void SwapTowers()
    {
        towerSwap = !towerSwap;
        if (towerSwap)
        {
            towerSlotW.GetComponent<TowerButton>().tower = GameManager.Instance.towers[4].tower;
            towerSlotW.GetComponent<TowerButton>().icon.sprite = GameManager.Instance.towers[4].tower.GetComponent<Tower>().towerInfo.towerImage;

            towerSlotD.GetComponent<TowerButton>().tower = GameManager.Instance.towers[5].tower;
            towerSlotD.GetComponent<TowerButton>().icon.sprite = GameManager.Instance.towers[5].tower.GetComponent<Tower>().towerInfo.towerImage;


            towerSlotS.GetComponent<TowerButton>().tower = GameManager.Instance.towers[6].tower;
            towerSlotS.GetComponent<TowerButton>().icon.sprite = GameManager.Instance.towers[6].tower.GetComponent<Tower>().towerInfo.towerImage;


            towerSlotA.GetComponent<TowerButton>().tower = GameManager.Instance.towers[7].tower;
            towerSlotA.GetComponent<TowerButton>().icon.sprite = GameManager.Instance.towers[7].tower.GetComponent<Tower>().towerInfo.towerImage;
        }
        else
        {
            towerSlotW.GetComponent<TowerButton>().tower = GameManager.Instance.towers[0].tower;
            towerSlotW.GetComponent<TowerButton>().icon.sprite = GameManager.Instance.towers[0].tower.GetComponent<Tower>().towerInfo.towerImage;

            towerSlotD.GetComponent<TowerButton>().tower = GameManager.Instance.towers[1].tower;
            towerSlotD.GetComponent<TowerButton>().icon.sprite = GameManager.Instance.towers[1].tower.GetComponent<Tower>().towerInfo.towerImage;


            towerSlotS.GetComponent<TowerButton>().tower = GameManager.Instance.towers[2].tower;
            towerSlotS.GetComponent<TowerButton>().icon.sprite = GameManager.Instance.towers[2].tower.GetComponent<Tower>().towerInfo.towerImage;


            towerSlotA.GetComponent<TowerButton>().tower = GameManager.Instance.towers[3].tower;
            towerSlotA.GetComponent<TowerButton>().icon.sprite = GameManager.Instance.towers[3].tower.GetComponent<Tower>().towerInfo.towerImage;
        }

        TowerManager.Instance.SwapTowers();
    }

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

    public void InitializeCursor()
    {
        isMoving = false;
        gameObject.transform.position = new Vector3(0.5f, 0.1f, -0.5f);

        towerSlotW.GetComponent<TowerButton>().tower = GameManager.Instance.towers[0].tower;
        towerSlotW.GetComponent<TowerButton>().icon.sprite = GameManager.Instance.towers[0].tower.GetComponent<Tower>().towerInfo.towerImage;
        TowerManager.Instance.SetResourceBarSprite(GameManager.Instance.towers[0].tower.GetComponent<Tower>(), tower1Slider, tower1ResourceSprite);


        towerSlotD.GetComponent<TowerButton>().tower = GameManager.Instance.towers[1].tower;
        towerSlotD.GetComponent<TowerButton>().icon.sprite = GameManager.Instance.towers[1].tower.GetComponent<Tower>().towerInfo.towerImage;
        TowerManager.Instance.SetResourceBarSprite(GameManager.Instance.towers[1].tower.GetComponent<Tower>(), tower2Slider, tower2ResourceSprite);


        towerSlotS.GetComponent<TowerButton>().tower = GameManager.Instance.towers[2].tower;
        towerSlotS.GetComponent<TowerButton>().icon.sprite = GameManager.Instance.towers[2].tower.GetComponent<Tower>().towerInfo.towerImage;
        TowerManager.Instance.SetResourceBarSprite(GameManager.Instance.towers[2].tower.GetComponent<Tower>(), tower3Slider, tower3ResourceSprite);


        towerSlotA.GetComponent<TowerButton>().tower = GameManager.Instance.towers[3].tower;
        towerSlotA.GetComponent<TowerButton>().icon.sprite = GameManager.Instance.towers[3].tower.GetComponent<Tower>().towerInfo.towerImage;
        TowerManager.Instance.SetResourceBarSprite(GameManager.Instance.towers[3].tower.GetComponent<Tower>(), tower4Slider, tower4ResourceSprite);

        pauseMovement = false;
        towerSwap = false;

        placementMenu.SetActive(false);
    }

    public void MoveCursor(Vector2 direction)
    {
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
        switch (CheckOnBeat())
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

    public void Buff1Trigger()
    {
        //if (towerSelectMenuOpened) return;
        //TowerEmpowerment(BuffType.Shield);
        //SpawnBeatHitResult();
    }
    public void Buff2Trigger()
    {
        //if (towerSelectMenuOpened) return;
        //TowerEmpowerment(BuffType.Multi);
        //SpawnBeatHitResult();
    }
    public void Buff3Trigger()
    {
        //if (towerSelectMenuOpened) return;
        //TowerEmpowerment(BuffType.Burn);
        //SpawnBeatHitResult();
    }

    public void Buff4Trigger()
    {
        if (towerSelectMenuOpened) return;

        TowerEmpowerment(BuffType.Normal);
        SpawnBeatHitResult(CheckOnBeat());
    }

    /*
    public void TriggerBuff()
    {
        if (towerSelectMenuOpened) return;

        TowerEmpowerment(BuffType.Normal);
    }
    */

    public void TowerEmpowerment(BuffType buff)
    {
        //FEEDBACK
        AudioManager.instance.PlaySound(hitSfx, this.gameObject.transform, 1.0f);
        
        //BUFFING & COMBO MANAGEMENT
        if(tile.placedTower != null && !beatIsHit) //if tile is not empty and beat is not hit already
        {
            switch (CheckOnBeat())
            {
                case _BeatResult.miss:
                    //FEEDBACK


                    //COMBO
                    ComboManager.Instance.ResetCombo(); // reset combo on miss
                    break;
                case _BeatResult.great:
                    //FEEDBACK
                    buffGreatPfxInstance = Instantiate(buffGreatPfx, tile.placedTower.transform.position, Quaternion.identity); // buff pfx

                    //COMBO & SCORE
                    ComboManager.Instance.IncreaseCombo(); // increase combo on great
                    //ComboManager.Instance.IncreaseScore();

                    //BUFFING
                    tile.placedTower.GetComponent<Tower>().ActivateBuff(buff); // activate buff
                    break;
                case _BeatResult.perfect:      
                    //FEEDBACK
                    buffPerfectPfxInstance = Instantiate(buffPerfectPfx, tile.placedTower.transform.position, Quaternion.identity); // buff pfx

                    //COMBO & SCORE
                    ComboManager.Instance.IncreaseCombo(); // increase combo on perfect
                    //ComboManager.Instance.IncreaseScore();

                    //BUFFING
                    tile.placedTower.GetComponent<Tower>().ActivateBuff(buff); // activate buff
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

    public void TryToPlaceTower(GameObject tower, AudioClip feedbackAudio, string feedbackVisual, int towerNum)
    {
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

            TowerManager.Instance.SetTower(tower, new Vector3(transform.position.x, 0.5f, transform.position.z), tile, towerNum, CheckOnBeat(), false);
            CombatManager.Instance.resourceNum -= tower.GetComponent<Tower>().towerInfo.resourceCost;

            SpawnBeatHitResult(CheckOnBeat());
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
            upgradeSlotW.GetComponent<TowerButton>().icon.sprite = tile.placedTower.GetComponent<Tower>().towerInfo.upgrade1;
            upgradeSlotD.GetComponent<TowerButton>().icon.sprite = tile.placedTower.GetComponent<Tower>().towerInfo.upgrade2;
            upgradeSlotS.GetComponent<TowerButton>().icon.sprite = tile.placedTower.GetComponent<Tower>().towerInfo.upgrade3;
            //upgradeSlotA.GetComponent<TowerButton>().icon.sprite = tile.placedTower.GetComponent<Tower>().towerInfo.upgrade4;

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
                //CombatManager.Instance.towerDisplay.SetActive(true);
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
            if (direction == Vector2.up) //W
            {
                if (towerSelectMenuOpened && tile.placedTower == null && !tile.cantPlaceTower)
                {
                    TryToPlaceTower(towerSlotW.GetComponent<TowerButton>().tower, upInvalidSfx, "Check Slot 01", 0);

                    return;
                }
            }
            else if (direction == Vector2.right) //D
            {
                if (towerSelectMenuOpened && tile.placedTower == null && !tile.cantPlaceTower)
                {
                    TryToPlaceTower(towerSlotD.GetComponent<TowerButton>().tower, leftInvalidSfx, "Check Slot 04", 1);

                    return;
                }
            }
            else if (direction == Vector2.down) //S
            {
                if (towerSelectMenuOpened && tile.placedTower == null && !tile.cantPlaceTower)
                {
                    TryToPlaceTower(towerSlotS.GetComponent<TowerButton>().tower, downInvalidSfx, "Check Slot 03", 2);

                    return;
                }
            }
            else if (direction == Vector2.left) //A
            {

                if (towerSelectMenuOpened && tile.placedTower == null && !tile.cantPlaceTower)
                {
                    TryToPlaceTower(towerSlotA.GetComponent<TowerButton>().tower, rightInvalidSfx, "Check Slot 02", 3);

                    return;
                }
            }
        }
        else
        {
            if (direction == Vector2.up) //W
            {
                if (towerSelectMenuOpened && tile.placedTower == null && !tile.cantPlaceTower)
                {
                    TryToPlaceTower(towerSlotW.GetComponent<TowerButton>().tower, upInvalidSfx, "Check Slot 01", 4);

                    return;
                }
            }
            else if (direction == Vector2.right) //D
            {
                if (towerSelectMenuOpened && tile.placedTower == null && !tile.cantPlaceTower)
                {
                    TryToPlaceTower(towerSlotD.GetComponent<TowerButton>().tower, rightInvalidSfx, "Check Slot 02", 5);

                    return;
                }
            }
            else if (direction == Vector2.down) //S
            {
                if (towerSelectMenuOpened && tile.placedTower == null && !tile.cantPlaceTower)
                {
                    TryToPlaceTower(towerSlotS.GetComponent<TowerButton>().tower, downInvalidSfx, "Check Slot 03", 6);

                    return;
                }
            }
            else if (direction == Vector2.left) //A
            {
                if (towerSelectMenuOpened && tile.placedTower == null && !tile.cantPlaceTower)
                {
                    TryToPlaceTower(towerSlotA.GetComponent<TowerButton>().tower, leftInvalidSfx, "Check Slot 04", 7);

                    return;
                }
            }

        }
        

    }

    public void UpgradeTower(Vector2 direction)
    {
        if (!towerSelectMenuOpened || placingTower || !upgradingTower) return;

        placingTower = true;
        Tower hoveredTower = tile.placedTower.GetComponent<Tower>();

        if (direction == Vector2.up)//upgrade 1 
        {
            //checks if over tower, if sufficent resources, if upgrade hasnt already been purchased
            if (towerSelectMenuOpened && hoveredTower != null && 
                CombatManager.Instance.resourceNum >= hoveredTower.towerInfo.upgradeCost1 &&
                !hoveredTower.upgradePurchased && !hoveredTower.towerInfo.isUpgradeOneLocked)
            {
                hoveredTower.upgradePurchased = true;
                hoveredTower.upgradeOneActive = true;

                //play upgrade sound
                AudioManager.instance.PlaySound(hoveredTower.towerUpgradeSfx, this.gameObject.transform, 1.0f);

                //tile.placedTower.GetComponent<Tower>().nextProjectile = tile.placedTower.GetComponent<Tower>().upgradeProjectile01;
                CombatManager.Instance.resourceNum -= hoveredTower.towerInfo.upgradeCost1;

                ClosePlacementMenu();

                return;
            }

            PlacementFeedback(upInvalidSfx, "Upgrade Slot 01");

        }
        else if (direction == Vector2.right)//upgrade 2
        {
            //checks if over tower, if sufficent resources, if upgrade hasnt already been purchased
            if (towerSelectMenuOpened && hoveredTower != null && 
                CombatManager.Instance.resourceNum >= hoveredTower.towerInfo.upgradeCost2 &&
                !hoveredTower.upgradePurchased && !hoveredTower.towerInfo.isUpgradeTwoLocked)
            {
                hoveredTower.upgradePurchased = true;
                hoveredTower.upgradeTwoActive = true;
                
                //play upgrade sound
                AudioManager.instance.PlaySound(hoveredTower.towerUpgradeSfx, this.gameObject.transform, 1.0f);

                CombatManager.Instance.resourceNum -= hoveredTower.towerInfo.upgradeCost2;
                
                ClosePlacementMenu();
                
                return;
            }

            PlacementFeedback(rightInvalidSfx, "Upgrade Slot 04");

        }
        //note:change this back to down if we need four upgrades in the future
        else if (direction == Vector2.left)//upgrade 3 
        {
            //checks if over tower, if sufficent resources, if upgrade hasnt already been purchased
            if (towerSelectMenuOpened && hoveredTower != null && 
                CombatManager.Instance.resourceNum >= hoveredTower.towerInfo.upgradeCost3 &&
                !hoveredTower.upgradePurchased && !hoveredTower.towerInfo.isUpgradeOneLocked)
            {
                hoveredTower.upgradePurchased = true;
                hoveredTower.upgradeThreeActive = true;
                
                //play upgrade sound
                AudioManager.instance.PlaySound(hoveredTower.towerUpgradeSfx, this.gameObject.transform, 1.0f);

                CombatManager.Instance.resourceNum -= hoveredTower.towerInfo.upgradeCost3;

                ClosePlacementMenu();
                
                return;
            }

            PlacementFeedback(downInvalidSfx, "Upgrade Slot 03");

        }
        else if (direction == Vector2.down)//upgrade 4 
        {

            ////checks if over tower, if sufficent resources, if upgrade hasnt already been purchased
            //if (towerSelectMenuOpened && tile.placedTower != null && 
            //    CombatManager.Instance.resourceNum >= tile.placedTower.GetComponent<Tower>().towerInfo.upgradeCost4 &&
            //    !tile.placedTower.GetComponent<Tower>().upgradePurchased)
            //{
            //    tile.placedTower.GetComponent<Tower>().upgradePurchased = true;
            //    tile.placedTower.GetComponent<Tower>().upgradeFourActive = true;
                
            //    //play upgrade sound
            //    AudioManager.instance.PlaySound(tile.placedTower.GetComponent<Tower>().towerUpgradeSfx, this.gameObject.transform, 1.0f);

            //    CombatManager.Instance.resourceNum -= tile.placedTower.GetComponent<Tower>().towerInfo.upgradeCost4;

            //    ClosePlacementMenu();

            //    return;
            //}

            //PlacementFeedback(leftInvalidSfx, "Upgrade Slot 02");

        }
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

    //I will store all the on beat tutorial stuff here
    public void Pulse()
    {
        //Debug.Log("pulse");
        cursorSprite.transform.localScale = pulseSize;
    }

    public void CheckPianoResult(Tower tower)
    {
        switch (CheckOnBeat())
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

    public _BeatResult CheckOnBeat()
    {
        //float songInBeats = ConductorV2.instance.songPositionInBeats;
        //float adjustedInputTime = songInBeats - GameManager.Instance.inputOffset;
        ////float timingError = Mathf.Abs(ConductorV2.instance.numberOfBeats - adjustedInputTime);
        //float timingError = Mathf.Abs(ConductorV2.instance.beatDuration - GameManager.Instance.inputOffset);
        ////Debug.Log($"Time Pressed:{ConductorV2.instance.songPositionInBeats}    Adjusted Input Time:{adjustedInputTime}   TimingError:{timingError}    beat duration: {ConductorV2.instance.beatDuration}");
        //Debug.Log($"TimingError:{timingError}    beat duration: {ConductorV2.instance.beatDuration}");

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

    /*
    public _BeatResult CheckOnInput(float inputTime, float inputTargetTime)
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
    */

    private void SpawnParticles(ParticleSystem particlesSource, ParticleSystem particlesInstance)
    {
        particlesInstance = Instantiate(particlesSource, transform.position, Quaternion.identity);
    }

    private void PlacementFeedback(AudioClip feedbackSound, string feedbackAnimation)
    {
        placingTower = false;
        Debug.Log("try to place tower");

        //play the sound & animation on the corresponding tower slot when the tower cannot be placed
        radialMenuAnimator.SetTrigger(feedbackAnimation);

        //play feedback sound
        AudioManager.instance.PlaySound(feedbackSound, this.gameObject.transform, 1.0f);
    }

    void UpdateGreyscaleShader() 
    { 
        // Check if guitar tower is on cooldown or player does not have enough resources to purchase them. If so, apply greyscale shader material. Otherwise, remove.
        if (upgradingTower)
        {
            if (CombatManager.Instance.resourceNum < tile.placedTower.GetComponent<Tower>().towerInfo.upgradeCost1 || tile.placedTower.GetComponent<Tower>().upgradeOneActive) 
            {
                upgradeSlotW.GetComponent<Image>().material = greyscaleShader;
            }
            else
            {
                upgradeSlotW.GetComponent<Image>().material = null;
            }
            
            // Check if drum tower is on cooldown or player does not have enough resources to purchase them. If so, apply greyscale shader material. Otherwise, remove.
            if (CombatManager.Instance.resourceNum < tile.placedTower.GetComponent<Tower>().towerInfo.upgradeCost2 || tile.placedTower.GetComponent<Tower>().upgradeTwoActive) 
            {
                upgradeSlotD.GetComponent<Image>().material = greyscaleShader;
            }
            else
            {
                upgradeSlotD.GetComponent<Image>().material = null;
            }

            // Check if bass tower is on cooldown or player does not have enough resources to purchase them. If so, apply greyscale shader material. Otherwise, remove.
            if (CombatManager.Instance.resourceNum < tile.placedTower.GetComponent<Tower>().towerInfo.upgradeCost3 || tile.placedTower.GetComponent<Tower>().upgradeThreeActive) 
            {
                upgradeSlotS.GetComponent<Image>().material = greyscaleShader;
            }
            else
            {
                upgradeSlotS.GetComponent<Image>().material = null;
            }

            /*
            foreach (var name in list)
            {
                if (CombatManager.Instance.resourceNum < upgradeCost || upgradeActive)
                {
                    upgradeSlot.GetComponent<Image>().material = greyscaleShader;
                }
                else
                {
                    upgradeSlot.GetComponent<Image>().material = null;
                }
            }
            */
        }
        else
        {

            if (GameManager.Instance.towers[0].towerCooldownInfo.towerCooldown || CombatManager.Instance.resourceNum < GameManager.Instance.towers[0].tower.GetComponent<Tower>().towerInfo.resourceCost) 
            {
                towerSlotW.GetComponent<Image>().material = greyscaleShader;
            }
            else
            {
                towerSlotW.GetComponent<Image>().material = null;
            }
            
            if (GameManager.Instance.towers[1].towerCooldownInfo.towerCooldown || CombatManager.Instance.resourceNum < GameManager.Instance.towers[1].tower.GetComponent<Tower>().towerInfo.resourceCost) 
            {
                towerSlotD.GetComponent<Image>().material = greyscaleShader;
            }
            else
            {
                towerSlotD.GetComponent<Image>().material = null;
            }

            if (GameManager.Instance.towers[2].towerCooldownInfo.towerCooldown || CombatManager.Instance.resourceNum < GameManager.Instance.towers[2].tower.GetComponent<Tower>().towerInfo.resourceCost) 
            {
                towerSlotS.GetComponent<Image>().material = greyscaleShader;
            }
            else
            {
                towerSlotS.GetComponent<Image>().material = null;
            }

            if (GameManager.Instance.towers[3].towerCooldownInfo.towerCooldown || CombatManager.Instance.resourceNum < GameManager.Instance.towers[3].tower.GetComponent<Tower>().towerInfo.resourceCost) 
            {
                towerSlotA.GetComponent<Image>().material = greyscaleShader;
            }
            else
            {
                towerSlotA.GetComponent<Image>().material = null;
            }
        }    
    }

}
