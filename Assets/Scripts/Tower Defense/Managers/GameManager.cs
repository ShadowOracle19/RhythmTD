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

    public Transform globalParent;
    public AudioSource menuMusic;
    public AudioSource buttonHighlightSFX;

    [SerializeField] public GameObject winScreen;
    [SerializeField] public GameObject failScreen;
    [SerializeField] private GameObject conductor;

    [Header("Screen Roots")]
    public GameObject combatRoot;
    public GameObject dialogueRoot;
    public GameObject menuRoot;
    [SerializeField] private GameObject settings;
    public GameObject pauseMenuRoot;
    public GameObject titleRoot;
    public GameObject exitMenuRoot;
    public GameObject showcaseCredits;

    [Header("Combat")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] public int _maxHealth = 5;
    [SerializeField] public int _currentHealth = 0;
    [SerializeField] public bool combatRunning = false;
    public TextMeshProUGUI waveCounter;
    public TextMeshProUGUI enemyCounter;
    public GameObject playerInputManager;
    public bool lostHealth = false;

    [Header("Pause Menu")]
    [SerializeField] public bool isGamePaused = false;
    [SerializeField] private Button restartEncounterButton;
    //[SerializeField] private GameObject pauseMenu;


    [Header("Encounter")]
    public EncounterCreator currentEncounter;
    public bool encounterRunning = false;
    public bool winState = false;
    public bool failState = false;
    public bool tutorialRunning = false;

    [Header("Dialogue")]
    public float textSpeed = 0.05f;

    [Header("Tutorial")]
    public DynamicSongCreator tutorialSong;

    [Header("Conductor")]
    public float audioOffset;
    public float inputOffset;

    [Header("Level Buttons")]
    [SerializeField] private GameObject levelButtons;
    [SerializeField] private Transform levelParent;
    [SerializeField] private ScrollView levelScrollView;
    [SerializeField] private int viewPortOffset = -150;
    private ItemButtonEvent _eventItemOnSelect;
    private ItemButtonEvent _eventItemOnSubmit;
    [SerializeField] private Selectable returnToMainMenuButton;
    public GameObject currentSelectedButton;

    [Header("Info Panel Connections")]
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

    [Header("Tower Loadout")]
    public List<TowerPlacementInfo> towers = new List<TowerPlacementInfo>();

    [Header("Recording Assets")]
    public Sprite recordingSpr;//RECORDING STATUS CODE
    public List<Sprite> repeatSprites = new List<Sprite>();

    [Header("Level Scoring")]
    public List<int> pointHolder = new List<int>();
    public int healthRemainingPointGain = 100;


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

        _currentHealth = _maxHealth;

        QualitySettings.maxQueuedFrames = 1;

        //Debug.Log(QualitySettings.maxQueuedFrames + " frame");
        //Debug.Log(QualitySettings.vSyncCount + " Vsync");
        Cursor.lockState = CursorLockMode.Locked;
        playerInputManager.SetActive(false);

        LoadLevelButtons();
        UpdateAllLevelSelectButtonNavigationReferences();
    }

    // Update is called once per frame
    void Update()
    {
        //Manages health only while combat is running
        if (combatRunning)
        {
            Health();
        }

    }

    #region level buttons
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
        //gameObject.GetComponent<Button>().onClick.AddListener(LoadLevel(encounter));
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

        LoadLevel(item.heldEncounter); //load encounter
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

    #region pause function
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
        }

        isGamePaused = false;
        pauseMenuRoot.SetActive(false);
        exitMenuRoot.SetActive(false);
        settings.SetActive(false);
        MenuEventManager.Instance.PauseMenuClose();
        Time.timeScale = 1;
    }
    #endregion

    #region tower defense health/damage
    public void Damage()
    {
        _currentHealth -= 1;
        
        ComboManager.Instance.ResetCombo();

        lostHealth = true; //set flag for failing objective 02
    }

    void Health()
    {
        healthSlider.maxValue = _maxHealth;
        healthSlider.value = _currentHealth;
    }
    #endregion

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

        StageManager.Instance.SetStage(currentEncounter.stage);


        ConductorV2.instance.CountUsIn(currentEncounter.combatEncounter.dynamicSong.bpm);
    }

    /*
    public void LoadLevel(EncounterCreator encounter)
    {
        currentEncounter = encounter;
        encounterRunning = true;

        //tutorialRunning = encounter.isTutorial;

        LoadCombatScene(); 

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

    public void LoadLevel(EncounterCreator encounter)
    {
        currentEncounter = encounter;
        encounterRunning = true;
        
        StartCoroutine(LoadingScreenManager.Instance.StartLoading());

        ResetCombatState();
    }

    public void LoadCombatScene()
    {
        combatRoot.SetActive(true); //enable combat scene
        StageManager.Instance.SetStage(currentEncounter.stage); //load encounter grid data
        CombatManager.Instance.combatInterface.SetActive(false); //disable combat UI
    }
    
    public void StartCombat()
    {
        combatRunning = true; //set game state to combat
        CombatManager.Instance.tutorialManager.SetActive(false); //disable tutorial manager
        CombatManager.Instance.LoadEncounter(currentEncounter.combatEncounter); //load encounter data
    }

    public void GameOver()
    {
        if (failState) return;
        failState = true;

        Cursor.lockState = CursorLockMode.Locked; //lock player cursor movement

        CombatManager.Instance.EndEncounter(); //end current encounter

        MenuEventManager.Instance.OpenFailScreen(); //open fail screen and set active object
    }

    public void WinLevel()
    {
        if (winState) return;
        winState = true;
        
        //CombatManager.Instance.EndEncounter();
        encounterRunning = false;

        // OBJECTIVES
        // level cleared
        currentEncounter.clearedObjective01 = true;

        // level cleared without losing health
        //currentEncounter.clearedObjective02 = (_currentHealth == _maxHealth);

        if (lostHealth == false) {
            currentEncounter.clearedObjective02 = true;
        }

        //check if unique level objective was cleared
        //currentEncounter.clearedObjective03 = LevelObjectiveManager.Instance.CheckIfObjectiveWasCompleted(currentEncounter.data.uniqueLevel3Objective);

        // SCORE
        pointHolder.Add(healthRemainingPointGain * _currentHealth);

        UnlockLevel(currentEncounter); //unlock next level
        if (currentEncounter.endDialogue == null)
        {
            StartWinLevelProcess();
            return;
        }

        dialogueRoot.SetActive(true);
        DialogueManager.Instance.LoadDialogue(currentEncounter.endDialogue);
        //conductor.SetActive(false);
        //MenuEventManager.Instance.OpenWinScreen(0);
        ConductorV2.instance.StopMusic();
    }

    public void StartWinLevelProcess()
    {
        MenuEventManager.Instance.OpenWinScreen(); //open win screen and set active object
        

        // TEMPORARILY DISABLED UNTIL NEW WIN SCREEN IS CONNECTED
        //winScreen.GetComponent<ResultScreenInfo>().WriteToResultScreen(true, currentEncounter.encounterName, ComboManager.Instance.score, ComboManager.Instance.highestCombo + ComboManager.Instance.score, _currentHealth == _maxHealth, false);
    }

    public void UnlockLevel(EncounterCreator wonLevel)
    {
        if (wonLevel.levelThatUnlocks != null)
        {
            currentSelectedButton.GetComponent<ItemButton>().nextButton.SetActive(true);
            wonLevel.levelThatUnlocks.isLevelLocked = false;
        }
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

    public void ResetCombatState()
    {
        // reset win & fail states
        winState = false;
        winScreen.SetActive(false);
        failState = false;
        failScreen.SetActive(false);

        // reset objective flags
        lostHealth = false;

        // reset player health
        _currentHealth = _maxHealth;

        /*
        // reset combo, multiplier, & score
        ComboManager.Instance.currentCombo = 0;
        ComboManager.Instance.highestCombo = 0;
        ComboManager.Instance.currentMultiplier = 1;
        ComboManager.Instance.score = 0;
        
        // set flags for all objects having been spawned back to false
        CombatManager.Instance.allEnemiesSpawned = false;
        CombatManager.Instance.allPickupsSpawned = false;

        // set enemy & pickup totals back to 0
        CombatManager.Instance.enemiesDefeated = 0;
        CombatManager.Instance.enemyTotal = 0;
        CombatManager.Instance.pickupTotal = 0;
        */
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

}

public enum UpgradeNum
{
    One, Two, Three
}
