using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ScriptableLevelData : ScriptableObject
{
    [Header("Level number")]
    public int lvlNum;
    [Header("Board Initialization Information")]
    public int boardWidth;
    public int boardHeight;
    public float cellSize;
    public Vector3 boardStartLocation;
    [Header("Level Information")]
    public IngredientName[] lvlIngredientsList;
    public int[] IngredientScore;
    public int movesForThisLvl;
}
public enum IngredientName
{
    GreenStuff,
    RedStuff,
    BlueStuff,
    Whiskey,
    Honey,
    HotPyro,
    Juixa,
    Rum,
    SpaceAcid,
    Stover,
    Tonicer,
    VoidFruit,
    Vorb,
    VorbBlood,
    VorbPiss,
    Watermelon
}
