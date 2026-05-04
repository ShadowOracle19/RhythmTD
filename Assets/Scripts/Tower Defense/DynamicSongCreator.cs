using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dynamic Song", menuName = "ScriptableObjects/DynamicSong")]
public class DynamicSongCreator : ScriptableObject
{
    public string songName;
    public int bpm;

    [Header("Tracks")]
    public AudioClip major; //bass
    public AudioClip flats; //drums
    public AudioClip trill; //guitarharmony
    public AudioClip chromatic; //guitarmelody
    public AudioClip allegro; //piano
    public AudioClip poco;
    public AudioClip forte;
    public AudioClip legato;
    public AudioClip tower9;
    public AudioClip tower10;
    public AudioClip tower11;
    public AudioClip tower12;

}
