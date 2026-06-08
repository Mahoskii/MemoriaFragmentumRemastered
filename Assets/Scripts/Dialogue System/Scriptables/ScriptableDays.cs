using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ScriptableDays : ScriptableObject
{
    [Header("Dialogues per day")]

    public ScriptableDialogue[] Scenes;
}
