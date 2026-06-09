using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePosition : MonoBehaviour
{
    public static ChangePosition Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void MoveCharacter(CharacterMove[] moves)
    {
        var chainMovement = DOTween.Sequence();
        foreach (CharacterMove move in moves)
        {
            for (int i = 0; i < move.locations.Length; i++)
            {
                //AnimatorDictionary.Instance.charAnimDictionary[move.charToMove.ToString()].CrossFade("Walk", 0.2f);
                chainMovement.Append(TransformDictionary.Instance.charTransformDictionary[move.charToMove.ToString()].DOMove(move.locations[i], move.duration[i])).SetEase(Ease.Linear);
            }
            //chainMovement.OnComplete(() => AnimatorDictionary.Instance.charAnimDictionary[move.charToMove.ToString()].CrossFade("Idle", 0.2f));
        }


    }
}
