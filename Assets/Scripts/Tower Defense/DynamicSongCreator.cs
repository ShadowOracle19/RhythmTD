using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dynamic Song", menuName = "ScriptableObjects/DynamicSong")]
public class DynamicSongCreator : ScriptableObject
{
    public string songName;
    public int bpm;

    [Header("Tracks")]
    public AudioClip bass;
    public AudioClip drums;
    public AudioClip guitarHarmony;
    public AudioClip guitarMelody;
    public AudioClip piano;
}
