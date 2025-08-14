using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Spawner : MonoBehaviour
{
    #region dont touch this
    private static Spawner _instance;
    public static Spawner Instance
    {
        get
        {
            if (_instance is null)
            {
                Debug.LogError("Spawner Manager is NULL");
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
    public int numberOfPickupsToSpawn = 0;
    public int currentNumberOfEnemiesSpawned = 0;
    public int currentNumberOfPickupsSpawned = 0;

    public bool allEnemiesSpawned = false;
    public bool allPickupsSpawned = false;

    public Transform enemyParent;
    public Transform pickupParent;

    public int lastRandomSpawn;

    public List<GameObject> spawnTiles = new List<GameObject>();
    public List<GameObject> pickupSpawnTiles = new List<GameObject>();

    public List<Enemy> enemies = new List<Enemy>();
    public List<Pickup> pickups = new List<Pickup>();

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
    public bool allPickupsSpawnedFromWave = false;
    public int numPickupsInWave = 0;
    public int delay;

    [Header("EnemyType")]
    public GameObject walkerEnemy;
    public GameObject wispEnemy;

    /*
    private void Update()
    {
        WaveCounter();
    }
    */

    public void WaveCounter()
    {
        GameManager.Instance.waveCounter.text = "Wave " + waveIndex + "/" + currentWaves.Count;
        
        if (startOnce && allEnemiesSpawnedFromWave && allPickupsSpawnedFromWave)
        {
            if (killAllEnemiesBeforeNextWave && enemies.Count != 0)
                return;
            if (timeRemainingToWaveStart >= delay) //reset delay
            {
                //allow enemies to spawn
                if (waveIndex >= currentWaves.Count) //if at the last wave stop running this
                {
                    return;
                }

                timeRemainingToWaveStart = 0; //set delay for next wave
                allEnemiesSpawnedFromWave = false;
                allPickupsSpawnedFromWave = false;
                StopForecastingWave(waveIndex);
                killAllEnemiesBeforeNextWave = currentWaves[waveIndex].killAllEnemiesWave;
            }
            else
            {
                ForecastWave(waveIndex);
                //timeRemainingToWaveStart += Time.deltaTime;
                timeRemainingToWaveStart += 1;
            }
        }
    }

    public void ForceEnemySpawn(float zpos, EnemyType enemyType)
    {
        numberOfEnemiesToSpawn += 1;
        currentNumberOfEnemiesSpawned += 1;
        GameObject enemy = null;
        switch (enemyType)
        {
            case EnemyType.Wisp:
                enemy = Instantiate(wispEnemy, new Vector3(transform.position.x, 0.5f, zpos), Quaternion.identity, enemyParent);
                break;
            case EnemyType.Walker:
                enemy = Instantiate(walkerEnemy, new Vector3(transform.position.x, 0.5f, zpos), Quaternion.identity, enemyParent);
                break;
            default:
                break;
        }
        
        
        ConductorV2.instance.enemyEvent.Add(enemy.GetComponent<Enemy>().trigger);

    }

    public void ForceEnemySpawnDynamic(float zPos, GameObject enemy)
    {
        numberOfEnemiesToSpawn += 1;
        currentNumberOfEnemiesSpawned += 1;

        GameObject _enemy = null;

        _enemy = Instantiate(enemy, new Vector3(transform.position.x, 0.5f, zPos), Quaternion.identity, enemyParent);
        

        ConductorV2.instance.enemyEvent.Add(_enemy.GetComponent<Enemy>().trigger);

    }

    public void ForceEnemySpawnOnTile(Transform pos, GameObject enemy)
    {
        numberOfEnemiesToSpawn += 1;
        currentNumberOfEnemiesSpawned += 1;

        GameObject _enemy = null;

        _enemy = Instantiate(enemy, pos.position, Quaternion.identity, enemyParent);


        ConductorV2.instance.enemyEvent.Add(_enemy.GetComponent<Enemy>().trigger);

    }

    public void SpawnUnitOnRandomTile(GameObject enemyPrefab)
    {
        numberOfEnemiesToSpawn += 1;
        currentNumberOfEnemiesSpawned += 1;

        GameObject _enemy = null;

        int randSpawnTile = Random.Range(0, spawnTiles.Count - 1);

        _enemy = Instantiate(enemyPrefab, new Vector3(transform.position.x, 0.5f,
                spawnTiles[randSpawnTile].GetComponent<Tile>().zPos), Quaternion.identity, enemyParent);


        ConductorV2.instance.enemyEvent.Add(_enemy.GetComponent<Enemy>().trigger);
    }

    public void StartSpawningEnemies()
    {
        if(!startOnce)
        {
            startOnce = true;
            ResetSpawner();
            killAllEnemiesBeforeNextWave = currentWaves[waveIndex].killAllEnemiesWave;
            StopForecastingWave(0);
        }
    }

    public void ResetSpawner()
    { 
        timeRemainingToWaveStart = 0;
        waveIndex = 0;
        numEnemiesInWave = 0;
        numPickupsInWave = 0;
        allEnemiesSpawned = false;
        allPickupsSpawned = false;
        allEnemiesSpawnedFromWave = false;
        allPickupsSpawnedFromWave = false;
        delay = 0;
    }

    public void SpawnUnit()
    {
        //if enemies haven't started spawning  or  game is paused  or  all enemies have been spawned in the current wave
        if (!startOnce || GameManager.Instance.isGamePaused || allEnemiesSpawnedFromWave)
            return;

        //once all enemies are spawned stop spawning them
        if (currentNumberOfEnemiesSpawned >= numberOfEnemiesToSpawn) 
        {
            Debug.Log("All Enemies Spawned");
            
            allEnemiesSpawnedFromWave = true;
            allEnemiesSpawned = true;

            

            if (allEnemiesSpawnedFromWave && allPickupsSpawnedFromWave  && (waveIndex != currentWaves.Count-1))
            {
                waveIndex += 1;
                numEnemiesInWave = 0;
                numPickupsInWave = 0;
                delay = currentWaves[waveIndex].delay;
            } 

            return;
        }

        //spawn all enemies in the current wave
        for (int i = 0; i < currentWaves[waveIndex].enemies.Count; i++)
        {
            int tileNum = currentWaves[waveIndex].enemies[i].tile;
            tileNum = Mathf.Clamp(tileNum, 0, 5);

            GameObject enemy = Instantiate(currentWaves[waveIndex].enemies[i].enemy, new Vector3(transform.position.x, 0.5f, 
                spawnTiles[tileNum].GetComponent<Tile>().zPos), Quaternion.identity, enemyParent);


            spawnTiles[tileNum].GetComponent<Tile>().EnemySpawnEffect(spawnParticles);
            

            ConductorV2.instance.enemyEvent.Add(enemy.GetComponent<Enemy>().trigger);

            enemies.Add(enemy.GetComponent<Enemy>());

            currentNumberOfEnemiesSpawned += 1;

            numEnemiesInWave += 1;
        }
        
        if(numEnemiesInWave == currentWaves[waveIndex].enemies.Count)
        {
            allEnemiesSpawnedFromWave = true;

            //if last wave, all enemies have been spawned
            if(waveIndex == currentWaves.Count-1)
            {
                allEnemiesSpawned = true;
                return;
            }

            if (allEnemiesSpawnedFromWave && allPickupsSpawnedFromWave)
            {
                waveIndex += 1;
                numEnemiesInWave = 0;
                numPickupsInWave = 0;
                delay = currentWaves[waveIndex].delay;
            }
        }
    }

    public void SpawnPickup()
    {
        //if enemies haven't started spawning  or  game is paused  or  all pickups have been spawned in the current wave
        if (!startOnce || GameManager.Instance.isGamePaused || allPickupsSpawnedFromWave)
            return;
        
        //once all pickups are spawned stop spawning them
        if (currentNumberOfPickupsSpawned >= numberOfPickupsToSpawn) 
        {
            Debug.Log("All Pickups Spawned");

            allPickupsSpawnedFromWave = true;
            allPickupsSpawned = true;

            if (allEnemiesSpawnedFromWave && allPickupsSpawnedFromWave && (waveIndex != currentWaves.Count-1))
            {
                waveIndex += 1;
                numEnemiesInWave = 0;
                numPickupsInWave = 0;
                delay = currentWaves[waveIndex].delay;
            }

            return;
        }

        //spawn all pickups in the current wave
        for (int i = 0; i < currentWaves[waveIndex].pickups.Count; i++)
        {
            int pickupTileNum = currentWaves[waveIndex].pickups[i].tile;
            pickupTileNum = Mathf.Clamp(pickupTileNum, 0, 5);

            GameObject pickup = Instantiate(currentWaves[waveIndex].pickups[i].pickup, new Vector3(pickupSpawnTiles[pickupTileNum].GetComponent<Tile>().xPos, pickupSpawnTiles[pickupTileNum].GetComponent<Tile>().yPos, 
                pickupSpawnTiles[pickupTileNum].GetComponent<Tile>().zPos), Quaternion.identity, pickupParent);

            ConductorV2.instance.pickupEvent.Add(pickup.GetComponent<Pickup>().trigger);

            pickups.Add(pickup.GetComponent<Pickup>());

            currentNumberOfPickupsSpawned += 1;

            numPickupsInWave += 1;
        }
        
        if(numPickupsInWave == currentWaves[waveIndex].pickups.Count)
        {
            allPickupsSpawnedFromWave = true;
 
            //if last wave, all pickups have been spawned
            if(waveIndex == currentWaves.Count-1)
            {
                allPickupsSpawned = true;
                return;
            }

            if (allEnemiesSpawnedFromWave && allPickupsSpawnedFromWave)
            {
                waveIndex += 1;
                numEnemiesInWave = 0;
                numPickupsInWave = 0;
                delay = currentWaves[waveIndex].delay;
            }
        }
    }

    // Telegraph wave
    public void ForecastWave(int _wave)
    {
        if (waveIndex == currentWaves.Count - 1)
            return;
        foreach (var enemyToForecast in currentWaves[_wave].enemies)
        {
            int tileNum = enemyToForecast.tile;
            tileNum = Mathf.Clamp(tileNum, 0, 5);
            spawnTiles[tileNum].GetComponent<Tile>().ForecastEnemy();
        }
           
    }

    // Stop telegraphing wave
    public void StopForecastingWave(int _wave) 
    {
        foreach (var _enemyToForecast in currentWaves[_wave].enemies)
        {
            int tileNum = _enemyToForecast.tile;
            tileNum = Mathf.Clamp(tileNum, 0, 5);
            spawnTiles[tileNum].GetComponent<Tile>().StopForecasting();
        }
    }
}
