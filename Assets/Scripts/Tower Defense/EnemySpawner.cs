using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    #region dont touch this
    private static EnemySpawner _instance;
    public static EnemySpawner Instance
    {
        get
        {
            if (_instance is null)
            {
                Debug.LogError("EnemySpawner Manager is NULL");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }
    #endregion

    public List<Wave> currentWaves = new List<Wave>();

    public bool startOnce = false;

    public int numberOfEnemiesToSpawn = 0;
    public int currentNumberOfEnemiesSpawned = 0;

    public bool allEnemiesSpawned = false;

    public Transform enemyParent;

    public int lastRandomSpawn;

    public List<GameObject> spawnTiles = new List<GameObject>();

    public List<Enemy> enemies = new List<Enemy>();

    public bool killAllEnemiesBeforeNextWave = false;

    //to be changed to using beats once it's been proven to work
    public float forecastEndsIn = 4;
    //don't touch this bit below, this is fine
    public bool forecastingActive = false;

    [SerializeField] private ParticleSystem spawnParticles;

    [Header("Wave info")]
    public float timeRemainingToWaveStart = 0;
    public int waveIndex = 0;
    public bool allEnemiesSpawnedFromWave = false;
    public int numEnemiesInWave = 0;
    public int delay;

    [Header("EnemyType")]
    public GameObject walkerEnemy;
    public GameObject wispEnemy;

    private void Update()
    {
        GameManager.Instance.waveCounter.text = "Wave " + waveIndex + "/" + currentWaves.Count;
        if(startOnce && allEnemiesSpawnedFromWave)
        {
            if (killAllEnemiesBeforeNextWave && enemies.Count != 0)
                return;
            if (timeRemainingToWaveStart >= delay)
            {
                //allow enemies to spawn
                if(waveIndex >= currentWaves.Count) //if at the last wave stop running this
                {
                    return;
                }
                timeRemainingToWaveStart = 0;//set delay for next wave
                allEnemiesSpawnedFromWave = false;
                killAllEnemiesBeforeNextWave = currentWaves[waveIndex].killAllEnemiesWave;
            }
            else
            {
                
                timeRemainingToWaveStart += Time.deltaTime;
            }
        }
    }

    public void ForceEnemySpawn(float ypos, EnemyType enemyType)
    {
        numberOfEnemiesToSpawn += 1;
        currentNumberOfEnemiesSpawned += 1;
        GameObject enemy = null;
        switch (enemyType)
        {
            case EnemyType.Wisp:
                enemy = Instantiate(wispEnemy, new Vector3(transform.position.x, 0.5f, ypos), Quaternion.identity, enemyParent);
                break;
            case EnemyType.Walker:
                enemy = Instantiate(walkerEnemy, new Vector3(transform.position.x, 0.5f, ypos), Quaternion.identity, enemyParent);
                break;
            default:
                break;
        }
        
        
        ConductorV2.instance.triggerEvent.Add(enemy.GetComponent<Enemy>().trigger);

    }

    public void StartSpawningEnemies()
    {
        if(!startOnce)
        {
            startOnce = true;
            timeRemainingToWaveStart = 0;
            waveIndex = 0;
            numEnemiesInWave = 0;
            delay = 0;
            allEnemiesSpawnedFromWave = true;
            killAllEnemiesBeforeNextWave = currentWaves[waveIndex].killAllEnemiesWave;
        }
    }

    public void SpawnUnit()
    {
        if (!startOnce || GameManager.Instance.isGamePaused || allEnemiesSpawnedFromWave)
            return;

        //once all enemies are spawned stop spawning them
        if (currentNumberOfEnemiesSpawned >= numberOfEnemiesToSpawn) 
        {
            allEnemiesSpawned = true;
            return;
        }
        

        for (int i = 0; i < currentWaves[waveIndex].numberOfEnemies; i++)
        {
            int randSpawn = Random.Range(0, spawnTiles.Count);
            if (randSpawn == lastRandomSpawn)
            {
                randSpawn = Random.Range(0, spawnTiles.Count);
            }
            GameObject enemy = Instantiate(currentWaves[waveIndex].enemy, new Vector3(transform.position.x, 0.5f, spawnTiles[randSpawn].GetComponent<Tile>().zPos), Quaternion.identity, enemyParent);
            lastRandomSpawn = randSpawn;


            spawnTiles[randSpawn].GetComponent<Tile>().forcastEnemy(spawnParticles);
            

            ConductorV2.instance.triggerEvent.Add(enemy.GetComponent<Enemy>().trigger);

            enemies.Add(enemy.GetComponent<Enemy>());

            currentNumberOfEnemiesSpawned += 1;

            numEnemiesInWave += 1;
        }
        


        //if (GameManager.Instance.tutorialRunning)
        //    return;

        
        if(numEnemiesInWave == currentWaves[waveIndex].numberOfEnemies)
        {
            
            waveIndex += 1;
            numEnemiesInWave = 0;
            allEnemiesSpawnedFromWave = true;

            if(waveIndex == currentWaves.Count)
            {
                allEnemiesSpawned = true;
                return;
            }
            delay = currentWaves[waveIndex].delay;
        }
    }
}

public enum EnemyType
{
    Wisp, Walker
}
