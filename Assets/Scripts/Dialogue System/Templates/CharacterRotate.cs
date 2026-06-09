using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterRotate
{
    [Header("Rotation Information")]
    public CharacterNames charToRotate;
    public FacingPositions position;
}

public enum FacingPositions
{
    Left,
    Right,
    Down,
    Up
}
