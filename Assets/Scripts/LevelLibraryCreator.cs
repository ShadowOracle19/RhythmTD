using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level Library", menuName = "ScriptableObjects/LevelLibrary")]
public class LevelLibraryCreator : ScriptableObject
{
    [Header("Levels")]
    public List<Level> levels = new List<Level>();
}

[System.Serializable]
public class Level
{
    public CombatMaker level;
    public GameObject levelMapObject;
}