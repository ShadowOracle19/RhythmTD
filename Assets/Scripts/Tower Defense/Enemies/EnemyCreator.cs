using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Type", menuName = "ScriptableObjects/EnemyType")]
public class EnemyCreator : ScriptableObject
{
    [Header("Enemy Info")]
    public string enemyName;
    [TextArea(5,20)]
    public string enemyDescription;
    public Sprite enemySprite;

    [Header("Enemy Stats")]
    public int maxHealth;
    public EnemyMovementPattern movementPattern;
    public ClashStrength clashStrength;
    public int deathIncome;
    public bool onDeathEffect;
}

public enum EnemyMovementPattern
{
    everyBeat, everyOtherBeat, random, moveThenCast, dontMove, everyTwoBeats, oncePerBar, onceEveryTwoBars
}

public enum ClashStrength
{
    Weak, Medium, High, Immune
}
