using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Conversation", menuName = "ScriptableObjects/DialogueCreator")]
public class Conversation : ScriptableObject
{
    public TextAsset dialogue;
}
