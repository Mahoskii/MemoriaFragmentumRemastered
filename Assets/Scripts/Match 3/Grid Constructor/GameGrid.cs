using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class GameGrid<TGridObject>
{
    private int width;
    private int height;
    private float cellSize;
    private Vector3 originPosition;
    private TGridObject[,] gameGridArray;

    //Game Board Constructor
    public GameGrid(int width, int height, float cellSize, Vector3 originPosition,
        Func<bool, GameObject, TGridObject> createGridObject,
        Func<string> getRandomTag,
        Func<string, Vector3, Quaternion, GameObject> activateGameObject,
        Action DisableIngredients, List<GameObject> ingredientsToDisable)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;
        DisableIngredients();
        gameGridArray = new TGridObject[width, height];

        for (int x = 0; x < gameGridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gameGridArray.GetLength(1); y++)
            {
                Vector3 worldPosition = GetWorldPosition(x, y) + (new Vector3(cellSize, cellSize) * 0.5f);
                GameObject ingredient = activateGameObject(getRandomTag(), worldPosition, Quaternion.identity);// IngredientsPooler.Instance.SpawnIngredientsFromPool(IngredientsPooler.Instance.ChooseRandomTag(), worldPosition, Quaternion.identity);
                ingredient.GetComponent<Ingredients>().SetIndicies(x, y);
                gameGridArray[x, y] = createGridObject(true, ingredient);
                ingredientsToDisable.Add(ingredient);
            }
        }
    }


    // set a value for each grid square based on world position that is converted into index position
    public void SetValue(Vector3 worldPosition, TGridObject value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetValue(x, y, value);
    }

    // set a value for each grid square based on index position
    public void SetValue(int x, int y, TGridObject value)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gameGridArray[x, y] = value;
        }
    }

    //Change array index position to world position
    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + originPosition;
    }

    //Change world position to array index position
    private void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }

    //Get the value in the grid index location based on world position
    public TGridObject GetValue(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetValue(x, y);
    }
    //Get the value in the grid index location
    public TGridObject GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gameGridArray[x, y];
        }
        else
        {
            return default(TGridObject);
        }
    }
}
