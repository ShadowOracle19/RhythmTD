using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Combat", menuName = "ScriptableObjects/CombatCreator")]
public class CombatMaker : ScriptableObject
{
    
    public DynamicSongCreator dynamicSong;
    public List<Wave> waves = new List<Wave>();
}

[System.Serializable]
public class Wave
{
    public int delay;
    public GameObject enemy;
    public int numberOfEnemies;
    public bool killAllEnemiesWave;
}