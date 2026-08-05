using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredients : MonoBehaviour
{
    public IngredientType ingredientType;

    public int x;
    public int y;

    public bool isMatched;
    private Vector3 currentPos;
    private Vector3 targetPos;

    public bool isMoving;

    public Ingredients(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public void SetIndicies(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    //MoveToTarget
    public void MoveToTarget(Vector3 targetPos)
    {
        StartCoroutine(MoveCoroutine(targetPos));
    }
    //Move Coroutine
    private IEnumerator MoveCoroutine(Vector3 targetPos)
    {
        isMoving = true;
        float duration = 0.2f;
        Vector3 startPosition = transform.position;
        float elaspedTime = 0f;

        while (elaspedTime < duration)
        {
            float t = elaspedTime / duration;
            transform.position = Vector3.Lerp(startPosition, targetPos, t);
            elaspedTime += Time.deltaTime;

            yield return null;
        }
        transform.position = targetPos;
        isMoving = false;
    }
}
public enum IngredientType
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
