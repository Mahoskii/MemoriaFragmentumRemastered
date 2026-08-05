using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateBoard : MonoBehaviour
{
    [SerializeField] private GameObject boardPanel;
    [SerializeField] private GameObject boardUI;
    public static Action<bool> ChangeGamePanelActiveState;
    public static Action ScoreInitialized;
    public static Func<bool> MatchThreeStarted;
    private void OnEnable()
    {
        ChangeGamePanelActiveState += TurnGameBoardOnorOff;
        MatchThreeStarted += IsGamePanelOpen;
    }
    private void OnDisable()
    {
        ChangeGamePanelActiveState -= TurnGameBoardOnorOff;
        MatchThreeStarted -= IsGamePanelOpen;
    }

    //for testing only. in the game itself have the ChangeGamePanelActiveState event called to start the board.
    //void Update()
    //{
    //    if (Input.GetKeyDown("space"))
    //    {
    //        TurnGameBoardOnorOff(true);
    //    }
    //}

    private void TurnGameBoardOnorOff(bool isGameStart)
    {
        GameTurnController.Instance.isGameEnded = false;
        boardPanel.SetActive(isGameStart);
        if (isGameStart)
        {
            //AudioManager.Instance.StopMusic("BGmusic");
            //AudioManager.Instance.PlayMusic("Match3");
            //SwitchCamera.Match3CamActive?.Invoke();
            ScoreInitialized?.Invoke();
        }
        boardUI.SetActive(isGameStart);
    }
    public bool IsGamePanelOpen()
    {
        if (boardPanel.activeSelf)
        {
            return true;
        }
        return false;
    }
}
