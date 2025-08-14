using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Android.Types;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    #region dont touch this
    private static CombatManager _instance;
    public static CombatManager Instance
    {
        get
        {
            if (_instance is null)
            {
                Debug.LogError("CombatManager is NULL");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }
    #endregion

    public bool spawnerDelayRunning = false;
    public bool allEnemiesSpawned = false;
    public bool allPickupsSpawned = false;
    public int enemiesDefeated = 0;
    public int enemyTotal = 0;
    public int pickupTotal = 0;
    [SerializeField] private Spawner objectSpawners;

    public CombatMaker currentEncounter;

    [SerializeField] public Transform enemiesParent;
    [SerializeField] public Transform towersParent;
    [SerializeField] public Transform projectilesParent;
    [SerializeField] public Transform chargesParent;
    [SerializeField] public Transform pickupsParent;

    public TextMeshProUGUI enemiesSpawnIn;
    public int enemyTimerMax = 30;
    public int enemyTimer = 40;
    bool switchColor = false;

    [Header("Round Info")]
    public int totalNumEnemies;
    public int totalNumPickups;

    [Header("Resources")]
    public int resourceNum;
    public int maxResource = 100;
    public Slider resourceSlider1;
    public Slider resourceSlider2;
    public Slider resourceSlider3;
    public Slider resourceSlider4;
    public int startingResources;

    [Header("Overcharge Resources")]
    public Slider overchargeSlider;
    public bool canPlaceEmpoweredTower = false;


    [Header("Combat UI")]
    public GameObject enemyTimerObject;
    public GameObject healthBar;
    //public GameObject controls;
    public GameObject resources;
    public GameObject overchargeResources;
    public GameObject towerDisplay;
    public GameObject feverBar;
    public GameObject metronome;
    public GameObject waveCounter;
    public GameObject combo;
    //public GameObject knockEmDead;

    public GameObject tutorialManager;

    // Start is called before the first frame update
    void Start()
    {
        //LoadEncounter(currentEncounter);
    }


    public void RestartEncounter()
    {
        GameManager.Instance.winScreen.SetActive(false);
        GameManager.Instance.winState = false;
        GameManager.Instance.gameOverScreen.SetActive(false);
        GameManager.Instance.loseState = false;

        if (GameManager.Instance.tutorialRunning || GameManager.Instance.currentEncounter.isShowcase)
        {
            EndEncounter();
            RestartTutorialEncounter();
            return;
        }

        EndEncounter();
        LoadEncounter(currentEncounter);
    }

    public void RestartTutorialEncounter()
    {
        GameManager.Instance.LoadTutorial();
        //remove enemies
        foreach (Transform child in enemiesParent)
        {
            child.gameObject.transform.DOKill();
            child.gameObject.GetComponent<Enemy>().RemoveEnemy();
        }
        //remove towers
        foreach (Transform child in towersParent)
        {
            child.gameObject.GetComponent<Tower>().RemoveTower();
        }
        //remove projectiles
        foreach (Transform child in projectilesParent)
        {
            child.gameObject.transform.DOKill();
            child.gameObject.GetComponent<Projectile>().RemoveProjectile();
        }
        //remove pickups
        foreach (Transform child in pickupsParent)
        {
            child.gameObject.GetComponent<Pickup>().RemovePickup();
        }

        objectSpawners.startOnce = false; // stop spawning

        CursorTD.Instance.isMoving = false;
        Cursor.lockState = CursorLockMode.Locked;

        ConductorV2.instance.drums.volume = 0;
        ConductorV2.instance.bass.volume = 0;
        ConductorV2.instance.piano.volume = 0;
        ConductorV2.instance.guitarH.volume = 0;
        ConductorV2.instance.guitarM.volume = 0;

        FeverSystem.Instance.feverBarNum = 0;
        ComboManager.Instance.ResetCombo();
        ComboManager.Instance.highestCombo = 0;

        //ConductorV2.instance.StopMusic();

        
    }

    //play this when loading up an encounter
    public void LoadEncounter(CombatMaker encounter)
    {
        if (GameManager.Instance.tutorialRunning)
        {
            GameManager.Instance.combatRunning = true;
        }
        GameManager.Instance.playerInputManager.SetActive(true);
        GameManager.Instance.menuMusic.Stop();
        GameManager.Instance.winScreen.SetActive(false);
        GameManager.Instance.winState = false;
        GameManager.Instance.gameOverScreen.SetActive(false);
        GameManager.Instance.loseState = false;
        GameManager.Instance._currentHealth = GameManager.Instance._maxHealth;

        currentEncounter = encounter;

        // set the flags for all objects having been spawned back to false
        allEnemiesSpawned = false;
        allPickupsSpawned = false;

        // set enemy & pickup totals back to 0 temporarily before counting
        enemiesDefeated = 0;
        enemyTotal = 0;
        pickupTotal = 0;

        // for each wave, get the total number of enemies & pickups and add them to the enemy & pickup totalsfor this encounter
        foreach (var item in currentEncounter.waves)
        {
            enemyTotal += item.enemies.Count;
            pickupTotal += item.pickups.Count;
        }

        // set the total number of enemies & pickups to the counted totals
        totalNumEnemies = enemyTotal;
        totalNumPickups = pickupTotal;
        
        // set the total number of objects to spawn to the object totals
        objectSpawners.numberOfEnemiesToSpawn = enemyTotal;
        objectSpawners.numberOfPickupsToSpawn = pickupTotal;

        // set objects to not spawn
        objectSpawners.startOnce = false;

        // set the current total number of objects spawned to 0
        objectSpawners.currentNumberOfEnemiesSpawned = 0;
        objectSpawners.currentNumberOfPickupsSpawned = 0;

        // set the list of waves in the spawner to those from the current encounter
        objectSpawners.currentWaves = currentEncounter.waves;

        objectSpawners.ForecastWave(0);


        resourceNum = startingResources;
        enemyTimer = enemyTimerMax;
        enemiesSpawnIn.gameObject.SetActive(true);


        CursorTD.Instance.InitializeCursor();

        BeatIndicatorManager.Instance.ResetBeatIndicator();

        TowerManager.Instance.SetupResourceBars();

        Cursor.lockState = CursorLockMode.Locked;

        if (GameManager.Instance.tutorialRunning)
            return;
        TowerManager.Instance.ResetTowerManager();
        ConductorV2.instance.CountUsIn(currentEncounter.dynamicSong.bpm);
    }

    public void EndEncounter()
    {
        //remove enemies
        foreach (Transform child in enemiesParent)
        {
            child.gameObject.GetComponent<Enemy>().RemoveEnemy();
        }
        //remove towers
        foreach (Transform child in towersParent)
        {
            child.gameObject.GetComponent<Tower>().RemoveTower();
        }
        //remove Projectiles
        foreach (Transform child in projectilesParent)
        {
            child.gameObject.GetComponent<Projectile>().RemoveProjectile();
        }
        //remove Projectiles
        foreach (Transform child in chargesParent)
        {
            child.gameObject.GetComponent<Charges>().RemoveCharge();
        }
        //remove pickups
        foreach (Transform child in pickupsParent)
        {
            child.gameObject.GetComponent<Pickup>().RemovePickup();
        }
        
        // reset spawner activity
        objectSpawners.startOnce = false;
        objectSpawners.ResetSpawner();

        CursorTD.Instance.pauseMovement = true;
        CursorTD.Instance.isMoving = false;
        Cursor.lockState = CursorLockMode.Locked;

        BeatIndicatorManager.Instance.ResetBeatIndicator();

        GameManager.Instance.menuMusic.Play();
        GameManager.Instance.playerInputManager.SetActive(false);
        ConductorV2.instance.drums.volume = 0;
        ConductorV2.instance.bass.volume = 0;
        ConductorV2.instance.piano.volume = 0;
        ConductorV2.instance.guitarH.volume = 0;
        ConductorV2.instance.guitarM.volume = 0;

        FeverSystem.Instance.feverBarNum = 0;
        ComboManager.Instance.ResetCombo();
        ComboManager.Instance.highestCombo = 0;

        ConductorV2.instance.StopMusic();
        GameManager.Instance.tutorialRunning = false;

        CombatDialogueManager.Instance.combatDialogueActive = false;
        CombatDialogueManager.Instance.Clear();
        CombatDialogueManager.Instance.dialogueBox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        TowerManager.Instance.TowerCost();


        ResourceBar();

        //checks if not all enemies have been spawned
        if (!objectSpawners.allEnemiesSpawned)
        {
            allEnemiesSpawned = false;
        }
        else
        {
            allEnemiesSpawned = true;
        }

        //checks if not all pickups have been spawned
        if (!objectSpawners.allPickupsSpawned)
        {
            allPickupsSpawned = false; 
        }
        else
        {
            allPickupsSpawned = true;
        }
        
        //enemies defeated text
        GameManager.Instance.enemyCounter.text = $"{enemiesDefeated}/{totalNumEnemies}";


        //delays enemy spawning
        DelayTimer();

        if (GameManager.Instance.tutorialRunning)
        {
            overchargeResources.SetActive(false);
            resourceNum = Mathf.Clamp(resourceNum, 0, 100);
            return;

        }


        overchargeSlider.value = resourceNum - 100;

        if (resourceNum > 100)
        {
            overchargeResources.SetActive(true);
        }
        else
        {
            overchargeResources.SetActive(false);
        }

        if (resourceNum == 150) canPlaceEmpoweredTower = true;
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance._currentHealth <= 0)
        {
            GameManager.Instance.GameOver();
        }

        else if(GameManager.Instance._currentHealth != 0 && allEnemiesSpawned && enemyTotal <= 0 && !GameManager.Instance.currentEncounter.isBossBattle)
        {
            if (GameManager.Instance._currentHealth <= 0)
                return;

            GameManager.Instance.WinLevel();
        } //checks if all enemies have died or player health hasnt reached zero to give a win state

    }

    void ResourceBar()
    {
        //resource stuff
        resourceNum = Mathf.Clamp(resourceNum, 0, maxResource);
        resourceSlider1.value = resourceNum;
        resourceSlider2.value = resourceNum - 25;
        resourceSlider3.value = resourceNum - 50;
        resourceSlider4.value = resourceNum - 75;
    }

    void DelayTimer()
    {
        if (GameManager.Instance.tutorialRunning)
            return;
        if (enemyTimer <= 0)
        {
            enemiesSpawnIn.gameObject.SetActive(false);

            objectSpawners.StartSpawningEnemies();
            return;
        }

        //Spawner.Instance.ForecastWave(0);//forecast the first wave

        enemiesSpawnIn.text = "Enemies Spawn in " + enemyTimer;
        //Start spawning enemies on the 10th bar
        

    }
    public void BeatCountdown()
    {
        enemyTimer -= 1;
        if (switchColor)
            enemiesSpawnIn.color = Color.red;
        else
            enemiesSpawnIn.color = Color.blue;
        switchColor = !switchColor;
    }

    public void GenerateResource()
    {
        if (GameManager.Instance.tutorialRunning && CursorTD.Instance.movementSequence)
            return;

        if (GameManager.Instance.tutorialRunning && resourceNum >= 25 && !CursorTD.Instance.towerPlaceSequence && !CursorTD.Instance.towerBuffSequence && !CursorTD.Instance.feverModeSequence && !CursorTD.Instance.towerPlacementMenuSequencePassed)
        {
            // Make sure index is set to whichever text says "Moving on-beat gives magic"
            if (TutorialManager.Instance.index == 4)
                TutorialManager.Instance.LoadNextTutorialDialogue();
            CursorTD.Instance.towerPlacementMenuSequence = true;
            return;
        }
        resourceNum += 1;
        return;
    }
     
}
