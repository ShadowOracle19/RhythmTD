
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
    private Vector3 originPos, targetPos;
    public float timeToMove = 1f;

    public Vector3 desiredMovement;

    //placement menu
    [SerializeField]private bool towerSelectMenuOpened = false;
    private bool inputOnce = false;
    private bool destructMode = false;

    public Tile tile;

    public GameObject placementMenu;
    public Animator radialMenuAnimator;
    public GameObject SlotW;
    public GameObject SlotA;
    public GameObject SlotS;
    public GameObject SlotD;

    public GameObject cursorSprite;
    public Vector3 pulseSize;

    public GameObject beatHitResultPrefab;

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

    // PFX
    [SerializeField] private ParticleSystem pianoResourceGenParticles;
    private ParticleSystem pianoResourceGenParticlesInstance;

    [SerializeField] private ParticleSystem cursorResourceGenParticles;
    private ParticleSystem cursorResourceGenParticlesInstance;

    // Start is called before the first frame update
    void Start()
    {
        radialMenuAnimator = placementMenu.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseMovement || ConductorV2.instance.countingIn)
            return;

        //return cursor sprite to origin size
        cursorSprite.transform.localScale = Vector3.Lerp(cursorSprite.transform.localScale, Vector3.one, Time.deltaTime * 5);
        

        if (GameManager.Instance.winState) return;

        

        if(tile != null && tile.placedTower != null)
        {
            tile.placedTower.GetComponent<Tower>().towerHover = true;
        }

        PlacementResourceBar();

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
        //placingTower = false;
        //beatIsHit = false;
    }

    public void PlacementResourceBar()
    {
        tower1Slider.value = CombatManager.Instance.resourceNum;
        tower2Slider.value = CombatManager.Instance.resourceNum;
        tower3Slider.value = CombatManager.Instance.resourceNum;
        tower4Slider.value = CombatManager.Instance.resourceNum;
    }

    public void HandleFeverModeInput()
    {
        if (GameManager.Instance.tutorialRunning && !feverModeSequence)
            return;
        

        FeverSystem.Instance.ActivateFeverMode();
        
    }

    public void SwapTowers()
    {
        //towerSwap = !towerSwap;
        //if (towerSwap)
        //{
        //    SlotW.GetComponent<TowerButton>().tower = TowerManager.Instance.towers[4];
        //    SlotW.GetComponent<TowerButton>().icon.sprite = TowerManager.Instance.towers[4].GetComponent<Tower>().towerInfo.towerImage;

        //    SlotA.GetComponent<TowerButton>().tower = TowerManager.Instance.towers[5];
        //    SlotA.GetComponent<TowerButton>().icon.sprite = TowerManager.Instance.towers[5].GetComponent<Tower>().towerInfo.towerImage;


        //    SlotS.GetComponent<TowerButton>().tower = TowerManager.Instance.towers[6];
        //    SlotS.GetComponent<TowerButton>().icon.sprite = TowerManager.Instance.towers[6].GetComponent<Tower>().towerInfo.towerImage;


        //    SlotD.GetComponent<TowerButton>().tower = TowerManager.Instance.towers[7];
        //    SlotD.GetComponent<TowerButton>().icon.sprite = TowerManager.Instance.towers[7].GetComponent<Tower>().towerInfo.towerImage;
        //}
        //else
        //{
        //    SlotW.GetComponent<TowerButton>().tower = TowerManager.Instance.towers[0];
        //    SlotW.GetComponent<TowerButton>().icon.sprite = TowerManager.Instance.towers[0].GetComponent<Tower>().towerInfo.towerImage;

        //    SlotA.GetComponent<TowerButton>().tower = TowerManager.Instance.towers[1];
        //    SlotA.GetComponent<TowerButton>().icon.sprite = TowerManager.Instance.towers[1].GetComponent<Tower>().towerInfo.towerImage;


        //    SlotS.GetComponent<TowerButton>().tower = TowerManager.Instance.towers[2];
        //    SlotS.GetComponent<TowerButton>().icon.sprite = TowerManager.Instance.towers[2].GetComponent<Tower>().towerInfo.towerImage;


        //    SlotD.GetComponent<TowerButton>().tower = TowerManager.Instance.towers[3];
        //    SlotD.GetComponent<TowerButton>().icon.sprite = TowerManager.Instance.towers[3].GetComponent<Tower>().towerInfo.towerImage;
        //}

        //TowerManager.Instance.SwapTowers();
        
        
    }

    public void DestroyMode()
    {
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
        gameObject.transform.position = new Vector3(-2.5f, -0.54f, 0);

        SlotW.GetComponent<TowerButton>().tower = TowerManager.Instance.towers[0];
        SlotW.GetComponent<TowerButton>().icon.sprite = TowerManager.Instance.towers[0].GetComponent<Tower>().towerInfo.towerImage;
        TowerManager.Instance.SetResourceBarSprite(TowerManager.Instance.towers[0].GetComponent<Tower>(), tower1Slider, tower1ResourceSprite);


        SlotA.GetComponent<TowerButton>().tower = TowerManager.Instance.towers[1];
        SlotA.GetComponent<TowerButton>().icon.sprite = TowerManager.Instance.towers[1].GetComponent<Tower>().towerInfo.towerImage;
        TowerManager.Instance.SetResourceBarSprite(TowerManager.Instance.towers[1].GetComponent<Tower>(), tower2Slider, tower2ResourceSprite);


        SlotS.GetComponent<TowerButton>().tower = TowerManager.Instance.towers[2];
        SlotS.GetComponent<TowerButton>().icon.sprite = TowerManager.Instance.towers[2].GetComponent<Tower>().towerInfo.towerImage;
        TowerManager.Instance.SetResourceBarSprite(TowerManager.Instance.towers[2].GetComponent<Tower>(), tower3Slider, tower3ResourceSprite);


        SlotD.GetComponent<TowerButton>().tower = TowerManager.Instance.towers[3];
        SlotD.GetComponent<TowerButton>().icon.sprite = TowerManager.Instance.towers[3].GetComponent<Tower>().towerInfo.towerImage;
        TowerManager.Instance.SetResourceBarSprite(TowerManager.Instance.towers[3].GetComponent<Tower>(), tower4Slider, tower4ResourceSprite);

        pauseMovement = false;
        towerSwap = false;

        placementMenu.SetActive(false);


    }

    public void MoveCursor(Vector2 direction)
    {
        if (isMoving || GameManager.Instance.winState || GameManager.Instance.loseState) return;
        
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

        SpawnBeatHitResult();

        

        if(towerSelectMenuOpened)
        {
            Debug.Log("movement");
            HighlightPlacementSlot(desiredMovement);
        }
        else
        {
            Move(desiredMovement);
        }

        
        
    }


    public void Buff1Trigger()
    {
        if (towerSelectMenuOpened) return;
        TowerEmpowerment(BuffType.Niimi);
        SpawnBeatHitResult();
    }
    public void Buff2Trigger()
    {
        if (towerSelectMenuOpened) return;
        TowerEmpowerment(BuffType.Sonu);
        SpawnBeatHitResult();
    }
    public void Buff3Trigger()
    {
        if (towerSelectMenuOpened) return;
        TowerEmpowerment(BuffType.Fayruz);
        SpawnBeatHitResult();
    }

    public void Buff4Trigger()
    {
        if (towerSelectMenuOpened) return;
        TowerEmpowerment(BuffType.Normal);
        SpawnBeatHitResult();
    }

    public void TowerEmpowerment(BuffType buff)
    {
        if(tile.placedTower != null && !beatIsHit)
        {

            switch (CheckOnBeat())
            {
                case _BeatResult.miss:
                    
                    ComboManager.Instance.ResetCombo();
                    break;
                case _BeatResult.great:
                    
                    ComboManager.Instance.IncreaseCombo();
                    tile.placedTower.GetComponent<Tower>().ActivateBuff(buff);
                    break;
                case _BeatResult.perfect:
                    
                    ComboManager.Instance.IncreaseCombo();
                    tile.placedTower.GetComponent<Tower>().ActivateBuff(buff);
                    break;
                default:
                    break;
            }
        }
        else
        {
            SpawnBeatHitResult();
            return;
        }
    }

    public void TryToPlaceTower(GameObject tower)
    {
        //checks if resource is available and if the tower is on cooldown
        if(CombatManager.Instance.resourceNum >= tower.GetComponent<Tower>().towerInfo.resourceCost 
            && !TowerManager.Instance.CheckIfOnCoolDown(tower.GetComponent<Tower>().towerInfo.type) &&
            tile != null && tile.placedTower == null) 
        {
            if(CombatManager.Instance.resourceNum >= 150)
            {
                TowerManager.Instance.SetTower(tower, transform.position, tile, tower.GetComponent<Tower>().towerInfo.type, CheckOnBeat(), true);
                CombatManager.Instance.resourceNum -= 150;
            }
            else if(CombatManager.Instance.resourceNum < 149)
            {
                TowerManager.Instance.SetTower(tower, transform.position, tile, tower.GetComponent<Tower>().towerInfo.type, CheckOnBeat(), false);
                CombatManager.Instance.resourceNum -= tower.GetComponent<Tower>().towerInfo.resourceCost;
            }

            Debug.Log("Place towers");

            SpawnBeatHitResult();
            TogglePlacementMenu();
            placingTower = false;
            return;
        }
        else //if tower can't be placed
        {
            //TogglePlacementMenu();

            return;
        }    
    }

    public void TogglePlacementMenu()
    {
        if (destructMode || GameManager.Instance.winState || GameManager.Instance.loseState || ConductorV2.instance.countingIn) return;

        towerSelectMenuOpened = true;
        placementMenu.SetActive(towerSelectMenuOpened);

        if (GameManager.Instance.tutorialRunning && towerPlacementMenuSequence && towerSelectMenuOpened)
        {
            towerPlacementMenuSequencePassed = true;
            TutorialManager.Instance.LoadNextTutorialDialogue();
            towerPlacementMenuSequence = false;
            towerPlaceSequence = true;
            CombatManager.Instance.towerDisplay.SetActive(true);
        }


    }

    public void ClosePlacementMenu()
    {
        placingTower = false;
        towerSelectMenuOpened = false;
        placementMenu.SetActive(towerSelectMenuOpened);
    }

    public void HighlightPlacementSlot(Vector2 direction)
    {
        if (!towerSelectMenuOpened || placingTower) return;

        placingTower = true;


        if (direction == Vector2.up)
        {
            if (towerSelectMenuOpened && tile.placedTower == null)
            {
                TryToPlaceTower(SlotW.GetComponent<TowerButton>().tower);

                //PlacementFeedback(SlotW.GetComponent<AudioSource>(), "Check Slot 01");

                return;
            }

            PlacementFeedback(SlotW.GetComponent<AudioSource>(), "Check Slot 01");

        }
        else if (direction == Vector2.left)
        {
            if (towerSelectMenuOpened && tile.placedTower == null)
            {
                TryToPlaceTower(SlotD.GetComponent<TowerButton>().tower);

                //PlacementFeedback(SlotD.GetComponent<AudioSource>(), "Check Slot 04");

                return;
            }

            PlacementFeedback(SlotD.GetComponent<AudioSource>(), "Check Slot 04");

        }
        else if (direction == Vector2.down)
        {
            if (towerSelectMenuOpened && tile.placedTower == null)
            {
                TryToPlaceTower(SlotS.GetComponent<TowerButton>().tower);

                //PlacementFeedback(SlotS.GetComponent<AudioSource>(), "Check Slot 03");

                return;
            }

            PlacementFeedback(SlotS.GetComponent<AudioSource>(), "Check Slot 03");

        }
        else if (direction == Vector2.right)
        {

            if (towerSelectMenuOpened && tile.placedTower == null)
            {
                TryToPlaceTower(SlotA.GetComponent<TowerButton>().tower);

                //PlacementFeedback(SlotA.GetComponent<AudioSource>(), "Check Slot 02");

                return;
            }

            PlacementFeedback(SlotA.GetComponent<AudioSource>(), "Check Slot 02");

        }

    }


    public void Move(Vector2 direction)
    {
        if (desiredMovement == Vector3.zero || towerSelectMenuOpened || isMoving || GameManager.Instance.winState || GameManager.Instance.loseState) 
            return;
        

        StartCoroutine(MovePlayer(direction));
    }

    private IEnumerator MovePlayer(Vector3 direction)
    {
        
        isMoving = true;

        float elapsedTime = 0;

        originPos = transform.position;

        targetPos = originPos + (direction * 1.2f);


        //bounding box function
        if((targetPos.x <= -5 || targetPos.x >= 9) || (targetPos.y <= -3.5 || targetPos.y >= 2))
        {
            isMoving = false;
            desiredMovement = Vector3.zero;
            yield break;
        }

        while(elapsedTime < timeToMove)
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
    private void OnTriggerStay2D(Collider2D collision)
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
            tile = collision.gameObject.GetComponent<Tile>();
        }
    }

    //I will store all the on beat tutorial stuff here
    public void Pulse()
    {
        //Debug.Log("pulse");
        cursorSprite.transform.localScale = pulseSize;
        //beatIsHit = false;
    }

    public void CheckPianoResult(Tower tower)
    {
        switch (CheckOnBeat())
        {
            case _BeatResult.great:
                CombatManager.Instance.resourceNum += tower.towerInfo.resourceGain;
                SpawnResourceGenParticles(pianoResourceGenParticles, pianoResourceGenParticlesInstance);
                break;
            case _BeatResult.perfect:
                CombatManager.Instance.resourceNum += tower.towerInfo.resourceGain;
                SpawnResourceGenParticles(pianoResourceGenParticles, pianoResourceGenParticlesInstance);
                break;
            default:
                break;
        }


        //SpawnResourceGenParticles(pianoResourceGenParticles, pianoResourceGenParticlesInstance);
    }

    public void SpawnBeatHitResult()
    {
        if (GameManager.Instance.winState || GameManager.Instance.loseState || GameManager.Instance.isGamePaused || beatIsHit) return;
        
        beatIsHit = true;
        GameObject beatResult = Instantiate(beatHitResultPrefab, new Vector3(transform.position.x, transform.position.y + 0.6f, transform.position.z), Quaternion.identity);

        switch (CheckOnBeat())
        {
            case _BeatResult.late:
                beatResult.GetComponent<TMP_Text>().text = "late";//miss beat
                beatResult.GetComponent<TMP_Text>().color = Color.grey;
                break;
            case _BeatResult.miss:
                beatResult.GetComponent<TMP_Text>().text = "miss";//miss beat
                beatResult.GetComponent<TMP_Text>().color = Color.red;
                break;
            case _BeatResult.early:
                beatResult.GetComponent<TMP_Text>().text = "early";
                beatResult.GetComponent<TMP_Text>().fontSize = 40;
                beatResult.GetComponent<TMP_Text>().color = Color.yellow;
                break;
            case _BeatResult.great:
                // Cursor resource generation
                CombatManager.Instance.resourceNum += 1;
                SpawnResourceGenParticles(cursorResourceGenParticles, cursorResourceGenParticlesInstance);

                beatResult.GetComponent<TMP_Text>().text = "great";
                beatResult.GetComponent<TMP_Text>().fontSize = 45;
                beatResult.GetComponent<TMP_Text>().color = Color.blue;
                break;
            case _BeatResult.perfect:
                // Cursor resource generation
                CombatManager.Instance.resourceNum += 3;
                SpawnResourceGenParticles(cursorResourceGenParticles, cursorResourceGenParticlesInstance);

                beatResult.GetComponent<TMP_Text>().text = "perfect";
                beatResult.GetComponent<TMP_Text>().fontSize = 50;
                beatResult.GetComponent<TMP_Text>().color = Color.green;
                break;
            default:
                // Cursor resource generation
                CombatManager.Instance.resourceNum += 3;
                SpawnResourceGenParticles(cursorResourceGenParticles, cursorResourceGenParticlesInstance);

                beatResult.GetComponent<TMP_Text>().text = "perfect";
                beatResult.GetComponent<TMP_Text>().fontSize = 50;
                beatResult.GetComponent<TMP_Text>().color = Color.green;
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

        if (ConductorV2.instance.beatDuration >= ConductorV2.instance.perfectBeatThreshold)//perfect beat hit 
        {
            ComboManager.Instance.IncreaseCombo();
            return _BeatResult.perfect;
            
        }
        else if (ConductorV2.instance.beatDuration >= ConductorV2.instance.greatBeatThreshold)//great beat hit
        {
            ComboManager.Instance.IncreaseCombo();
            return _BeatResult.great;
        }
        else if (ConductorV2.instance.beatDuration >= ConductorV2.instance.earlyBeatThreshold)//early beat hit
        {
            return _BeatResult.early;

        }
        else if (ConductorV2.instance.beatDuration >= ConductorV2.instance.missBeatThreshold)//miss beat hit
        {
            ComboManager.Instance.ResetCombo();
            return _BeatResult.miss;

        }
        else if (ConductorV2.instance.beatDuration < ConductorV2.instance.missBeatThreshold)//late beat hit
        {
            return _BeatResult.late;
        }
        else
        {
            return _BeatResult.miss;
        }
    }


    private void SpawnResourceGenParticles(ParticleSystem particlesSource, ParticleSystem particlesInstance)
    {
        particlesInstance = Instantiate(particlesSource, transform.position, Quaternion.identity);
    }

    private void PlacementFeedback(AudioSource invalidPlacementSFX, string invalidPlacementAnimation)
    {
        placingTower = false;
        Debug.Log("try to place tower");
        //Play the sound & animation on the corresponding tower slot when the tower cannot be placed
        invalidPlacementSFX.Play();
        radialMenuAnimator.SetTrigger(invalidPlacementAnimation);
        //radialMenuAnimator.ResetTrigger(invalidPlacementAnimation);
    }

}
