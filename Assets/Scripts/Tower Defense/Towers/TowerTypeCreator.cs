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
    public GameObject towerLoadoutUIPrefab;

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
    [Tooltip("Allows the tower to attack regardless of enemy in range")]
    public bool canAttackWhenever = false;
    [Tooltip("Set to true if projectile can pierce through enemies")]
    public bool projectilePiercesEnemies = false;

    [Header("AOE")]
    [Tooltip("Set to true if you want tower to be AOE")]
    public bool isAOETower = false;

    [Header("Resource Tower")]
    public bool isResourceTower = false;
    public int resourceGain = 5;

    [Header("Tower Upgrade One")]
    public Sprite upgrade1;
    public int upgradeCost1 = 25;

    [Header("Tower Upgrade Two")]
    public Sprite upgrade2;
    public int upgradeCost2 = 25;

    [Header("Tower Upgrade Three")]
    public Sprite upgrade3;
    public int upgradeCost3 = 25;

    [Header("Tower Upgrade Four")]
    public Sprite upgrade4;
    public int upgradeCost4 = 25;
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
