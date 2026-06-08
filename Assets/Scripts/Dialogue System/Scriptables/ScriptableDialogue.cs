using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ScriptableDialogue : ScriptableObject
{
    [Header("Scene Dialogues")]
    public bool isAnswer;
    public DialogueLinesAndActions[] Dialogue;
}
