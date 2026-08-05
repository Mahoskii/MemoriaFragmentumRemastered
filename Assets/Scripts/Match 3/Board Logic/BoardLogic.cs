using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardLogic : MonoBehaviour
{
    private GameGrid<IngredientsNode> grid;
    private int gridWidth;
    private int gridHeight;
    private float cellSize;
    private Vector3 originPosition;

    [SerializeField] private Camera match3Cam;

    private List<GameObject> ingredientsToDisable = new();
    [SerializeField]
    private Ingredients selectedIngredient;
    [SerializeField]
    private bool isProcessingMove;

    private bool arethereMatches = false;

    List<Ingredients> ingredientsToRemove = new();

    [SerializeField] private ScriptableLevelData[] lvlData;


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = match3Cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null && hit.collider.gameObject.GetComponent<Ingredients>())
            {
                if (isProcessingMove)
                {
                    return;
                }
                Ingredients ingredient = hit.collider.gameObject.GetComponent<Ingredients>();
                SelectAnIngredient(ingredient);
            }
        }
    }


    private void OnEnable()
    {
        GameTurnController.gameFinished += AddAndDisableIngredients;
        InitializeGrid();
    }
    private void OnDisable()
    {
        GameTurnController.gameFinished -= AddAndDisableIngredients;
    }

    public void InitializeGrid()
    {
        gridWidth = lvlData[GameTurnController.Instance.currentLvl].boardWidth;
        gridHeight = lvlData[GameTurnController.Instance.currentLvl].boardHeight;
        cellSize = lvlData[GameTurnController.Instance.currentLvl].cellSize;
        originPosition = lvlData[GameTurnController.Instance.currentLvl].boardStartLocation;
        grid = new GameGrid<IngredientsNode>(gridWidth, gridHeight, cellSize, originPosition, (bool isUseable, GameObject ingredient) => new IngredientsNode(isUseable, ingredient), () => GameTurnController.Instance.ChooseRandomTag(), (string randomeTag, Vector3 position, Quaternion rotation) => IngredientPooler.Instance.SpawnIngredientsFromPool(randomeTag, position, rotation), () => DisableIngredients(), ingredientsToDisable);
        arethereMatches = CheckBoard();
        while (arethereMatches)
        {
            grid = new GameGrid<IngredientsNode>(gridWidth, gridHeight, cellSize, originPosition, (bool isUseable, GameObject ingredient) => new IngredientsNode(isUseable, ingredient), () => GameTurnController.Instance.ChooseRandomTag(), (string randomeTag, Vector3 position, Quaternion rotation) => IngredientPooler.Instance.SpawnIngredientsFromPool(randomeTag, position, rotation), () => DisableIngredients(), ingredientsToDisable);
            arethereMatches = CheckBoard();
        }
    }

    public bool CheckBoard()
    {
        if (GameTurnController.Instance.isGameEnded)
        {
            return false;
        }

        bool hasMatched = false;

        ingredientsToRemove.Clear();

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (grid.GetValue(x, y).ingredient != null)
                {
                    grid.GetValue(x, y).ingredient.GetComponent<Ingredients>().isMatched = false;
                }
            }
        }

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                //checking if potion node is useable
                if (grid.GetValue(x, y).isUseable)
                {
                    //then proceen to get potion class in node.
                    Ingredients ingredient = grid.GetValue(x, y).ingredient.GetComponent<Ingredients>();

                    //ensure its not matched
                    if (!ingredient.isMatched)
                    {
                        //run some matching logic

                        MatchResult matchedIngredients = IsConnected(ingredient);

                        if (matchedIngredients.connectedIngredients.Count >= 3)
                        {
                            //complex matching...
                            MatchResult superMatchedIngredients = SuperMatch(matchedIngredients);

                            ingredientsToRemove.AddRange(superMatchedIngredients.connectedIngredients);
                            foreach (Ingredients ing in superMatchedIngredients.connectedIngredients)
                            {
                                ing.isMatched = true;
                            }

                            hasMatched = true;
                        }
                    }
                }
            }
        }
        return hasMatched;
    }

    public IEnumerator ProcessTurnOnMatchBoard(bool subtractMoves)
    {
        foreach (Ingredients ing in ingredientsToRemove)
        {
            ing.isMatched = false;
        }

        DisableAndRefill(ingredientsToRemove);
        string ingType = ingredientsToRemove[0].ingredientType.ToString();
        GameTurnController.Instance.ProcessTurn(ingType, ingredientsToRemove.Count, subtractMoves);
        yield return new WaitForSeconds(0.4f);

        if (CheckBoard())
        {
            StartCoroutine(ProcessTurnOnMatchBoard(false));
        }
    }

    private void AddAndDisableIngredients()
    {
        List<Ingredients> clearBoarList = new();
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Ingredients ingredientToTurnOff = grid.GetValue(x, y).ingredient.GetComponent<Ingredients>();
                clearBoarList.Add(ingredientToTurnOff);
            }
        }
        foreach (Ingredients ing in clearBoarList)
        {
            //getting it's x and y indicies and storing them
            int xIndex = ing.x;
            int yIndex = ing.y;

            //Disable the ingredient 
            ing.gameObject.SetActive(false);

            //Create a blank node on the game board.
            grid.SetValue(xIndex, yIndex, new IngredientsNode(true, null));
        }

    }
    private void DisableIngredients()
    {
        if (ingredientsToDisable != null)
        {
            foreach (GameObject ing in ingredientsToDisable)
            {
                ing.SetActive(false);
            }
            ingredientsToDisable.Clear();
        }
    }

    #region Cascading Ingredients

    private void DisableAndRefill(List<Ingredients> ingredientsToDisable)
    {
        //Removing the potion and clearing the board at that location
        foreach (Ingredients ing in ingredientsToDisable)
        {
            //getting it's x and y indicies and storing them
            int xIndex = ing.x;
            int yIndex = ing.y;

            //Disable the ingredient 
            ing.gameObject.SetActive(false);

            //Create a blank node on the game board.
            grid.SetValue(xIndex, yIndex, new IngredientsNode(true, null));
        }

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (grid.GetValue(x, y).ingredient == null)
                {
                    RefillIngredients(x, y);
                }
            }
        }
    }
    private void RefillIngredients(int x, int y)
    {
        //y offset
        int yOffset = 1;

        //while the cell above our current cell is null and we're below the board
        while (y + yOffset < gridHeight && grid.GetValue(x, y + yOffset).ingredient == null)
        {
            //increment y offset
            yOffset++;
        }

        //We've either hit the top of the board or we found an ingredient
        if (y + yOffset < gridHeight && grid.GetValue(x, y + yOffset).ingredient != null)
        {
            //we've found an ingredient
            Ingredients ingAbove = grid.GetValue(x, y + yOffset).ingredient.GetComponent<Ingredients>();

            //Move it to the correct location
            Vector3 targetPos = grid.GetWorldPosition(x, y) + (new Vector3(cellSize, cellSize) * 0.5f);
            //Move to location
            ingAbove.MoveToTarget(targetPos);
            //Update indicies
            ingAbove.SetIndicies(x, y);
            //Update game board
            grid.SetValue(x, y, grid.GetValue(x, y + yOffset));
            //set the location the ingredient came from to null
            grid.SetValue(x, y + yOffset, new IngredientsNode(true, null));
        }

        //if we've hit the top of the board without finding an ingredient
        if (y + yOffset == gridHeight)
        {
            SpawnIngredientsAtTop(x);
        }
    }

    private void SpawnIngredientsAtTop(int x)
    {
        int index = FindIndexOfLowestNull(x);
        int locationToMoveTo = gridHeight - index;
        //get a random ingredient
        Vector3 positionToSpawnIn = grid.GetWorldPosition(x, gridHeight) + (new Vector3(cellSize, cellSize) * 0.5f);
        GameObject newIngredient = IngredientPooler.Instance.SpawnIngredientsFromPool(GameTurnController.Instance.ChooseRandomTag(), positionToSpawnIn, Quaternion.identity);
        //set indicies
        newIngredient.GetComponent<Ingredients>().SetIndicies(x, index);
        //set it on the game board
        grid.SetValue(x, index, new IngredientsNode(true, newIngredient));
        //move it to that location
        Vector3 targetPosition = grid.GetWorldPosition(x, index) + (new Vector3(cellSize, cellSize) * 0.5f);
        newIngredient.GetComponent<Ingredients>().MoveToTarget(targetPosition);
    }

    private int FindIndexOfLowestNull(int x)
    {
        int lowestNull = 99;
        for (int y = gridHeight - 1; y >= 0; y--)
        {
            if (grid.GetValue(x, y).ingredient == null)
            {
                lowestNull = y;
            }
        }
        return lowestNull;
    }

    #endregion

    #region Checking the Board For Matches

    private MatchResult SuperMatch(MatchResult matchedIngredients)
    {
        //if we have a horizontal or long horizontal match
        if (matchedIngredients.direction == MatchDirection.Horizonal || matchedIngredients.direction == MatchDirection.LongHorizontal)
        {
            //for each ingredient...
            foreach (Ingredients ing in matchedIngredients.connectedIngredients)
            {
                List<Ingredients> extraConnectedIngredients = new();
                //check up
                CheckDirection(ing, new Vector3Int(0, 1), extraConnectedIngredients);
                //check down
                CheckDirection(ing, new Vector3Int(0, -1), extraConnectedIngredients);

                //do we have 2 or more ingredients that have been matched against this current ingredient?
                if (extraConnectedIngredients.Count >= 2)
                {
                    extraConnectedIngredients.AddRange(matchedIngredients.connectedIngredients);

                    //return our super match
                    return new MatchResult
                    {
                        connectedIngredients = extraConnectedIngredients,
                        direction = MatchDirection.Super
                    };
                }
            }
            //we didn't have a supert match, so return out normal match
            return new MatchResult
            {
                connectedIngredients = matchedIngredients.connectedIngredients,
                direction = matchedIngredients.direction
            };
        }
        //preform the same check for the vertical matches if needed
        else if (matchedIngredients.direction == MatchDirection.Vertical || matchedIngredients.direction == MatchDirection.LongVertical)
        {
            foreach (Ingredients ing in matchedIngredients.connectedIngredients)
            {
                List<Ingredients> extraConnectedIngredients = new();
                CheckDirection(ing, new Vector3Int(1, 0), extraConnectedIngredients);
                CheckDirection(ing, new Vector3Int(-1, 0), extraConnectedIngredients);

                if (extraConnectedIngredients.Count >= 2)
                {
                    extraConnectedIngredients.AddRange(matchedIngredients.connectedIngredients);
                    return new MatchResult
                    {
                        connectedIngredients = extraConnectedIngredients,
                        direction = MatchDirection.Super
                    };
                }
            }
            return new MatchResult
            {
                connectedIngredients = matchedIngredients.connectedIngredients,
                direction = matchedIngredients.direction
            };
        }

        return null;
    }

    MatchResult IsConnected(Ingredients ingredient)
    {
        List<Ingredients> connectedIngredients = new();
        IngredientType ingredientType = ingredient.ingredientType;

        connectedIngredients.Add(ingredient);
        //check right
        CheckDirection(ingredient, new Vector3Int(1, 0), connectedIngredients);
        //check left
        CheckDirection(ingredient, new Vector3Int(-1, 0), connectedIngredients);
        //have we made a 3 match? (Horizontal Match)
        if (connectedIngredients.Count == 3)
        {
            return new MatchResult
            {
                connectedIngredients = connectedIngredients,
                direction = MatchDirection.Horizonal
            };
        }
        //checking for more than 3 (Long Horizonatl Match)
        else if (connectedIngredients.Count > 3)
        {
            return new MatchResult
            {
                connectedIngredients = connectedIngredients,
                direction = MatchDirection.LongHorizontal
            };
        }
        //clear out the connected ingrediants
        connectedIngredients.Clear();
        //readd out initial potion
        connectedIngredients.Add(ingredient);
        //check up
        CheckDirection(ingredient, new Vector3Int(0, 1), connectedIngredients);
        //check down
        CheckDirection(ingredient, new Vector3Int(0, -1), connectedIngredients);
        //have we made a 3 match? (Vertical Match)
        if (connectedIngredients.Count == 3)
        {
            return new MatchResult
            {
                connectedIngredients = connectedIngredients,
                direction = MatchDirection.Vertical
            };
        }
        //checking for more than 3 (Long Vertical Match)
        else if (connectedIngredients.Count > 3)
        {
            return new MatchResult
            {
                connectedIngredients = connectedIngredients,
                direction = MatchDirection.LongVertical
            };
        }
        else
        {
            return new MatchResult
            {
                connectedIngredients = connectedIngredients,
                direction = MatchDirection.None
            };
        }
    }

    void CheckDirection(Ingredients ing, Vector3Int direction, List<Ingredients> connectedIngredients)
    {
        IngredientType ingredientType = ing.ingredientType;
        int x = ing.x + direction.x;
        int y = ing.y + direction.y;

        //check that we're within the boundaries of the board
        while (x >= 0 && x < gridWidth && y >= 0 && y < gridHeight)
        {
            if (grid.GetValue(x, y).isUseable)
            {
                Ingredients neighbourIngredient = grid.GetValue(x, y).ingredient.GetComponent<Ingredients>();
                //does our ingredientType Match? it must also not be matched
                if (!neighbourIngredient.isMatched && neighbourIngredient.ingredientType == ingredientType)
                {
                    connectedIngredients.Add(neighbourIngredient);

                    x += direction.x;
                    y += direction.y;
                }
                else
                {
                    break;
                }
            }
            else
            {
                break;
            }
        }
    }

    #endregion

    #region Swapping

    //Select an ingredient
    public void SelectAnIngredient(Ingredients ingredient)
    {
        // if we dont have an ingredient currently selected then set the ingredient i just clicked to my selected ingredient
        if (selectedIngredient == null)
        {
            selectedIngredient = ingredient;
        }
        // if we select the same ingredient twice, then make selected ingredient null
        else if (selectedIngredient == ingredient)
        {
            selectedIngredient = null;
        }
        // if selected ingredient is not null and not current potion, attempt a swap
        //selected ingredient back to null
        else if (selectedIngredient != ingredient)
        {
            SwapIngredients(selectedIngredient, ingredient);
            selectedIngredient = null;
        }

    }
    //swap ingredient - logic
    private void SwapIngredients(Ingredients currentIng, Ingredients targetIng)
    {
        //!IsAdjacent dont do anything
        if (!IsAdjacent(currentIng, targetIng))
        {
            //AudioManager.Instance.PlaySFX("WrongMove");
            return;
        }

        DoSwap(currentIng, targetIng);

        isProcessingMove = true;

        StartCoroutine(ProcessMatches(currentIng, targetIng));
    }
    // do swap
    private void DoSwap(Ingredients currentIng, Ingredients targetIng)
    {
        GameObject temp = grid.GetValue(currentIng.x, currentIng.y).ingredient;
        grid.GetValue(currentIng.x, currentIng.y).ingredient = grid.GetValue(targetIng.x, targetIng.y).ingredient;
        grid.GetValue(targetIng.x, targetIng.y).ingredient = temp;

        //update indicies
        int tempXIndex = currentIng.x;
        int tempYIndex = currentIng.y;
        currentIng.x = targetIng.x;
        currentIng.y = targetIng.y;
        targetIng.x = tempXIndex;
        targetIng.y = tempYIndex;

        currentIng.MoveToTarget(grid.GetValue(targetIng.x, targetIng.y).ingredient.transform.position);
        targetIng.MoveToTarget(grid.GetValue(currentIng.x, currentIng.y).ingredient.transform.position);
        // the function is working properly. try to instead of moving it with the function in the ingerdients script to move it
        // with the function that instantiates them from the pool.
        //AudioManager.Instance.PlaySFX("RightMove");
    }
    //IsAdjacet
    private bool IsAdjacent(Ingredients currentIng, Ingredients targetIng)
    {
        return Mathf.Abs(currentIng.x - targetIng.x) + Mathf.Abs(currentIng.y - targetIng.y) == 1;
    }
    //ProcessMatches

    private IEnumerator ProcessMatches(Ingredients currentIng, Ingredients targetIng)
    {
        yield return new WaitForSeconds(0.2f);

        if (CheckBoard())
        {
            StartCoroutine(ProcessTurnOnMatchBoard(true));
        }
        else
        {
            DoSwap(currentIng, targetIng);
        }

        isProcessingMove = false;
    }

    #endregion
}
