using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerObjective
{
    public GameObjective objective;
    public string description;
}

public enum GameObjective
{
    GoToMemoryMachine,
    GoToRoom,
    GoToWork,
    FindActors
}
