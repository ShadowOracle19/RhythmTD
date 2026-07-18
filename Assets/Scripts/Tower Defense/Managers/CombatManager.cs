using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
//using UnityEditor.SceneManagement;
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

    #region Variables
    [Header("<b><size=15>Encounter<b><size=15>")]
    [Line(255,255,255)]
    public CombatMaker currentEncounter;

    [Space(20)][Header("<b><size=15>Spawning<b><size=15>")]
    [Line(255,255,255)]
    [SerializeField] private Spawner objectSpawners;
    public bool spawnerDelayRunning = false;
    [Space(10)]
    public bool allEnemiesSpawned = false;
    public bool allPickupsSpawned = false;
    public int enemiesDefeated = 0;
    public int enemyTotal = 0;
    public int pickupTotal = 0;
    public int totalNumEnemies;
    public int totalNumPickups;
    [Space(10)]
    [SerializeField] public Transform enemiesParent;
    [SerializeField] public Transform towersParent;
    [SerializeField] public Transform projectilesParent;
    [SerializeField] public Transform chargesParent;
    [SerializeField] public Transform pickupsParent;
    [SerializeField] public Transform stageParent;
    [Space(10)]
    public TextMeshProUGUI enemiesSpawnIn;
    public int enemyTimerMax = 30;
    public int enemyTimer = 40;
    bool switchColor = false;

    [Space(20)][Header("<b><size=15>Resources<b><size=15>")]
    [Line(255,255,255)]
    public bool overrideStartingResources = false;
    public int startingResourcesOverride = 100;
    [Space(10)]
    public int resourceNum = 0;
    public int resourceCap = 999;
    public int resourceCapMax = 999;
    public int resourceCapLimited = 150;
    

    [Space(20)][Header("<b><size=15>UI<b><size=15>")]
    [Line(255,255,255)]
    public GameObject combatInterface;
    public GameObject countInObject;
    public GameObject enemyTimerObject;
    public GameObject healthBar;
    public GameObject resources;
    public GameObject infinityIcon;
    public GameObject resourceText;
    public TextMeshProUGUI resourceNumText;
    public GameObject feverBar;
    public GameObject waveCounter;
    public GameObject combo;
    [Space(10)]
    public GameObject tutorialManager;

    [Space(20)][Header("<b><size=15>Stage End Sequence<b><size=15>")]
    [Line(255,255,255)]
    public bool fadeMusicFinished = false;
    public float fadeDurationInBeats = 8.0f;
    #endregion

    #region Start
    // Start is called before the first frame update
    void Start()
    {
        
    }
    #endregion

    #region OnDisable
    private void OnDisable()
    {
        
    }
    #endregion

    #region Update
    // Update is called once per frame
    void Update()
    {
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
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance._currentHealth <= 0)
        {
            GameManager.Instance.FailLevel();
        }

        else if(GameManager.Instance._currentHealth != 0 && allEnemiesSpawned && enemyTotal <= 0 && !GameManager.Instance.currentEncounter.isBossBattle)
        {
            if (GameManager.Instance._currentHealth <= 0)
                return;

            GameManager.Instance.WinLevel();
        } //checks if all enemies have died or player health hasnt reached zero to give a win state

    }
    #endregion

    #region Restart
    public void RestartEncounter()
    {
        // close win & fail screens
        GameManager.Instance.winScreen.SetActive(false);
        GameManager.Instance.winState = false;
        GameManager.Instance.failScreen.SetActive(false);
        GameManager.Instance.failState = false;

        GameManager.Instance.hasLostHealth = false; //reset flag for failing objective 02

        // restart encounter differently for tutorials & showcases
        if (GameManager.Instance.tutorialRunning || GameManager.Instance.currentEncounter.isShowcase)
        {
            EndEncounter();

            StageManager.Instance.SetStage(GameManager.Instance.currentEncounter.stage);

            RestartTutorialEncounter();

            return;
        }

        EndEncounter();

        StageManager.Instance.SetStage(GameManager.Instance.currentEncounter.stage); // rebuild stage

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

        ConductorV2.instance.flat.volume = 0;
        ConductorV2.instance.major.volume = 0;
        ConductorV2.instance.allegro.volume = 0;
        ConductorV2.instance.trill.volume = 0;
        ConductorV2.instance.chromatic.volume = 0;
        ConductorV2.instance.poco.volume = 0;
        ConductorV2.instance.forte.volume = 0;
        ConductorV2.instance.legato.volume = 0;
        ConductorV2.instance.Tower9.volume = 0;
        ConductorV2.instance.Tower10.volume = 0;
        ConductorV2.instance.Tower11.volume = 0;
        ConductorV2.instance.Tower12.volume = 0;

        FeverSystem.Instance.feverBarNum = 0;
        ScoreManager.Instance.ResetCombo();
        ScoreManager.Instance.highestCombo = 0;

        //ConductorV2.instance.StopMusic(); 
    }
    #endregion

    #region Start level
    //
    public void SpawnStagePlatform(CombatMaker encounter)
    {
        var stage = Instantiate(encounter.stagePrefab, stageParent);

        //add spawn and pickup tile call to spawner
        Spawner.Instance.spawnTiles = stage.GetComponent<StageObject>().spawnTiles;
        Spawner.Instance.pickupSpawnTiles = stage.GetComponent<StageObject>().pickupTiles;
    }

    //play this when loading up an encounter
    public void LoadEncounter(CombatMaker encounter)
    {
        currentEncounter = encounter;
        
        /*
        if (GameManager.Instance.tutorialRunning)
        {
            GameManager.Instance.combatRunning = true;
        }
        */

        GameManager.Instance.menuMusic.Stop();

        GameManager.Instance.playerInputManager.SetActive(true);

        GameManager.Instance.ResetCombatState();

        // for each wave, get the total number of enemies & pickups and add them to the enemy & pickup totals for this encounter
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

        // set starting resources
        if (GameManager.Instance.isInfiniteResources) {
            infinityIcon.SetActive(true);
            resourceText.SetActive(false);
        }
        else {
            infinityIcon.SetActive(false);
            resourceText.SetActive(true);
        }

        if (GameManager.Instance.isLimitedResources) {
            resourceCap = resourceCapLimited;
        }
        else {
            resourceCap = resourceCapMax;
        }
        
        if (overrideStartingResources) {
            resourceNum = startingResourcesOverride;
        }
        else if (GameManager.Instance.isInfiniteResources) {
            resourceNum = resourceCap;
        }
        else {
            resourceNum = currentEncounter.startingResources;
        }

        enemyTimer = enemyTimerMax; // reset enemy spawn countdown timer
        enemiesSpawnIn.gameObject.SetActive(true); // enable enemy spawn countdown text object

        CursorTD.Instance.InitializeCursor(); // initialize the player cursor
        Cursor.lockState = CursorLockMode.Locked;

        /*
        if (GameManager.Instance.tutorialRunning)
            return;
        */
        
        GameManager.Instance.conductor.SetActive(true); //

        //Set Animation BPM
        //Debug.Log(ConductorV2.instance.bpm);
        AnimationManager.instance.SetCombatAnimSpeed();

        // start count in
        ConductorV2.instance.CountUsIn(currentEncounter.dynamicSong.bpm);
    }
    #endregion

    #region End level
    public void StartFailSequence()
    {
        // fade music out
        StartCoroutine(FadeTracks(0.0f,fadeDurationInBeats));
        StartCoroutine(WaitForFailSequenceEnd());

        MenuEventManager.Instance.combatInterfaceAnimator.SetTrigger("Combat End");
        
        ClearEncounter();
    }
    
    public void StartWinSequence()
    {
        // fade music out
        StartCoroutine(FadeTracks(0.0f,fadeDurationInBeats));
        StartCoroutine(WaitForWinSequenceEnd());

        MenuEventManager.Instance.combatInterfaceAnimator.SetTrigger("Combat End");
        
        ClearEncounter();
    }

    public IEnumerator WaitForFailSequenceEnd()
    {
        while (fadeMusicFinished == false)
        {
            yield return null;
        }
        
        ConductorV2.instance.StopMusic();
        GameManager.Instance.conductor.SetActive(false);

        MenuEventManager.Instance.OpenFailScreen();
        
        StopCoroutine(WaitForFailSequenceEnd());
    }

    public IEnumerator WaitForWinSequenceEnd()
    {
        while (fadeMusicFinished == false)
        {
            yield return null;
        }
        
        ConductorV2.instance.StopMusic();
        GameManager.Instance.conductor.SetActive(false);

        GameManager.Instance.UnlockLevel(GameManager.Instance.currentEncounter); 
        if (GameManager.Instance.currentEncounter.endDialogue == null)
        {
            MenuEventManager.Instance.OpenWinScreen();
        }
        else
        {
            GameManager.Instance.dialogueRoot.SetActive(true);
            DialogueManager.Instance.LoadDialogue(GameManager.Instance.currentEncounter.endDialogue);
        }
        
        StopCoroutine(WaitForWinSequenceEnd());
    }

    public void EndEncounter()
    {
        // lock player movement
        CursorTD.Instance.pauseMovement = true;
        CursorTD.Instance.isMoving = false;
        Cursor.lockState = CursorLockMode.Locked;

        // remove enemies
        foreach (Transform child in enemiesParent)
        {
            child.gameObject.GetComponent<Enemy>().RemoveEnemy();
        }
        // remove towers
        foreach (Transform child in towersParent)
        {
            child.gameObject.GetComponent<Tower>().RemoveTower();
        }
        // remove projectiles
        foreach (Transform child in projectilesParent)
        {
            child.gameObject.GetComponent<Projectile>().RemoveProjectile();
        }
        // remove charges
        foreach (Transform child in chargesParent)
        {
            child.gameObject.GetComponent<Charges>().RemoveCharge();
        }
        // remove pickups
        foreach (Transform child in pickupsParent)
        {
            child.gameObject.GetComponent<Pickup>().RemovePickup();
        }
        
        // reset spawner activity
        objectSpawners.startOnce = false;
        objectSpawners.ResetSpawner();

        BeatIndicatorManager.Instance.ResetBeatIndicator();

        //GameManager.Instance.menuMusic.Play();
        GameManager.Instance.playerInputManager.SetActive(false);

        //GameManager.Instance.pointHolder.Clear();

        // reset fever bar, combo, and highest combo
        FeverSystem.Instance.feverBarNum = 0;
        ScoreManager.Instance.ResetCombo();
        ScoreManager.Instance.highestCombo = 0;

        ConductorV2.instance.StopMusic();

        GameManager.Instance.tutorialRunning = false; 
        GameManager.Instance.combatRunning = false;

        CombatDialogueManager.Instance.combatDialogueActive = false;
        CombatDialogueManager.Instance.Clear();
        CombatDialogueManager.Instance.dialogueBox.SetActive(false);

        // clear spawn tile list data
        Spawner.Instance.spawnTiles.Clear();
        Spawner.Instance.pickupSpawnTiles.Clear();

        // fade music out
        StartCoroutine(FadeTracks(0.0f, fadeDurationInBeats));

        //stageParent.GetComponentInChildren<StageObject>().DestroyStage(); // remove stage
    }
    #endregion

    #region Reset
    public void ClearEncounter() 
    {
        // lock player movement
        CursorTD.Instance.pauseMovement = true;
        CursorTD.Instance.isMoving = false;
        Cursor.lockState = CursorLockMode.Locked;
        
        // remove enemies
        foreach (Transform child in enemiesParent)
        {
            child.gameObject.GetComponent<Enemy>().RemoveEnemy();
        }
        // remove towers
        foreach (Transform child in towersParent)
        {
            child.gameObject.GetComponent<Tower>().RemoveTower();
        }
        // remove projectiles
        foreach (Transform child in projectilesParent)
        {
            child.gameObject.GetComponent<Projectile>().RemoveProjectile();
        }
        // remove charges
        foreach (Transform child in chargesParent)
        {
            child.gameObject.GetComponent<Charges>().RemoveCharge();
        }
        // remove pickups
        foreach (Transform child in pickupsParent)
        {
            child.gameObject.GetComponent<Pickup>().RemovePickup();
        }
        
        // reset spawner activity
        objectSpawners.startOnce = false;
        objectSpawners.ResetSpawner();

        BeatIndicatorManager.Instance.ResetBeatIndicator();

        //GameManager.Instance.menuMusic.Play();
        GameManager.Instance.playerInputManager.SetActive(false);
        //GameManager.Instance.pointHolder.Clear();

        // reset fever bar, combo, and highest combo
        FeverSystem.Instance.feverBarNum = 0;
        ScoreManager.Instance.ResetCombo();
        ScoreManager.Instance.highestCombo = 0;

        GameManager.Instance.tutorialRunning = false; 
        GameManager.Instance.combatRunning = false;

        CombatDialogueManager.Instance.combatDialogueActive = false;
        CombatDialogueManager.Instance.Clear();
        CombatDialogueManager.Instance.dialogueBox.SetActive(false);

        // clear spawn tile list data
        Spawner.Instance.spawnTiles.Clear();
        Spawner.Instance.pickupSpawnTiles.Clear();
    }
    #endregion
    
    #region Music controls
    public IEnumerator FadeTracks(float targetVolume, float durationInBeats)
    {
        fadeMusicFinished = false;

        float timeElapsed = 0.0f;
        float fadeDuration = (ConductorV2.instance.crotchet * durationInBeats);

        // track volume start points
        float trackVolume01 = ConductorV2.instance.flat.volume;
        float trackVolume02 = ConductorV2.instance.major.volume;
        float trackVolume03 = ConductorV2.instance.allegro.volume;
        float trackVolume04 = ConductorV2.instance.trill.volume;
        float trackVolume05 = ConductorV2.instance.chromatic.volume;
        float trackVolume06 = ConductorV2.instance.poco.volume;
        float trackVolume07 = ConductorV2.instance.forte.volume;  
        float trackVolume08 = ConductorV2.instance.legato.volume; 
        float trackVolume09 = ConductorV2.instance.Tower9.volume;
        float trackVolume10 = ConductorV2.instance.Tower10.volume;    
        float trackVolume11 = ConductorV2.instance.Tower11.volume;
        float trackVolume12 = ConductorV2.instance.Tower12.volume;

        while (timeElapsed < fadeDuration) //this should be the time across 4 beats
        {
            float t = timeElapsed / fadeDuration;

            /*
            foreach (float trackVolume in tracks)
            {
                trackVolume = Mathf.Lerp(trackInitialVolumes[index], 0, t)
            }
            */

            ConductorV2.instance.flat.volume = Mathf.Lerp(trackVolume01,targetVolume,t);
            ConductorV2.instance.major.volume = Mathf.Lerp(trackVolume02,targetVolume,t);
            ConductorV2.instance.allegro.volume = Mathf.Lerp(trackVolume03,targetVolume,t);
            ConductorV2.instance.trill.volume = Mathf.Lerp(trackVolume04,targetVolume,t);
            ConductorV2.instance.chromatic.volume = Mathf.Lerp(trackVolume05,targetVolume,t);
            ConductorV2.instance.poco.volume = Mathf.Lerp(trackVolume06,targetVolume,t);
            ConductorV2.instance.forte.volume = Mathf.Lerp(trackVolume07,targetVolume,t);  
            ConductorV2.instance.legato.volume = Mathf.Lerp(trackVolume08,targetVolume,t);
            ConductorV2.instance.Tower9.volume = Mathf.Lerp(trackVolume09,targetVolume,t);
            ConductorV2.instance.Tower10.volume = Mathf.Lerp(trackVolume10,targetVolume,t);    
            ConductorV2.instance.Tower11.volume = Mathf.Lerp(trackVolume11,targetVolume,t);
            ConductorV2.instance.Tower12.volume = Mathf.Lerp(trackVolume12,targetVolume,t);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // set all tracks to target volume
        ConductorV2.instance.flat.volume = targetVolume;
        ConductorV2.instance.major.volume = targetVolume;
        ConductorV2.instance.allegro.volume = targetVolume;
        ConductorV2.instance.trill.volume = targetVolume;
        ConductorV2.instance.chromatic.volume = targetVolume;
        ConductorV2.instance.poco.volume = targetVolume;
        ConductorV2.instance.forte.volume = targetVolume;  
        ConductorV2.instance.legato.volume = targetVolume; 
        ConductorV2.instance.Tower9.volume = targetVolume;
        ConductorV2.instance.Tower10.volume = targetVolume;    
        ConductorV2.instance.Tower11.volume = targetVolume;
        ConductorV2.instance.Tower12.volume = targetVolume;

        fadeMusicFinished = true;

        StopCoroutine(FadeTracks(0.0f, fadeDurationInBeats));
    }

    /*
    public IEnumerator FadeTrack(float targetVolume, float durationInBeats)
    {
        float timeElapsed = 0.0f;
        float fadeDuration = (ConductorV2.instance.crotchet * durationInBeats);

        float initialVolume = ConductorV2.instance.'track'.volume; // get track initial volume
        float fadeProgress = 0.0f;

        while (timeElapsed < fadeDuration) //this should be the time across 4 beats
        {
            fadeProgress = timeElapsed / fadeDuration;
            ConductorV2.instance.'track'.volume = Mathf.Lerp(initialVolume, targetVolume, fadeProgress);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        ConductorV2.instance.'track'.volume = targetVolume; // set track volume to exact target volume

        StopCoroutine(FadeTrack(0.0f, fadeDurationInBeats));
    }
    */
    #endregion

    #region Resource bar
    void ResourceBar()
    {
        resourceNum = Mathf.Clamp(resourceNum, 0, resourceCap);
        
        if (GameManager.Instance.isInfiniteResources)
        {
            return;
        }
        else
        {
            resourceNumText.text = resourceNum.ToString();
        } 
    }
    #endregion

    #region Enemy timer
    // Starts spawning the first enemies after an initial countdown
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

        enemiesSpawnIn.text = "Enemies Spawn in " + enemyTimer;
    }

    // Updates the delay timer that counts down between the start of a level and when the first enemies start spawning
    public void BeatCountdown()
    {
        enemyTimer -= 1;
        if (switchColor)
            enemiesSpawnIn.color = Color.red;
        else
            enemiesSpawnIn.color = Color.blue;
        switchColor = !switchColor;
    }
    #endregion

    #region Tutorial resource generation
    //
    public void GenerateResource()
    {
        if (GameManager.Instance.tutorialRunning && CursorTD.Instance.movementSequence)
            return;

        if (GameManager.Instance.tutorialRunning && resourceNum >= 25 && !CursorTD.Instance.towerPlaceSequence && !CursorTD.Instance.towerBuffSequence && !CursorTD.Instance.feverModeSequence && !CursorTD.Instance.towerPlacementMenuSequencePassed)
        {
            // Make sure index is set to whichever text says "Moving on-beat gives energy"
            if (TutorialManager.Instance.index == 4)
                TutorialManager.Instance.LoadNextTutorialDialogue();
            CursorTD.Instance.towerPlacementMenuSequence = true;
            return;
        }
        resourceNum += 1;
        return;
    }
    #endregion
}
