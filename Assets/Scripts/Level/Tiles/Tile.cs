using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool canPlaceTower = false;
    [SerializeField] public GameObject placedTower;

    private ParticleSystem spawnInstance;

    [Header("Forecasting for SpawnTiles")]
    public bool forecastingActive = false;
    public GameObject forecastingObject;

    [Header("Spawn Tile Info")]
    public float zPos = 0;

    [Header("Target Tile Info")]
    public Transform targetPos;

    private void Update()
    {
        if(forecastingObject != null)
        {
            forecastingObject.SetActive(forecastingActive);
        }
    }


    public void EnemySpawnEffect(ParticleSystem particles)
    {
        spawnInstance = Instantiate(particles, transform.position, Quaternion.identity);
    }
    
    public void ForecastEnemy()
    {
        forecastingActive = true;
    }

    public void StopForecasting()
    {
        forecastingActive = false;
    }
}
