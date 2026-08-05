using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region dont touch this
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance is null)
            {
                Debug.LogError("GameManager is NULL");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    #region Variables
    public Transform globalParent;

    [Space(20)][Header("<b><size=15>Conductor<b><size=15>")]
    [Line(255,255,255)]
    public GameObject conductor;

    [Space(20)][Header("<b><size=15>Audio<b><size=15>")]
    [Line(255,255,255)]
    public AudioSource menuMusic;
    public AudioSource buttonHighlightSFX;

    [Space(20)][Header("<b><size=15>Screens<b><size=15>")]
    [Line(255,255,255)]
    public GameObject titleRoot;
    [Space(10)]
    public GameObject menuRoot;
    [Space(10)]
    public GameObject dialogueRoot;
    [Space(10)]
    public GameObject combatRoot;
    public GameObject winScreen;
    public GameObject failScreen;
    [Space(10)]
    public GameObject pauseMenuRoot;
    [SerializeField] private GameObject settings;
    [SerializeField] private Button restartEncounterButton;
    public GameObject exitMenuRoot;

    [Space(20)][Header("<b><size=15>Pausing<b><size=15>")]
    [Line(255,255,255)]
    [SerializeField] public bool isGamePaused = false;
    
    //[SerializeField] private GameObject pauseMenu;
    /*
    public GameObject exitMenuRoot;
    public GameObject showcaseCredits;

    [Header("Combat")]
    //[SerializeField] private Slider healthSlider;
    [SerializeField] public int _maxHealth = 5;
    [SerializeField] public int _currentHealth = 0;
    [SerializeField] public bool combatRunning = false;
    
    public TextMeshProUGUI waveCounter;
    public TextMeshProUGUI enemyCounter;
    public GameObject playerInputManager;
    public bool lostHealth = false;
    public bool isDynamicMusicActive = true;

    [Header("Pause Menu")]
    [SerializeField] public bool isGamePaused = false;
    */

    [Space(20)][Header("<b><size=15>Combat<b><size=15>")]
    [Line(255,255,255)]
    public GameObject playerInputManager;
    public LayerMask interactableMask;
    [Space(10)]
    [SerializeField] public bool combatRunning = false;
    [Space(10)][Header("Health")]
    [SerializeField] public int _maxHealth = 5;
    [SerializeField] public int _currentHealth = 0;
    public bool hasLostHealth = false;
    //[SerializeField] private Slider healthSlider;
    [Space(10)][Header("Enemy UI")]
    public TextMeshProUGUI waveCounter;
    public TextMeshProUGUI enemyCounter;
    
    [Space(20)][Header("<b><size=15>Encounter<b><size=15>")]
    [Line(255,255,255)]
    public EncounterCreator currentEncounter;
    public bool encounterRunning = false;
    public bool tutorialRunning = false;
    public bool winState = false;
    public bool failState = false;

    [Space(20)][Header("<b><size=15>Tutorial<b><size=15>")]
    [Line(255,255,255)]
    public DynamicSongCreator tutorialSong;

    [Space(20)][Header("<b><size=15>Conductor<b><size=15>")]
    [Line(255,255,255)]
    public float audioOffset;
    public float inputOffset;

    [Space(20)][Header("<b><size=15>Level Buttons<b><size=15>")]
    [Line(255,255,255)]
    [SerializeField] private GameObject levelButtons;
    [SerializeField] private Transform levelParent;
    [SerializeField] private ScrollView levelScrollView;
    [SerializeField] private int viewPortOffset = -150;
    private ItemButtonEvent _eventItemOnSelect;
    private ItemButtonEvent _eventItemOnSubmit;
    [SerializeField] private Selectable returnToMainMenuButton;
    public GameObject currentSelectedButton;

    [Space(20)][Header("<b><size=15>Level Info Panel<b><size=15>")]
    [Line(255,255,255)]
    public GameObject infoPanel;
    public TextMeshProUGUI levelNameText;
    public TextMeshProUGUI levelNumText;
    public TextMeshProUGUI objectiveText01;
    public TextMeshProUGUI objectiveText02;
    public TextMeshProUGUI objectiveText03;
    public Image levelPreviewImage;
    public Image objectiveImage01;
    public Image objectiveImage02;
    public Image objectiveImage03;
    public List<Image> intelImages;
    public int imageIndex = 0;

    [Space(20)][Header("<b><size=15>Modifiers<b><size=15>")]
    [Line(255,255,255)]
    [Header("<b><size=15>General<b><size=15>")]
    public bool isOneHealth = false; // Reduces player HP to 1 hit from failing                    EFFECT IMPLEMENTED / NOT TESTED / NO TOGGLE YET
    public bool isNoFail = false; // Prevents the player from failing a level                    EFFECT IMPLEMENTED / NOT TESTED / NO TOGGLE YET
    public bool isTowerFragile = false; // Reduces all tower HP values to 1 hit from destruction                    EFFECT IMPLEMENTED / NOT TESTED / NO TOGGLE YET
    public bool isEnemyFragile = false; // Reduces all enemy HP values to 1 hit from destruction                    EFFECT IMPLEMENTED / NOT TESTED / NO TOGGLE YET
    public bool isDoubleTime = false; // Doubles game speed (does not currently increase music speed)                    EFFECT IMPLEMENTED / NOT TESTED / NO TOGGLE YET
    public bool isHalfTime = false; // Halves game speed (does not currently decrease music speed)                    EFFECT IMPLEMENTED / NOT TESTED / NO TOGGLE YET
    public bool isPreciseTiming = false; // Tightens the perfect judgement window                    EFFECT IMPLEMENTED / NOT TESTED / NO TOGGLE YET
    public bool isGenerousTiming = false; // Increases the perfect judgement window                    EFFECT IMPLEMENTED / NOT TESTED / NO TOGGLE YET
    public bool isPerfectsOnly = false; // Only perfects allowed                    EFFECT IMPLEMENTED / NOT TESTED / NO TOGGLE YET
    public bool isHitsOnly = false; // No misses allowed                    EFFECT IMPLEMENTED / NOT TESTED / NO TOGGLE YET
    public bool isLimitedResources = false; // Limits the resource cap, preventing the player from accumulating enough resources to build multiple towers in short succession                    EFFECT IMPLEMENTED / NOT TESTED / NO TOGGLE YET
    public bool isInfiniteResources = false; // Grants the player infinite resources                    EFFECT IMPLEMENTED / NOT TESTED / NO TOGGLE YET
    [Space(10)][Header("<b><size=15>Misc<b><size=15>")]
    public bool isNotesHidden = false; // Hides the input prompts for tower attack patterns
    public bool isMirrorMode = false; // Mirrors grid tiles and enemy spawn locations along the x axis
    public bool isInvisibleEnemies = false; // Makes enemies invisible (VFX & SFX are still enabled)
    public bool isRandomEnemies = false; // Spawns random enemy types in place of usual enemy spawns
    public bool isDeafened = false; // BGM & SFX are muted/muffled
    [Space(10)][Header("<b><size=15>Info<b><size=15>")]
    public TextMeshProUGUI modifierName;
    public TextMeshProUGUI modifierDescription;
    public List<string> modifierNames = new List<string>();
    public List<string> modifierDescriptions = new List<string>();

    [Space(20)][Header("<b><size=15>Modifier Toggles<b><size=15>")]
    [Line(255,255,255)]
    [Header("<b><size=15>General<b><size=15>")]
    public Toggle oneHealthToggle;
    public Toggle noFailToggle;
    public Toggle towerFragileToggle;
    public Toggle enemyFragileToggle;
    public Toggle doubleTimeToggle;
    public Toggle halfTimeToggle;
    public Toggle preciseTimingToggle;
    public Toggle generousTimingToggle;
    public Toggle perfectsOnlyToggle;
    public Toggle hitsOnlyToggle;
    public Toggle limitedResourceToggle;
    public Toggle infiniteResourcesToggle;

    [Space(20)][Header("<b><size=15>Tower Loadout<b><size=15>")]
    [Line(255,255,255)]
    public List<TowerPlacementInfo> towers = new List<TowerPlacementInfo>();

    [Space(20)][Header("<b><size=15>Tower Repetition Assets<b><size=15>")]
    [Line(255,255,255)]
    public Sprite recordingSpr;
    public List<Sprite> repeatSprites = new List<Sprite>();

    [Space(20)][Header("<b><size=15>Level Scoring<b><size=15>")]
    [Line(255,255,255)]
    public List<int> pointHolder = new List<int>();
    public int healthRemainingPointGain = 100;
    #endregion

    #region Start
    // Start is called before the first frame update
    void Start()
    {
        //make sure when scene starts title root is set active
        titleRoot.SetActive(true);
        menuRoot.SetActive(false);
        combatRoot.SetActive(false);
        dialogueRoot.SetActive(false);
        settings.SetActive(false);
        pauseMenuRoot.SetActive(false);
        exitMenuRoot.SetActive(false);

        //setup interactable mask
        interactableMask = LayerMask.GetMask("Enemy", "Tower", "Stage");

        _currentHealth = _maxHealth;

        QualitySettings.maxQueuedFrames = 1;

        //Debug.Log(QualitySettings.maxQueuedFrames + " frame");
        //Debug.Log(QualitySettings.vSyncCount + " Vsync");
        Cursor.lockState = CursorLockMode.Locked;
        playerInputManager.SetActive(false);

        LoadLevelButtons();
        UpdateAllLevelSelectButtonNavigationReferences();
    }
    #endregion

    #region Update
    // Update is called once per frame
    void Update()
    {
        /*
        //Manages health only while combat is running
        if (combatRunning)
        {
            Health();
        }
        */
    }
    #endregion

    #region Level buttons
    private void LoadLevelButtons()
    {
        EncounterCreator[] encounters = Resources.LoadAll<EncounterCreator>("Encounters/");

        foreach (EncounterCreator encounter in encounters)
        {
            CreateButton(encounter);
        }
    }

    private ItemButton CreateButton(EncounterCreator encounter)
    {
        //Debug.Log(encounter);

        GameObject gameObject;
        ItemButton item;

        gameObject = Instantiate(levelButtons, Vector3.zero, Quaternion.identity);
        gameObject.transform.SetParent(levelParent);

        gameObject.transform.localScale = new Vector3(1f, 1f, 1);
        
        gameObject.name = encounter.name;

        //set params
        item = gameObject.GetComponent<ItemButton>();
        item.ItemNameValue = encounter.LevelLabel;
        item.heldEncounter = encounter;
        item.fill.color = encounter.fillColor;
        item.GetComponent<ItemButton>().viewportOffset = viewPortOffset;
        viewPortOffset += levelScrollView.viewportOffsetValue;

        //add event listeners
        //gameObject.GetComponent<Button>().onClick.AddListener(LoadStage(encounter));
        item.OnSubmitEvent.AddListener((ItemButton) => { HandleEventItemOnSubmit(item); });
        item.OnSelectEvent.AddListener((ItemButton) => { HandleEventItemOnSelect(item); });

        return item;
    }

    public void HandleEventItemOnSelect(ItemButton item)
    {
        levelScrollView.HandleOnSelect(item);
        MenuEventManager.Instance.UpdateLastSelectedLevel();
    }

    // Close main menu (level select) & load encounter when level button is pressed
    public void HandleEventItemOnSubmit(ItemButton item)
    {
        //combatRoot.SetActive(true); //enable combat scene
        buttonHighlightSFX.Play(); //play button feedback sfx
        MenuEventManager.Instance.UpdateLastSelectedLevel(); //record last selected level button
        //MenuEventManager.Instance.CloseMainMenu(); //stop main menu music, update last selected level, disable main menu
        currentSelectedButton = item.gameObject;

        LoadStage(item.heldEncounter); //load encounter
    }

    // 
    private void UpdateAllLevelSelectButtonNavigationReferences()
    {
        ItemButton[] children = levelParent.GetComponentsInChildren<ItemButton>();

        if (children.Length < 2)
        {
            return; //must have at least 2 buttons
        }

        ItemButton itemRef;
        Navigation navigation;

        for (int i = 0; i < children.Length; i++)
        {
            itemRef = children[i];

            navigation = itemRef.gameObject.GetComponent<Button>().navigation;

            navigation.selectOnLeft = GetNavigationLeft(i, children.Length);
            navigation.selectOnRight = GetNavigationRight(i, children.Length);

            itemRef.nextButton = navigation.selectOnRight.gameObject;

            navigation.selectOnUp = returnToMainMenuButton;

            itemRef.gameObject.GetComponent<Button>().navigation = navigation;

            //level locking functions
            itemRef.heldEncounter.levelThatUnlocks = GetNextEncounter(i, children.Length);

            if(itemRef.heldEncounter.isLevelLocked)//when level is declared locked
            {
                itemRef.gameObject.SetActive(false);
            }
        }
    }

    private Selectable GetNavigationRight(int indexCurrent, int length)
    {
        ItemButton itemRight;

        if (indexCurrent == length - 1) //last item
        {
            //looping dont set anything here
            return null;
        }
        else
        {
            itemRight = levelParent.GetChild(indexCurrent + 1).GetComponent<ItemButton>();

            //testing only remove this when testing is done
            if (itemRight.heldEncounter.isLevelLocked) //in testing if a button is unlocked but the next is locked this will look for the next available selectable
            {
                return GetNextActiveNavigation(indexCurrent, length, itemRight);
            }
        }

        return itemRight.GetComponent<Selectable>();
    }

    //TESTING ONLY
    private Selectable GetNextActiveNavigation(int indexCurrent, int length, ItemButton ifAllSelectablesAreLocked)
    {
        ItemButton itemActive = null;
        ItemButton[] children = levelParent.GetComponentsInChildren<ItemButton>();

        for (int i = indexCurrent + 1; i < children.Length; i++)
        {
            if (children[i].heldEncounter.isLevelLocked == false)
            {
                itemActive = children[i];
                return itemActive.GetComponent<Selectable>();
            }

            else
                itemActive = ifAllSelectablesAreLocked;
        }
        //Debug.Log(itemActive.gameObject);
        return itemActive.GetComponent<Selectable>();
    }


    private Selectable GetNavigationLeft(int indexCurrent, int length)
    {
        ItemButton itemLeft;

        if (indexCurrent == 0)
        {
            //looping dont set anything here
            return null;
        }
        else
        {
            itemLeft = levelParent.GetChild(indexCurrent - 1).GetComponent<ItemButton>();
        }

        return itemLeft.GetComponent<Selectable>();
    }

    private EncounterCreator GetNextEncounter(int indexCurrent, int length)
    {
        ItemButton itemNext;

        if (indexCurrent == length - 1) //last item
        {
            //looping dont set anything here
            return null;
        }
        else
        {
            itemNext = levelParent.GetChild(indexCurrent + 1).GetComponent<ItemButton>();
        }

        return itemNext.heldEncounter;
    }
    #endregion

    #region Pausing
    public void HandlePauseMenuInput()
    {
        //allows player to use pause menu in combat, level select and dialogue
        if (combatRunning || menuRoot.activeSelf || tutorialRunning)
        {
            if (dialogueRoot.activeSelf) return;
            //flips the isGamePaused bool
            isGamePaused = !isGamePaused;

            //game is paused
            if (isGamePaused)
            {
                PauseGame();
            }
            //game is unpaused
            else
            {
                ResumeGame();
            }
            
        }
    }

    public void PauseGame()
    {
        if (dialogueRoot.activeSelf || menuRoot.activeSelf)
        {
            //set the restart encounter button to disabled
            restartEncounterButton.interactable = false;
        }
        else
        {
            restartEncounterButton.interactable = true;
            //conductor.SetActive(false);
        }
        if (combatRunning && !dialogueRoot.activeSelf)
        {

            ConductorV2.instance.PauseMusic();
        }

        Cursor.lockState = CursorLockMode.Locked;
        isGamePaused = true;
        pauseMenuRoot.SetActive(true);
        MenuEventManager.Instance.PauseMenuOpen();
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        if (combatRunning && !dialogueRoot.activeSelf)
        {
            Cursor.lockState = CursorLockMode.Locked;
            conductor.SetActive(true);
            ConductorV2.instance.ResumeMusic();
        }

        isGamePaused = false;
        pauseMenuRoot.SetActive(false);
        exitMenuRoot.SetActive(false);
        settings.SetActive(false);
        MenuEventManager.Instance.PauseMenuClose();
        Time.timeScale = 1;
    }
    #endregion

    #region Health
    public void Damage()
    {
        if (!isNoFail)
        {
            _currentHealth -= 1;
        }
        
        ScoreManager.Instance.ResetCombo();

        hasLostHealth = true; //set flag for failing objective 02
    }

    void Health()
    {
        //healthSlider.maxValue = _maxHealth;
        //healthSlider.value = _currentHealth;
    }
    #endregion

    #region Tutorial
    public void LoadTutorial()
    {

        Camera.main.transform.position = Camera.main.GetComponent<CameraPositions>().CombatCameraPos;
        Camera.main.transform.rotation = Quaternion.Euler(new Vector3(60, 0, 0));
        Camera.main.fieldOfView = 60;

        _currentHealth = _maxHealth;
        
        // MUSIC MANAGEMENT
        menuMusic.Stop();

        // ROOT & VARIABLE MANAGEMENT
        combatRoot.SetActive(true);
        CombatManager.Instance.allEnemiesSpawned = false;
        CombatManager.Instance.tutorialManager.SetActive(true);
        Spawner.Instance.allEnemiesSpawned = false;
        tutorialRunning = true;
        winState = false;
        winScreen.SetActive(false);
        failState = false;
        failScreen.SetActive(false);
        playerInputManager.SetActive(true);

        CombatManager.Instance.enemyTimerObject.SetActive(false);
        //CombatManager.Instance.healthBar.SetActive(false);
        CombatManager.Instance.resources.SetActive(false);
        //CombatManager.Instance.towerDisplay.SetActive(false);
        CombatManager.Instance.feverBar.SetActive(false);
        //CombatManager.Instance.metronome.SetActive(false);
        CombatManager.Instance.waveCounter.SetActive(false);
        CombatManager.Instance.combo.SetActive(false);


        CombatManager.Instance.tutorialManager.SetActive(true);


        CursorTD.Instance.movementSequence = false;
        CursorTD.Instance.towerPlacementMenuSequence = false;
        CursorTD.Instance.towerPlacementMenuSequencePassed = false;
        CursorTD.Instance.towerPlaceSequence = false;
        CursorTD.Instance.towerBuffSequence = false;
        CursorTD.Instance.feverModeSequence = false;

        CursorTD.Instance.InitializeCursor();

        TowerManager.Instance.ResetTowerManager();

        CombatManager.Instance.enemyTotal = 7;


        List<Wave> empty = new List<Wave>();

        Spawner.Instance.currentWaves = empty;

        Spawner.Instance.numberOfEnemiesToSpawn = 7;
        Spawner.Instance.numberOfPickupsToSpawn = 0;

        Spawner.Instance.startOnce = false;

        Spawner.Instance.currentNumberOfEnemiesSpawned = 0;
        Spawner.Instance.currentNumberOfPickupsSpawned = 0;

        Spawner.Instance.ResetSpawner();

        TutorialManager.Instance.LoadTutorial();

        StageManager.Instance.SetStageEnvironment(currentEncounter.stage);


        ConductorV2.instance.CountUsIn(currentEncounter.combatEncounter.dynamicSong.bpm);
    }

    public void TutorialWinState()
    {
        if (winState) return;
        winState = true;

        CombatManager.Instance.tutorialManager.SetActive(false);

        CombatManager.Instance.EndEncounter();
        encounterRunning = false;
        Cursor.lockState = CursorLockMode.Locked;

        //winScreen.SetActive(true);
        //conductor.SetActive(false);
        //MenuEventManager.Instance.OpenWinScreen(0);
        dialogueRoot.SetActive(true);
        DialogueManager.Instance.LoadDialogue(currentEncounter.endDialogue);
        ConductorV2.instance.StopMusic();
    }
    #endregion

    /*
    public void LoadStage(EncounterCreator encounter)
    {
        currentEncounter = encounter;
        encounterRunning = true;

        //tutorialRunning = encounter.isTutorial;

        SetCombatScene(); 

        if (currentEncounter.introDialogue == null)
        {
            //LoadingScreenManager.Instance.StartLoading(MenuEventManager.Instance.loadoutScreen, menuRoot);
            MenuEventManager.Instance.OpenLoadoutMenu();
        }
        else
        {
            LoadingScreenManager.Instance.StartLoading(dialogueRoot, menuRoot);
            DialogueManager.Instance.LoadDialogue(currentEncounter.introDialogue);
        } 

        ResetCombatState();
    }
    */

    #region Start level
    public void LoadStage(EncounterCreator encounter)
    {
        currentEncounter = encounter;
        encounterRunning = true;
        
        StartCoroutine(LoadingScreenManager.Instance.StartLoading());
    }

    public void SetCombatScene()
    {
        combatRoot.SetActive(true); //enable combat scene

        StageManager.Instance.SetStageEnvironment(currentEncounter.stage); //load encounter grid data
        StageManager.Instance.SpawnStageGrid(currentEncounter.combatEncounter);

        //CombatManager.Instance.combatInterface.SetActive(false); //disable combat UI
    }
    
    public void StartCombat()
    {
        combatRunning = true; //set game state to combat
        CombatManager.Instance.tutorialManager.SetActive(false); //disable tutorial manager
        CombatManager.Instance.LoadEncounter(currentEncounter.combatEncounter); //load encounter data
    }
    #endregion

    #region End level
    public void FailLevel()
    {
        if (failState) return; //prevents function from continuing if already in the fail state
        failState = true;

        Cursor.lockState = CursorLockMode.Locked; //lock player cursor movement
        //CombatManager.Instance.EndEncounter(); //end current encounter
        CombatManager.Instance.StartFailSequence(); //open fail screen and set active object
    }

    public void WinLevel()
    {
        if (winState) return; //prevents function from continuing if already in the win state
        winState = true;
        
        Cursor.lockState = CursorLockMode.Locked; //lock player cursor movement
        CombatManager.Instance.StartWinSequence();

        encounterRunning = false;

        // OBJECTIVES
        // level cleared
        currentEncounter.clearedObjective01 = true;

        // level cleared without losing health
        if (hasLostHealth == false) {
            currentEncounter.clearedObjective02 = true;
        }

        //check if unique level objective was cleared
        //currentEncounter.clearedObjective03 = LevelObjectiveManager.Instance.CheckIfObjectiveWasCompleted(currentEncounter.data.uniqueLevel3Objective);

        // SCORE
        if (isOneHealth)
        {
            pointHolder.Add(healthRemainingPointGain * _maxHealth); 
        }
        else
        {
            pointHolder.Add(healthRemainingPointGain * _currentHealth); 
        }
    }

    public void StartWinLevelProcess()
    {
        MenuEventManager.Instance.OpenWinScreen(); //open win screen and set active object
        
        // TEMPORARILY DISABLED UNTIL NEW WIN SCREEN IS CONNECTED
        //winScreen.GetComponent<ResultScreenInfo>().WriteToResultScreen(true, currentEncounter.encounterName, ScoreManager.Instance.score, ScoreManager.Instance.highestCombo + ScoreManager.Instance.score, _currentHealth == _maxHealth, false);
    }
    #endregion

    #region Reset
    public void ResetCombatState()
    {
        // reset win & fail states
        winState = false;
        winScreen.SetActive(false);
        failState = false;
        failScreen.SetActive(false);
        
        // reset objective flags
        hasLostHealth = false;

        // reset player health
        if (isOneHealth)
        {
            _currentHealth = 1;
        }
        else
        {
            _currentHealth = _maxHealth;
        }
        
        // reset combo, multiplier, & score
        ScoreManager.Instance.ResetStageScoreData();
        
        // set flags for all objects having been spawned back to false
        CombatManager.Instance.allEnemiesSpawned = false;
        CombatManager.Instance.allPickupsSpawned = false;

        // set enemy & pickup totals back to 0
        CombatManager.Instance.enemiesDefeated = 0;
        CombatManager.Instance.enemyTotal = 0;
        CombatManager.Instance.pickupTotal = 0;

        Debug.Log("Combat State Reset");
    }
    #endregion

    #region Unlock triggers
    public void UnlockLevel(EncounterCreator wonLevel)
    {
        if (wonLevel.levelThatUnlocks != null)
        {
            currentSelectedButton.GetComponent<ItemButton>().nextButton.SetActive(true);
            wonLevel.levelThatUnlocks.isLevelLocked = false;
        }
    }

    public void UnlockUpgrade(Tower tower, UpgradeNum upgrade)
    {
        switch (upgrade)
        {
            case UpgradeNum.One:
                tower.towerInfo.isUpgradeOneLocked = false;
                break;
            case UpgradeNum.Two:
                tower.towerInfo.isUpgradeTwoLocked = false;
                break;
            case UpgradeNum.Three:
                tower.towerInfo.isUpgradeThreeLocked = false;
                break;
        }
    }
    #endregion

    #region Modifier toggles
    public void UpdateModDescription(int modifierInfoIndex)
    {
        modifierName.text = modifierNames[modifierInfoIndex];
        modifierDescription.text = modifierDescriptions[modifierInfoIndex];
    }
    
    public void ToggleOneHealth()
    {
        if (oneHealthToggle.isOn && noFailToggle.isOn)
        {
            isOneHealth = oneHealthToggle.isOn;

            noFailToggle.isOn = false;
            isNoFail = noFailToggle.isOn;
        }
        else
        {
            isOneHealth = oneHealthToggle.isOn;
        }
    }

    public void ToggleNoFail()
    {
        if (oneHealthToggle.isOn && noFailToggle.isOn)
        {
            isNoFail = noFailToggle.isOn;

            oneHealthToggle.isOn = false;
            isOneHealth = oneHealthToggle.isOn;
        }
        else
        {
            isNoFail = noFailToggle.isOn;
        }
    }

    public void ToggleFragileTowers()
    {
        isTowerFragile = towerFragileToggle.isOn;
    }

    public void ToggleFragileEnemies()
    {
        isEnemyFragile = enemyFragileToggle.isOn;
    }

    public void ToggleDoubleTime()
    {
        if (doubleTimeToggle.isOn && halfTimeToggle.isOn)
        {
            isDoubleTime = doubleTimeToggle.isOn;

            halfTimeToggle.isOn = false;
            isHalfTime = halfTimeToggle.isOn;
        }
        else
        {
            isDoubleTime = doubleTimeToggle.isOn;
        }
    }

    public void ToggleHalfTime()
    {
        if (doubleTimeToggle.isOn && halfTimeToggle.isOn)
        {
            isHalfTime = halfTimeToggle.isOn;

            doubleTimeToggle.isOn = false;
            isDoubleTime = doubleTimeToggle.isOn;
        }
        else
        {
            isHalfTime = halfTimeToggle.isOn;
        }
    }

    public void TogglePrecision()
    {
        if (preciseTimingToggle.isOn && generousTimingToggle.isOn)
        {
            isPreciseTiming = preciseTimingToggle.isOn;

            generousTimingToggle.isOn = false;
            isGenerousTiming = generousTimingToggle.isOn;
        }
        else
        {
            isPreciseTiming = preciseTimingToggle.isOn;
        }
    }

    public void ToggleSenzaMisura()
    {
        if (preciseTimingToggle.isOn && generousTimingToggle.isOn)
        {
            isGenerousTiming = generousTimingToggle.isOn;

            preciseTimingToggle.isOn = false;
            isPreciseTiming = preciseTimingToggle.isOn;
        }
        else
        {
            isGenerousTiming = generousTimingToggle.isOn;
        }
    }

    public void TogglePerfectionist()
    {
        if (perfectsOnlyToggle.isOn && hitsOnlyToggle.isOn)
        {
            isPerfectsOnly = perfectsOnlyToggle.isOn;

            hitsOnlyToggle.isOn = false;
            isHitsOnly = hitsOnlyToggle.isOn;
        }
        else
        {
            isPerfectsOnly = perfectsOnlyToggle.isOn;
        }
    }

    public void ToggleNoMissTakes()
    {
        if (perfectsOnlyToggle.isOn && hitsOnlyToggle.isOn)
        {
            isHitsOnly = hitsOnlyToggle.isOn;

            perfectsOnlyToggle.isOn = false;
            isPerfectsOnly = perfectsOnlyToggle.isOn;
        }
        else
        {
            isHitsOnly = hitsOnlyToggle.isOn;
        }
    }

    public void ToggleLowBattery()
    {
        if (limitedResourceToggle.isOn && infiniteResourcesToggle.isOn)
        {
            isLimitedResources = limitedResourceToggle.isOn;

            infiniteResourcesToggle.isOn = false;
            isInfiniteResources = infiniteResourcesToggle.isOn;
        }
        else
        {
            isLimitedResources = limitedResourceToggle.isOn;
        }
    }

    public void ToggleInfinitePower()
    {
        if (limitedResourceToggle.isOn && infiniteResourcesToggle.isOn)
        {
            isInfiniteResources = infiniteResourcesToggle.isOn;

            limitedResourceToggle.isOn = false;
            isLimitedResources = limitedResourceToggle.isOn;
        }
        else
        {
            isInfiniteResources = infiniteResourcesToggle.isOn;
        }   
    }
    #endregion
}

public enum UpgradeNum
{
    One, Two, Three
}
