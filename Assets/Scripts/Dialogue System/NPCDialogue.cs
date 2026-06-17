using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class NPCDialogue : MonoBehaviour, IDialogue
{
    private bool isPlayerNear;

    [Header("Dialogue")]
    public ScriptableDays[] allDialogues;
    private int nextDialogue;
    public int dayCounter;

    private void OnEnable()
    {
        //DayCounter.DoneOnDayEnd += ResetNextDialogue;
    }
    private void OnDisable()
    {
        //DayCounter.DoneOnDayEnd -= ResetNextDialogue;
    }

    void Update()
    {
        if (DialogueUI.Instance.IsDialogueBoxActive() && isPlayerNear && !DialogueScenes.IsInCutscene.Invoke()) //!ActivateBoard.MatchThreeStarted.Invoke() && !DialogueScenes.IsInCutscene.Invoke())
        {
            WhatToDisplay(dayCounter, nextDialogue); //DayCounter.Instance.currentDay, nextDialogue);

        }
        else if (Input.GetKeyDown("space") && allDialogues[dayCounter].Scenes[nextDialogue].Dialogue.Length > DialogueUI.Instance.lineIndex && allDialogues[dayCounter].Scenes[nextDialogue].Dialogue[DialogueUI.Instance.lineIndex].actionToPreform == ActionToPreform.MoveCharacter && !isPlayerNear)
        {
            isPlayerNear = true;
        }
        else if (Input.GetKeyDown(KeyCode.E) && isPlayerNear)
        {
            DialogueUI.Instance.ToggleDialogueBox(true);
        }

        if (!DialogueUI.Instance.IsDialogueBoxActive() && allDialogues[dayCounter].Scenes[nextDialogue].isAnswer) //[DayCounter.Instance.currentDay].Scenes[nextDialogue].isAnswer)
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
        if (DialogueUI.Instance.AreChoiceButtonsActive())
        {
            DialogueUI.Instance.UnSetOptions(allDialogues[dayCounter].Scenes[nextDialogue].Dialogue[DialogueUI.Instance.lineIndex].howManyOptions); //DayCounter.Instance.currentDay].Scenes[nextDialogue].Dialogue[DialogueUI.Instance.lineIndex].howManyOptions);
        }
        if (nextDialogue < allDialogues[dayCounter].Scenes.Length - 1) //DayCounter.Instance.currentDay].Scenes.Length - 1)
        {
            nextDialogue += moveTo;
        }
    }
    public void ResetNextDialogue()
    {
        nextDialogue = 0;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Players"))
        {
            isPlayerNear = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Players"))
        {
            isPlayerNear = false;
        }
    }
}
