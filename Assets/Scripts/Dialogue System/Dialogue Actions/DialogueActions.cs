using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueActions : MonoBehaviour
{
    public static DialogueActions Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }


    public void WhichAction(ActionToPreform action, CharacterMove[] move, CharacterRotate[] rotations)
    {
        switch (action)
        {
            case ActionToPreform.StartMatch3:
                //ActivateBoard.ChangeGamePanelActiveState?.Invoke(true);
                break;

            case ActionToPreform.ChangeDay:
                //DayCounter.Instance.ChangeDay();
                break;

            case ActionToPreform.MoveCharacter:
                //ChangePosition.Instance.MoveCharacter(move);
                break;

            case ActionToPreform.ChangeScene:
                DialogueScenes.StartScene?.Invoke();
                break;

            case ActionToPreform.EndGame:
                //AudioManager.Instance.StopMusic("BGmusic");
                //AudioManager.Instance.StopMusic("Cutscene");
                //AudioManager.Instance.StopMusic("Match3");
                SceneManager.LoadSceneAsync("GameOver");
                break;

            case ActionToPreform.HelpTonyEnding:
                //AudioManager.Instance.StopMusic("BGmusic");
                //AudioManager.Instance.StopMusic("Cutscene");
                //AudioManager.Instance.StopMusic("Match3");
                SceneManager.LoadSceneAsync("TonyEnd");
                break;

            case ActionToPreform.HelpSunnyEnding:
                //AudioManager.Instance.StopMusic("BGmusic");
                //AudioManager.Instance.StopMusic("Cutscene");
                //AudioManager.Instance.StopMusic("Match3");
                SceneManager.LoadSceneAsync("SunnyEnd");
                break;

            case ActionToPreform.RotateCharacter:
                //rotate character
                ChangeRotation.Instance.ChangeCharRotation(rotations);
                break;

            default:
                Debug.Log("somethin' ain't right");
                break;
        }
    }
}
