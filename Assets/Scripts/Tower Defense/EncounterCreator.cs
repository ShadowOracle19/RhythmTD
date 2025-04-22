using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Encounter", menuName = "ScriptableObjects/EncounterCreator")]
public class EncounterCreator : ScriptableObject
{
    [Header("Base Encounter Creator")]
    public TextAsset introDialogue;
    public CombatMaker combatEncounter;
    public TextAsset endDialogue;

    [Header("Tutorial")]
    public bool isTutorial = false;

    [Header("Show Case Level")]
    public bool isShowcase = false;

}
