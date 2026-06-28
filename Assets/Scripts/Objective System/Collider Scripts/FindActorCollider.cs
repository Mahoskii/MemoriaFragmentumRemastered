using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindActorCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Players") && ObjectiveGoalReached.Instance.objectiveName == GameObjective.FindActors && ObjectiveGoalReached.Instance.objectiveActive)
        {
            //set active objective to false
            ObjectiveGoalReached.Instance.objectiveActive = false;
            //fade to black
            DayUI.StartScene?.Invoke();
            //move player to correct position
            //ActorsLocationChanger.PlayerMoved?.Invoke(new Vector3(9, 12.7f, 0));
            //unfade to black
            //start cutscene
            DialogueScenes.StartScene?.Invoke();
        }
    }
}
