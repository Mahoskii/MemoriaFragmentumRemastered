using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterMove
{
    [Header("Movement Information")]
    public CharacterNames charToMove;
    public Vector3[] locations;
    public float[] duration;
}
