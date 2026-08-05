using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchResult
{
    public List<Ingredients> connectedIngredients;
    public MatchDirection direction;
}
public enum MatchDirection
{
    Vertical,
    Horizonal,
    LongVertical,
    LongHorizontal,
    Super,
    None
}
