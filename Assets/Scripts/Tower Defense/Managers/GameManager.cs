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

    public Transform globelParent;
    public AudioSource menuMusic;
    public AudioSource buttonHighlightSFX;

    [SerializeField] public GameObject gameOverScreen;
    [SerializeField] public GameObject winScreen;
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

    [Header("Pause Menu")]
    [SerializeField] public bool isGamePaused = false;
    [SerializeField] private Button restartEncounterButton;
    //[SerializeField] private GameObject pauseMenu;


    [Header("Encounter")]
    public EncounterCreator currentEncounter;
    public bool encounterRunning = false;
    public bool winState = false;
    public bool loseState = false;
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

        Debug.Log(QualitySettings.maxQueuedFrames + " frame");
        Debug.Log(QualitySettings.vSyncCount + " Vsync");
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
        buttonHighlightSFX.Play(); //play button feedback sfx
        MenuEventManager.Instance.UpdateLastSelectedLevel();
        MenuEventManager.Instance.CloseMainMenu(); //stop main menu music, update last selected level, disable main menu
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

        ItemButton item;
        Navigation navigation;

        for (int i = 0; i < children.Length; i++)
        {
            item = children[i];

            navigation = item.gameObject.GetComponent<Button>().navigation;

            navigation.selectOnLeft = GetNavigationLeft(i, children.Length);
            navigation.selectOnRight = GetNavigationRight(i, children.Length);
            navigation.selectOnUp = returnToMainMenuButton;

            item.gameObject.GetComponent<Button>().navigation = navigation;

            //level locking functions
            item.heldEncounter.levelThatUnlocks = GetNextEncounter(i, children.Length);

            if(item.heldEncounter.isLevelLocked)
            {
                item.gameObject.SetActive(false);
            }
        }
    }

    private Selectable GetNavigationRight(int indexCurrent, int length)
    {
        ItemButton item;

        if (indexCurrent == length - 1) //last item
        {
            //looping dont set anything here
            return null;
        }
        else
        {
            item = levelParent.GetChild(indexCurrent + 1).GetComponent<ItemButton>();
        }

        return item.GetComponent<Selectable>();
    }

    private Selectable GetNavigationLeft(int indexCurrent, int length)
    {
        ItemButton item;

        if (indexCurrent == 0)
        {
            //looping dont set anything here
            return null;
        }
        else
        {
            item = levelParent.GetChild(indexCurrent - 1).GetComponent<ItemButton>();
        }

        return item.GetComponent<Selectable>();
    }

    private EncounterCreator GetNextEncounter(int indexCurrent, int length)
    {
        ItemButton item;

        if (indexCurrent == length - 1) //last item
        {
            //looping dont set anything here
            return null;
        }
        else
        {
            item = levelParent.GetChild(indexCurrent + 1).GetComponent<ItemButton>();
        }

        return item.heldEncounter;
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
        loseState = false;
        gameOverScreen.SetActive(false);
        playerInputManager.SetActive(true);

        CombatManager.Instance.enemyTimerObject.SetActive(false);
        //CombatManager.Instance.healthBar.SetActive(false);
        CombatManager.Instance.resources.SetActive(false);
        CombatManager.Instance.towerDisplay.SetActive(false);
        CombatManager.Instance.feverBar.SetActive(false);
        CombatManager.Instance.metronome.SetActive(false);
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

    public void LoadLevel(EncounterCreator encounter)
    {
        currentEncounter = encounter;

        tutorialRunning = encounter.isTutorial;

        encounterRunning = true;

        winState = false;
        winScreen.SetActive(false);
        loseState = false;
        gameOverScreen.SetActive(false);

        if (currentEncounter.introDialogue == null)
        {

            LoadCombat();
            CombatManager.Instance.enemyTimerObject.SetActive(true);
            CombatManager.Instance.healthBar.SetActive(true);
            //CombatManager.Instance.controls.SetActive(true);
            CombatManager.Instance.resources.SetActive(true);
            CombatManager.Instance.towerDisplay.SetActive(true);
            CombatManager.Instance.feverBar.SetActive(true);
            CombatManager.Instance.metronome.SetActive(true);
            CombatManager.Instance.waveCounter.SetActive(true);
            CombatManager.Instance.combo.SetActive(true);

            return;
        }

        LoadingScreenManager.Instance.StartLoading(dialogueRoot, menuRoot);
        //dialogueRoot.SetActive(true);
        DialogueManager.Instance.LoadDialogue(currentEncounter.introDialogue);
    }

    public void LoadCombat()
    {
        combatRoot.SetActive(true); //enable combat scene

        CombatManager.Instance.tutorialManager.SetActive(false); //disable tutorial manager

        combatRunning = true; //set game state to combat

        StageManager.Instance.SetStage(currentEncounter.stage); //load encounter grid data

        CombatManager.Instance.LoadEncounter(currentEncounter.combatEncounter); //load encounter data

        //reset camera position, rotation, & FOV
        Camera.main.transform.position = Camera.main.GetComponent<CameraPositions>().CombatCameraPos;
        Camera.main.transform.rotation = Quaternion.Euler(new Vector3(60,0,0));
        Camera.main.fieldOfView = 60;
    }

    public void GameOver()
    {
        if (loseState) return;
        loseState = true;

        Cursor.lockState = CursorLockMode.Locked; //lock player cursor movement

        CombatManager.Instance.EndEncounter(); //end current encounter

        MenuEventManager.Instance.OpenFailScreen(); //open fail screen and set active object
    }

    public void WinLevel()
    {
        if (winState) return;
        winState = true;

        CombatManager.Instance.EndEncounter();
        encounterRunning = false;

        ConductorV2.instance.StopMusic();
        pointHolder.Add(healthRemainingPointGain * _currentHealth);

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
        
        UnlockLevel(currentEncounter); //unlock next level

        // TEMPORARILY DISABLED UNTIL NEW WIN SCREEN IS CONNECTED
        //winScreen.GetComponent<ResultScreenInfo>().WriteToResultScreen(true, currentEncounter.encounterName, ComboManager.Instance.score, ComboManager.Instance.highestCombo + ComboManager.Instance.score, _currentHealth == _maxHealth, false);
    }

    public void UnlockLevel(EncounterCreator wonLevel)
    {
        if (wonLevel.levelThatUnlocks != null)
        {
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

    public void ResetGameManagerVariables()
    {

    }

}
