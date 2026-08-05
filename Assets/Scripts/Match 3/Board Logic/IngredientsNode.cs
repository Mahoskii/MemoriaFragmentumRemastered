using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientsNode
{
    public bool isUseable;

    public GameObject ingredient;

    public IngredientsNode(bool isUsable, GameObject ingredient)
    {
        this.isUseable = isUsable;
        this.ingredient = ingredient;
    }
}
