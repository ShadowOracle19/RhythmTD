using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Encounter", menuName = "ScriptableObjects/EncounterCreator")]
public class EncounterCreator : ScriptableObject
{
    [Header("Encounter Info")]
    public string encounterName;
    public string LevelLabel;
    public LevelSelection stage;

    [Header("Base Encounter Creator")]
    public TextAsset introDialogue;
    public CombatMaker combatEncounter;
    public TextAsset endDialogue;

    [Header("Tutorial")]
    public bool isTutorial = false;

    [Header("Boss Battle")]
    public bool isBossBattle = false;

    [Header("Show Case Level")]
    public bool isShowcase = false;

    [Header("Encounter Data")]
    public EncounterData data;
    public Color fillColor;
    public bool isLevelLocked = true;
    public EncounterCreator levelThatUnlocks;

    [Header("Objectives")]
    public bool clearedObjective01; //cleared the level
    public bool clearedObjective02; //cleared the level without losing health (does not count if the player lost and regained health)
    public bool clearedObjective03; //unique level objective
}

[System.Serializable]
public class EncounterData
{
    // Level Info
    public string objectiveDesc01;
    public string objectiveDesc02;
    public string objectiveDesc03;
    public Sprite levelPreview;
    public Sprite objectiveIncompleteIcon;
    public Sprite objectiveCompleteIcon;
    public List<Sprite> intelIcons;
}
