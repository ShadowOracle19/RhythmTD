using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dynamic Song", menuName = "ScriptableObjects/DynamicSong")]
public class DynamicSongCreator : ScriptableObject
{
    [Header("<b><size=15>Song Data<b><size=15>")]
    [Line(255,255,255)]
    public string songName;
    public int bpm;

    [Space(20)][Header("<b><size=15>Song Tracks<b><size=15>")]
    [Line(255,255,255)]
    public AudioClip major; //bass
    public AudioClip flat; //drums
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
