using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tower Type", menuName = "ScriptableObjects/TowerType")]
public class TowerTypeCreator : ScriptableObject
{
    [System.Serializable]
    public class Note
    {
        [Range(0.0f, 1.0f)]
        public float notePosition;
        public float noteTime;
        public float holdTime;
    }
    
    [Header("<b><size=15>Tower Info</size></b>")]
    [Line(255,255,255)]
    public string towerName;
    public InstrumentType type;
    [TextArea(5,20)]
    public string towerDescription;
    public Sprite towerImage;
    public GameObject towerLoadoutUIPrefab;
    public TowerResourceCost cost;
    public CooldownTime cooldown;


    [Space(20)][Header("<b><size=15>Tower Stats</size></b>")]
    [Line(255, 255, 255)]
    public int towerHealth = 0;
    public int resourceCost = 0;
    [Tooltip("In Beats")]
    public int cooldownTime = 0; 
    public int damage = 0;
    public int range = 0;
    [Space(5)]
    [Tooltip("Allows the tower to attack regardless of enemy in range")]
    public bool canAttackWhenever = false;
    [Tooltip("Set to true if projectile can pierce through enemies")]
    public bool projectilePiercesEnemies = false;

    [Space(20)][Header("<b><size=15>Tower Attack & Input</size></b>")]
    [Line(255, 255, 255)]
    public TowerAttackPattern attackPattern;
    public List<Note> inputs = new List<Note>();
    //public Note[] inputs;

    [Space(20)][Header("<b><size=15>Tower Parameters</size></b>")]
    [Line(255, 255, 255)]

    [Header("AOE")]
    [Tooltip("Set to true if you want tower to be AOE")]
    public bool isAOETower = false;


    [Space(5)][Header("Resource Tower")]
    public bool isResourceTower = false;
    public int resourceGain = 5;


    [Space(20)]
    [Header("<b><size=15>Tower Upgrades</size></b>")]

    [Line(255, 255, 255)]
    [Header("Tower Upgrade One")]
    public Sprite upgrade1;
    public int upgradeCost1 = 25;
    [Tooltip("Set to true to lock, set to false to unlock")]
    public bool isUpgradeOneLocked = true;

    [Space(5)]
    [Header("Tower Upgrade Two")]
    public Sprite upgrade2;
    public int upgradeCost2 = 25;
    [Tooltip("Set to true to lock, set to false to unlock")]
    public bool isUpgradeTwoLocked = true;

    [Space(5)]
    [Header("Tower Upgrade Three")]
    public Sprite upgrade3;
    public int upgradeCost3 = 25;
    [Tooltip("Set to true to lock, set to false to unlock")]
    public bool isUpgradeThreeLocked = true;


    //[Space(5)]
    //[Header("Tower Upgrade Four")]
    //public Sprite upgrade4;
    //public int upgradeCost4 = 25;
}

public enum TowerAttackPattern
{

    standard, snake, none
    //everyBeat, everyMeasure, everyOtherBeat, everyBeatButOne, snakePatternFire, none

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
