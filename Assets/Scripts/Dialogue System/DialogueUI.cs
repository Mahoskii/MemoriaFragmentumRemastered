using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;
using System;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance;

    [Header("UI Elements")]
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private Image portrait;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private GameObject[] optionsButtons;
    [SerializeField] private TextMeshProUGUI[] optionsText;

    [Header("Text Variables")]
    [HideInInspector] public int sceneNumber;
    [HideInInspector] public int lineIndex;
    [HideInInspector] public bool isInDialogue;
    [HideInInspector] public bool areButtonsSet;

    [Header("Text Flow Control Bools")]
    private bool isTextPlaying;
    private bool skipTextPlay;

    public static Action<int> MovedToAnswer;

    public static int sceneCounter;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void ToggleDialogueBox(bool onOrOff)
    {
        dialogueBox.SetActive(onOrOff);
        if (onOrOff)
        {
            //AudioManager.Instance.StopMusic("BGmusic");
            //AudioManager.Instance.PlayMusic("Cutscene");
        }
        else
        {
            //AudioManager.Instance.StopMusic("Cutscene");
        }
    }
    public bool IsDialogueBoxActive()
    {
        if (dialogueBox.activeSelf)
        {
            isInDialogue = true;
            return true;
        }
        isInDialogue = false;
        return false;
    }
    public bool AreChoiceButtonsActive()
    {
        if (optionsButtons[0].activeSelf)
        {
            return true;
        }
        return false;
    }
    public void UpdateCharacterName(string nameText)
    {
        this.nameText.text = nameText;
    }
    public void UpdateCharacterPortrait(Sprite portrait)
    {
        this.portrait.sprite = portrait;
    }
    public void UpdateDialogue(DialogueLinesAndActions[] conversationArray, Action<int> NextDialogue)
    {
        // start displaying the text
        if (dialogueText.text != conversationArray[lineIndex].lineToDisplay && !isTextPlaying)
        {
            skipTextPlay = false;
            DisplayTextOverTime(conversationArray[lineIndex].lineToDisplay);//.Replace("{Player}", GetPlayerName.Instance.playerName));
            // move a character during a scene if needed
            // and change character rotation if needed
            if (conversationArray[lineIndex].doAction && (conversationArray[lineIndex].actionToPreform == ActionToPreform.MoveCharacter || conversationArray[lineIndex].actionToPreform == ActionToPreform.RotateCharacter))
            {
                //rotate character
                //move character
                DialogueActions.Instance.WhichAction(conversationArray[lineIndex].actionToPreform, conversationArray[lineIndex].movementInfo, conversationArray[lineIndex].rotationInfo);
            }
        }
        // fully display the text if the player wants to skip it playing on screen
        else if (Input.GetKeyDown("space") && dialogueText.text != conversationArray[lineIndex].lineToDisplay && isTextPlaying)
        {
            skipTextPlay = true;
            dialogueText.text = conversationArray[lineIndex].lineToDisplay;//.Replace("{Player}", GetPlayerName.Instance.playerName);
        }
        //when reaching the end of a line, check if we continue to the next line, or end the conversation
        else if (Input.GetKeyDown("space") && dialogueText.text == conversationArray[lineIndex].lineToDisplay && !isTextPlaying && !conversationArray[lineIndex].isPlayerChoice)
        {
            //clean the text
            CleanDialogue();
            //move to next line
            if (lineIndex < conversationArray.Length - 1)
            {
                lineIndex++;
                //check if there are options for the player to choose
                if (conversationArray[lineIndex].isPlayerChoice && !areButtonsSet)
                {
                    SetOptions(conversationArray[lineIndex].howManyOptions, conversationArray[lineIndex].options);
                    MovedToAnswer += NextDialogue;
                }
            }
            //end conversation
            else
            {
                sceneCounter++;
                CleanDialogue();
                ToggleDialogueBox(false);
                isInDialogue = false;
                //set objective if there is one
                //if (conversationArray[lineIndex].giveObjective && !ObjectiveGoalReached.Instance.objectiveActive)
                //{
                //    ObjectiveGoalReached.Instance.GetObjectiveInfo(conversationArray[lineIndex].objective.objective, conversationArray[lineIndex].objective.description);
                //}
                //preform an action if there is one
                if (conversationArray[lineIndex].doAction && conversationArray[lineIndex].actionToPreform != ActionToPreform.MoveCharacter && conversationArray[lineIndex].actionToPreform != ActionToPreform.RotateCharacter)
                {
                    DialogueActions.Instance.WhichAction(conversationArray[lineIndex].actionToPreform, conversationArray[lineIndex].movementInfo, conversationArray[lineIndex].rotationInfo);
                }
                lineIndex = 0;
                NextDialogue(1);
                NPCDialogue.Istalking?.Invoke(false);
            }

        }

    }

    //public void UpdateDialogue(string dialogueText, int lineIdex, DialogueLinesAndActions[] array, Action<int> NextDialogue, bool isPlayerChoise, bool doAction, ActionToPreform action, CharacterMove[] moves)
    public async void DisplayTextOverTime(string dialogueText)
    {
        isTextPlaying = true;
        foreach (char letter in dialogueText.ToCharArray())
        {
            if (skipTextPlay)
            {
                isTextPlaying = false;
                return;
            }
            else
            {
                this.dialogueText.text += letter;
                //AudioManager.Instance.PlaySFX("Speech2Short");
                await Task.Delay(100);
            }

        }
        isTextPlaying = false;
    }
    public async void DisplayTextOverTime(string dialogueText, int speed, TextMeshProUGUI displayText)
    {
        foreach (char letter in dialogueText.ToCharArray())
        {
            displayText.text += letter;
            //AudioManager.Instance.PlaySFX("Speech2Short");
            await Task.Delay(speed);
        }
    }
    private void CleanDialogue()
    {
        this.dialogueText.text = "";
    }

    public void SetOptions(int optionsNum, string[] optionArray)
    {
        for (int i = 0; i < optionsNum; i++)
        {
            optionsText[i].text = optionArray[i];
            optionsButtons[i].SetActive(true);
        }
        areButtonsSet = true;
    }

    public void UnSetOptions(int optionsNum)
    {
        for (int i = 0; i < optionsNum; i++)
        {
            optionsText[i].text = "";
            optionsButtons[i].SetActive(false);
        }
        lineIndex = 0;
    }

    public void OnAnswerChoice(int answer)
    {
        //AudioManager.Instance.PlaySFX("Button");
        CleanDialogue();
        MovedToAnswer?.Invoke(answer);
        areButtonsSet = false;
        MovedToAnswer = null;
    }

}
