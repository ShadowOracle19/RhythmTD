using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tower Type", menuName = "ScriptableObjects/TowerType")]
public class TowerTypeCreator : ScriptableObject
{
    [Header("Tower Info")]
    public string towerName;
    [TextArea(5,20)]
    public string towerDescription;
    public Sprite towerImage;

    [Header("Tower Stats")]
    public InstrumentType type;
    public int resourceCost = 0;
    public TowerResourceCost cost;
    [Tooltip("In Beats")]
    public int cooldownTime = 0;
    public int towerHealth = 0;
    public TowerAttackPattern attackPattern;
    public int damage = 0;
    public int range = 0;
    [Tooltip("Set to true if projectile can pierce through enemies")]
    public bool projectilePiercesEnemies = false;
    [Tooltip("Also changes color of AOE attack")]
    public Color projectileColor;

    [Header("AOE")]
    [Tooltip("Set to true if you want tower to be AOE")]
    public bool isAOETower = false;

    [Header("Resource Tower")]
    public bool isResourceTower = false;
    public int resourceGain = 5;

}

public enum TowerAttackPattern
{

    everyBeat, everyMeasure, everyOtherBeat, everyBeatButOne, snakePatternFire, none

}

public enum TowerResourceCost
{
    one, two, three, four
}
