using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueAudioInfo", menuName = "ScriptableObjects/DialogueAudioInfoSO", order = 1)]


public class DialogueAudioInfoSO : ScriptableObject
{
    public string id;

    public AudioClip[] dialogueTypingSoundClips;
 
}