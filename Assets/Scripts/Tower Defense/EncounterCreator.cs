using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Encounter", menuName = "ScriptableObjects/EncounterCreator")]
public class EncounterCreator : ScriptableObject
{
    [Header("Encounter Info")]
    public string encounterName;
    public string LevelLabel;

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
