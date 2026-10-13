using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Pickup Type", menuName = "ScriptableObjects/PickupType")]
public class PickupCreator : ScriptableObject
{
    [Header("Pickup Info")]
    public string pickupName;
    [TextArea(5,20)]
    public string pickupDescription;
    public Sprite pickupSprite;

    [Header("Pickup Stats")]
    public int score;
    public int health;
    public int energy;
    public PickupMovementPattern movementPattern;
    public int moveSpeed;
    public PickupType pickupType;
}

public enum PickupMovementPattern
{
    everyBeat, everyOtherBeat, dontMove, everyTwoBeats, oncePerBar, onceEveryTwoBars
}

public enum PickupType
{
    score, health, energy
}
