using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool cantPlaceTower = false;
    [SerializeField] public GameObject placedTower;

    private ParticleSystem spawnInstance;

    [Header("Forecasting for SpawnTiles")]
    public bool forecastingActive = false;
    public GameObject forecastingObject;

    [Header("Spawn Tile Info")]
    public float xPos = -5.5f;
    public float yPos = 0.5f;
    public float zPos = 0;

    [Header("Target Tile Info")]
    public Transform targetPos;

    [Header("Tile Glow")]
    public Renderer glowObject;
    public Material glowMaterial;
    public Material unglowMaterial;

    void Start()
    {
        xPos = -5.5f;
        yPos = 0.5f;
        zPos = this.gameObject.transform.position.z;
    }
    
    private void Update()
    {
        if(forecastingObject != null)
        {
            forecastingObject.SetActive(forecastingActive);
        }

        if (placedTower != null)
        {
            glowObject.material = glowMaterial;
        }
        else
        {
            glowObject.material = unglowMaterial;
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
