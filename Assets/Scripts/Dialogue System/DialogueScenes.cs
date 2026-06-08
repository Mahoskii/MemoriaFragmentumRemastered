using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueScenes : MonoBehaviour, IDialogue
{


    [Header("Delegates")]
    public static Func<bool> IsInCutscene;
    public static Action StartScene;

    [Header("Dialogue")]
    public ScriptableDays[] allDialogues;
    private int nextDialogue;
    private bool isCutscene;
    public int currentDay;

    private void OnEnable()
    {
        IsInCutscene += IsScene;
        //DayCounter.DoneOnDayEnd += ResetNextDialogue;
        StartScene += StartCutscene;
    }
    private void OnDisable()
    {
        IsInCutscene -= IsScene;
        //DayCounter.DoneOnDayEnd -= ResetNextDialogue;
        StartScene -= StartCutscene;
    }


    private void Update()
    {
        if (DialogueUI.Instance.IsDialogueBoxActive() && isCutscene)// && !ActivateBoard.MatchThreeStarted.Invoke())
        {

            WhatToDisplay(currentDay, nextDialogue);// DayCounter.Instance.currentDay, nextDialogue);
        }
        if (!DialogueUI.Instance.IsDialogueBoxActive() && allDialogues[currentDay].Scenes[nextDialogue].isAnswer) //[DayCounter.Instance.currentDay].Scenes[nextDialogue].isAnswer)
        {
            NextDialogue(1);
        }
    }

    public void WhatToDisplay(int day, int sceneNumber)
    {
        DialogueUI.Instance.UpdateDialogue(allDialogues[day].Scenes[sceneNumber].Dialogue, NextDialogue);
        DialogueUI.Instance.UpdateCharacterName(allDialogues[day].Scenes[sceneNumber].Dialogue[DialogueUI.Instance.lineIndex].characterName.ToString());//.Replace("Player", GetPlayerName.Instance.playerName));
        DialogueUI.Instance.UpdateCharacterPortrait(DialogueSprites.Instance.spriteDictionary[allDialogues[day].Scenes[sceneNumber].Dialogue[DialogueUI.Instance.lineIndex].characterPortrait.ToString()]);
    }

    public void NextDialogue(int moveTo = 0)
    {
        //if (DialogueUI.Instance.AreChoiceButtonsActive())
        //{
        //    DialogueUI.Instance.UnSetOptions(allDialogues[DayCounter.Instance.currentDay].Scenes[nextDialogue].Dialogue[DialogueUI.Instance.lineIndex].howManyOptions);
        //}
        //if (nextDialogue < allDialogues[DayCounter.Instance.currentDay].Scenes.Length - 1)
        //{
        //    nextDialogue += moveTo;
        //}
        //if (ActivateBoard.MatchThreeStarted.Invoke())
        //{
        //    AudioManager.Instance.StopMusic("Cutscene");
        //    AudioManager.Instance.PlayMusic("Match3");
        //    isCutscene = false;

        //}
        //else if(!ActivateBoard.MatchThreeStarted.Invoke())
        //{
        //    AudioManager.Instance.PlayMusic("Cutscene");
        //    isCutscene = true;
        //}
    }
    public void ResetNextDialogue()
    {
        nextDialogue = 0;
    }

    public void StartCutscene()
    {
        //if (!ActivateBoard.MatchThreeStarted.Invoke() && !DialogueUI.Instance.IsDialogueBoxActive())
        //{
        //    DialogueUI.Instance.ToggleDialogueBox(true);
        //    AudioManager.Instance.StopMusic("BGmusic");
        //    AudioManager.Instance.PlayMusic("Cutscene");
        //    isCutscene = true;
        //}
        DialogueUI.Instance.ToggleDialogueBox(true);
    }

    public bool IsScene()
    {
        if (isCutscene)
        {
            return true;
        }
        return false;
    }
}
