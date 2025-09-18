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
    public CooldownTime cooldown;
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

    [Header("Projectile Type")]
    [Tooltip("Set to desired firing type")]
    public ProjectileType projectileType;

    [Header("Resource Tower")]
    public bool isResourceTower = false;
    public int resourceGain = 5;

    [Header("Upgrade Icons")]
    public Sprite upgrade1;
    public Sprite upgrade2;
    public Sprite upgrade3;
    public Sprite upgrade4;
}

public enum TowerAttackPattern
{

    everyBeat, everyMeasure, everyOtherBeat, everyBeatButOne, snakePatternFire, none

}

public enum TowerResourceCost
{
    one, two, three, four
}

public enum ProjectileType
{
    Bullet, AOE, Charges
}

public enum CooldownTime
{
    Short, Medium, Long
}
