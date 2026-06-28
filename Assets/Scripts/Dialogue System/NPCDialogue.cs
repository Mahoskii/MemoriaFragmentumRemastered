using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using System;
using UnityEngine;

public class NPCDialogue : MonoBehaviour, IDialogue
{
    private bool isPlayerNear;
    [SerializeField] private string idName;
    private bool isTalking;

    [Header("Dialogue")]
    public ScriptableDays[] allDialogues;
    private int nextDialogue;
    public int dayCounter;

    public static Action<bool> Istalking;

    private void OnEnable()
    {
        //DayCounter.DoneOnDayEnd += ResetNextDialogue;
        Istalking += ToggleTalking;
    }
    private void OnDisable()
    {
        //DayCounter.DoneOnDayEnd -= ResetNextDialogue;
        Istalking -= ToggleTalking;
    }

    void Update()
    {
        if (DialogueUI.Instance.IsDialogueBoxActive() && (isPlayerNear || IsTalking()) && !DialogueScenes.IsInCutscene.Invoke()) //!ActivateBoard.MatchThreeStarted.Invoke() && !DialogueScenes.IsInCutscene.Invoke())
        {
            WhatToDisplay(dayCounter, nextDialogue); //DayCounter.Instance.currentDay, nextDialogue);

        }
        else if (Input.GetKeyDown(KeyCode.E) && isPlayerNear)
        {
            DialogueUI.Instance.ToggleDialogueBox(true);
            isTalking = true;
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
        if (collision.CompareTag("Players"))// && this.gameObject.tag == idName)//this.gameObject.CompareTag(idName))
        {
            isPlayerNear = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Players") && !IsTalking())
        {
            isPlayerNear = false;

        }
    }

    public void ToggleTalking(bool YesorNo)
    {
        isTalking = YesorNo;
    }
    public bool IsTalking()
    {
        if (isTalking)
        {
            return true;
        }
        return false;
    }


}
