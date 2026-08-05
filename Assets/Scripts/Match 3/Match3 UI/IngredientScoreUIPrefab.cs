using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IngredientScoreUIPrefab : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI ingredientName;
    [SerializeField] public Image ingredientImage;

    public IngredientScoreUIPrefab(string ingredientName, Sprite ingredientImage, int ingPoints, int ingGoal)
    {
        this.ingredientName.text = $"{ingredientName}: {ingPoints} / {ingGoal}";
        this.ingredientImage.sprite = ingredientImage;
    }

    public void SetIngGoalComp(string ingredientName, Sprite ingredientImage, int ingPoints, int ingGoal)
    {
        this.ingredientName.text = $"{ingredientName}: {ingPoints} / {ingGoal}";
        this.ingredientImage.sprite = ingredientImage;
    }

    public void UpdateGoalComp(string ingredientName, int ingPoints, int ingGoal)
    {
        this.ingredientName.text = $"{ingredientName}: {ingPoints} / {ingGoal}";
    }
}
