using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class LevelScoreUIUpdater : MonoBehaviour
{
    [SerializeField] private ScriptableLevelData[] lvlData;
    [SerializeField] private List<IngredientUIInfo> ingredientSprites;
    [SerializeField] private GameObject[] ingUIComponent;
    private Dictionary<string, Sprite> ingredientSpritesDictionary;

    public TMP_Text movesTxt;

    private bool doUpdate;

    private void OnEnable()
    {
        SetDictionary();
        SetIngLvlUIElements();
        doUpdate = true;
    }
    private void OnDisable()
    {
        doUpdate = false;

        TurnUIElementOff();
    }


    void Update()
    {
        if (doUpdate)
        {
            movesTxt.text = $"Moves Left: {GameTurnController.Instance.moves.ToString()}";
            UpdateIngLvlUIElements();
        }
    }

    public void SetDictionary()
    {
        ingredientSpritesDictionary = new Dictionary<string, Sprite>();
        string[] ingNames = new string[ingredientSprites.Count];
        for (int i = 0; i < ingredientSprites.Count; i++)
        {
            ingNames[i] = ingredientSprites[i].ingName;
        }
        for (int i = 0; i < lvlData[GameTurnController.Instance.currentLvl].lvlIngredientsList.Length; i++)
        {
            int index = Array.IndexOf(ingNames, lvlData[GameTurnController.Instance.currentLvl].lvlIngredientsList[i].ToString());
            ingredientSpritesDictionary.Add(ingredientSprites[index].ingName, ingredientSprites[index].ingSprite);

        }
    }

    private void SetIngLvlUIElements()
    {
        for (int i = 0; i < lvlData[GameTurnController.Instance.currentLvl].lvlIngredientsList.Length; i++)
        {
            if (lvlData[GameTurnController.Instance.currentLvl].lvlIngredientsList.Length > ingUIComponent.Length)
            {
                Debug.Log("please make more components");
                return;
            }
            IngredientScoreUIPrefab component = ingUIComponent[i].GetComponent<IngredientScoreUIPrefab>();
            component.SetIngGoalComp(lvlData[GameTurnController.Instance.currentLvl].lvlIngredientsList[i].ToString(), ingredientSpritesDictionary[lvlData[GameTurnController.Instance.currentLvl].lvlIngredientsList[i].ToString()], 0, GameTurnController.Instance.goal[i]);
            ingUIComponent[i].SetActive(true);
        }
    }

    private void TurnUIElementOff()
    {
        for (int i = 0; i < ingUIComponent.Length; i++)
        {
            if (ingUIComponent[i].activeSelf == true)
            {
                ingUIComponent[i].SetActive(false);
            }
        }
    }

    private void UpdateIngLvlUIElements()
    {
        for (int i = 0; i < lvlData[GameTurnController.Instance.currentLvl].lvlIngredientsList.Length; i++)
        {
            IngredientScoreUIPrefab component = ingUIComponent[i].GetComponent<IngredientScoreUIPrefab>();
            if (GameTurnController.Instance.ScorePerIngredient[lvlData[GameTurnController.Instance.currentLvl].lvlIngredientsList[i].ToString()] < GameTurnController.Instance.goal[i])
            {
                component.UpdateGoalComp(lvlData[GameTurnController.Instance.currentLvl].lvlIngredientsList[i].ToString(), GameTurnController.Instance.ScorePerIngredient[lvlData[GameTurnController.Instance.currentLvl].lvlIngredientsList[i].ToString()], GameTurnController.Instance.goal[i]);
            }
            else if (GameTurnController.Instance.ScorePerIngredient[lvlData[GameTurnController.Instance.currentLvl].lvlIngredientsList[i].ToString()] >= GameTurnController.Instance.goal[i])
            {
                GameTurnController.Instance.ScorePerIngredient[lvlData[GameTurnController.Instance.currentLvl].lvlIngredientsList[i].ToString()] = GameTurnController.Instance.goal[i];
                component.UpdateGoalComp(lvlData[GameTurnController.Instance.currentLvl].lvlIngredientsList[i].ToString(), GameTurnController.Instance.ScorePerIngredient[lvlData[GameTurnController.Instance.currentLvl].lvlIngredientsList[i].ToString()], GameTurnController.Instance.goal[i]);
            }
        }
    }
}
