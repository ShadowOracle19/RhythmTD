using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Combat", menuName = "ScriptableObjects/CombatCreator")]
public class CombatMaker : ScriptableObject
{
    [Header("<b><size=15>Combat Encounter Data</size></b>")]
    public DynamicSongCreator dynamicSong;
    public GameObject stagePrefab;
    public int startingResources = 50;
    public List<Wave> waves = new List<Wave>();

    [Header("Tower Limitation")]
    [Tooltip("Enable this to limit tower placement")]
    public bool enableTowerLimit = false;
    public int towerLimit = 1;

    [Space(20)]
    [Header("End Level Text")]
    [Line(255, 255, 255)]
    [TextArea(2, 4)]
    public string wonGame = "Congrats on winning rockstar!";
    [TextArea(2, 4)]
    public string lostGame = "Commander is winning!";
}

[System.Serializable]
public class Wave
{
    public int delay;
    public List<EnemyInit> enemies = new List<EnemyInit>();
    public List<PickupInit> pickups = new List<PickupInit>(); //added for pickups
    public bool killAllEnemiesWave;
    public bool collectAllPickupsWave;
    public bool pickupsWave; //added for pickups
}

[System.Serializable]
public class EnemyInit
{
    public GameObject enemy;

    [Range(0,5)] public int tile;

}

[System.Serializable]
public class PickupInit //added for pickups
{
    public GameObject pickup;

    [Range(0,5)] public int tile;

}


