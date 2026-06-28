using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBarCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Players") && ObjectiveGoalReached.Instance.objectiveName == GameObjective.GoToWork && ObjectiveGoalReached.Instance.objectiveActive)
        {
            //set active objective to false
            ObjectiveGoalReached.Instance.objectiveActive = false;
            //fade to black
            DayUI.StartScene?.Invoke();
            //move player to correct position
            //ActorsLocationChanger.PlayerMoved?.Invoke(new Vector3(5.47f, 61.1f, 0));
            //unfade to black
            //start cutscene
            DialogueScenes.StartScene?.Invoke();
        }
    }
}
