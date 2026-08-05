using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GameTurnController : MonoBehaviour
{
    public static GameTurnController Instance; // static reference
    [HideInInspector] public int currentLvl;
    public GameObject backGroundPanel; // grey background
    public GameObject victoryPanel;
    public GameObject losePanel;

    [SerializeField] private ScriptableLevelData[] lvlData;
    public Dictionary<string, int> ScorePerIngredient;
    [HideInInspector] public int[] goal; //the amount of points you need to take to win.
    [HideInInspector] public int moves; // the amount of turns you can take
    [HideInInspector] public int[] points; // the current points you have earned.

    [HideInInspector] public bool isGameEnded;

    public static Action gameFinished;


    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        ActivateBoard.ScoreInitialized += InitializeLvlGoalUI;
    }

    private void OnDisable()
    {
        ActivateBoard.ScoreInitialized -= InitializeLvlGoalUI;
    }

    public void InitializeLvlGoalUI()
    {
        goal = new int[lvlData[currentLvl].lvlIngredientsList.Length];
        points = new int[lvlData[currentLvl].lvlIngredientsList.Length];
        moves = lvlData[currentLvl].movesForThisLvl;
        ScorePerIngredient = new Dictionary<string, int>();
        for (int i = 0; i < lvlData[currentLvl].lvlIngredientsList.Length; i++)
        {
            goal[i] = lvlData[currentLvl].IngredientScore[i];
            ScorePerIngredient.Add(lvlData[currentLvl].lvlIngredientsList[i].ToString(), points[i]);
        }
    }

    public void ProcessTurn(string ingredientName, int pointsToGain, bool subtractMoves)
    {
        ScorePerIngredient[ingredientName] += pointsToGain;
        if (subtractMoves)
        {
            moves--;
        }
        if (CheckIfGoalWasReached())
        {
            //you've won the game
            isGameEnded = true;
            //Display a victory screen
            backGroundPanel.SetActive(true);
            victoryPanel.SetActive(true);
            gameFinished?.Invoke();
            return;
        }
        if (moves == 0)
        {
            //lose the game
            isGameEnded = true;
            //Display a defeat screen
            backGroundPanel.SetActive(true);
            losePanel.SetActive(true);
            gameFinished?.Invoke();
            return;
        }
    }

    public string ChooseRandomTag()
    {
        int randomIndex = UnityEngine.Random.Range(0, lvlData[currentLvl].lvlIngredientsList.Length);
        string randomTag = lvlData[currentLvl].lvlIngredientsList[randomIndex].ToString();
        return randomTag;
    }
    private bool CheckIfGoalWasReached()
    {
        int howManyCompleted = 0;
        for (int i = 0; i < lvlData[currentLvl].lvlIngredientsList.Length; i++)
        {
            if (ScorePerIngredient[lvlData[currentLvl].lvlIngredientsList[i].ToString()] >= goal[i])
            {
                howManyCompleted++;
            }
        }
        if (howManyCompleted == lvlData[currentLvl].lvlIngredientsList.Length)
        {
            return true;
        }

        return false;
    }
    //attached to a button to close the panel on game end
    public void OnGameWin()
    {
        if (currentLvl < lvlData.Length - 1)
        {
            currentLvl++;
        }
        backGroundPanel.SetActive(false);
        victoryPanel.SetActive(false);
        ActivateBoard.ChangeGamePanelActiveState?.Invoke(false);
        //SwitchCamera.MainCamActive?.Invoke();
        //AudioManager.Instance.StopMusic("Match3");
        //AudioManager.Instance.PlayMusic("BGmusic");
        //if (DialogueScenes.IsInCutscene.Invoke())
        //{
        //    DialogueUI.Instance.ToggleDialogueBox(true);
        //}
        DialogueScenes.StartScene?.Invoke();
    }
    public void OnGameLose()
    {
        backGroundPanel.SetActive(false);
        losePanel.SetActive(false);
        ActivateBoard.ChangeGamePanelActiveState?.Invoke(false);
        ActivateBoard.ChangeGamePanelActiveState(true);
    }
}
