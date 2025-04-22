using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

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
    }
    #endregion

    public Transform globelParent;
    public Transform projectileParent;
    public AudioSource menuMusic;

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
        //Cursor.lockState = CursorLockMode.None;
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
        _currentHealth = _maxHealth;
        menuMusic.Stop();
        combatRoot.SetActive(true);
        CombatManager.Instance.allEnemiesSpawned = false;
        CombatManager.Instance.tutorialManager.SetActive(true);
        EnemySpawner.Instance.allEnemiesSpawned = false;
        tutorialRunning = true;
        winState = false;
        winScreen.SetActive(false);
        loseState = false;
        gameOverScreen.SetActive(false);
        playerInputManager.SetActive(true);

        CombatManager.Instance.enemyTimerObject.SetActive(false);
        CombatManager.Instance.healthBar.SetActive(false);
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
        TowerManager.Instance.SetupResourceBars();




        CombatManager.Instance.enemyTotal = 7;


        List<Wave> empty = new List<Wave>();
        EnemySpawner.Instance.currentWaves = empty;
        EnemySpawner.Instance.numberOfEnemiesToSpawn = 7;
        EnemySpawner.Instance.startOnce = false;
        EnemySpawner.Instance.currentNumberOfEnemiesSpawned = 0;

        EnemySpawner.Instance.currentNumberOfEnemiesSpawned = 0;


        TutorialManager.Instance.LoadTutorial();

        ConductorV2.instance.CountUsIn(currentEncounter.combatEncounter.dynamicSong.bpm);
    }

    public void LoadEncounter(EncounterCreator encounter)
    {
        tutorialRunning = encounter.isTutorial;

        

        currentEncounter = encounter;
        encounterRunning = true;
        winState = false;
        winScreen.SetActive(false);
        loseState = false;
        gameOverScreen.SetActive(false);

        if (currentEncounter.introDialogue == null)
        {
            combatRoot.SetActive(true);
            CombatManager.Instance.tutorialManager.SetActive(false);
            combatRunning = true;
            CombatManager.Instance.LoadEncounter(currentEncounter.combatEncounter);
            return;
        }
        dialogueRoot.SetActive(true);
        DialogueManager.Instance.LoadDialogue(currentEncounter.introDialogue);
    }

    public void GameOver()
    {
        if (loseState) return;
        loseState = true;
        //Cursor.lockState = CursorLockMode.None;
        CombatManager.Instance.EndEncounter();
        gameOverScreen.SetActive(true);
        MenuEventManager.Instance.LoseScreenOpen();
        //conductor.SetActive(false);
        //ConductorV2.instance.StopMusic();
    }

    public void WinLevel()
    {
        if (winState) return;
        winState = true;
        CombatManager.Instance.EndEncounter();
        encounterRunning = false;
        ConductorV2.instance.StopMusic();


        if (currentEncounter.endDialogue == null)
        {
            winScreen.SetActive(true);
            MenuEventManager.Instance.WinScreenOpen();
            return;
        }

        dialogueRoot.SetActive(true);
        DialogueManager.Instance.LoadDialogue(currentEncounter.endDialogue);
        //conductor.SetActive(false);
        //MenuEventManager.Instance.WinScreenOpen();
        ConductorV2.instance.StopMusic();
    }

    public void TutorialWinState()
    {
        if (winState) return;
        winState = true;

        CombatManager.Instance.tutorialManager.SetActive(false);

        CombatManager.Instance.EndEncounter();
        encounterRunning = false;
        //Cursor.lockState = CursorLockMode.None;

        //winScreen.SetActive(true);
        //conductor.SetActive(false);
        //MenuEventManager.Instance.WinScreenOpen();
        dialogueRoot.SetActive(true);
        DialogueManager.Instance.LoadDialogue(currentEncounter.endDialogue);
        ConductorV2.instance.StopMusic();
    }

    public void ResetGameManagerVariables()
    {

    }

}
