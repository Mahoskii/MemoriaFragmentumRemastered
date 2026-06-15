using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueLinesAndActions
{
    [Header("Speaker Information")]
    public CharacterNames characterName;
    public CharacterPortraitsTags characterPortrait;
    [TextArea(3, 10)]
    public string lineToDisplay;

    [Header("Dialogue Options")]
    public bool isPlayerChoice;
    public int howManyOptions;
    [TextArea(3, 10)]
    public string[] options;

    [Header("Actions Information")]
    public bool doAction;
    public ActionToPreform actionToPreform;
    public CharacterMove[] movementInfo;
    public CharacterRotate[] rotationInfo;

    [Header("Objective Information")]
    public bool giveObjective;
    //public PlayerObjective objective;
}

public enum CharacterPortraitsTags
{
    Player,
    NPC1,
    NPC2,
    NPC3
    //PlayerNeutral,
    //PlayerHappy,
    //PlayerSerious,
    //PlayerSmile,
    //PlayerShy,
    //O1R47,
    //AshTalk,
    //AshHappy,
    //AshSerious,
    //AshSad,
    //RogawaSmile,
    //RogawaHappy,
    //RogawaTalk,
    //RogawaSerious,
    //SunnySad,
    //SunnySerious,
    //SunnyMad,
    //SunnyFacepalm,
    //SunnySmile,
    //TonyNormal,
    //TonySmile,
    //TonyShy,
    //TonyBored,
    //TonySad,
    //TonyDerp,
    //ToyoNormal,
    //ToyoTalk,
    //ToyoSad,
    //ToyoHappy,
    //AlienAngry,
    //AlienConfused,
    //AlienNormal,
    //HumanAngry,
    //HumanConfused,
    //HumanNormal,
    //RobotAngry,
    //RobotConfused,
    //RobotNormal
}
public enum CharacterNames
{
    Player,
    NPC1,
    NPC2,
    NPC3
    //Player,
    //O1R47,
    //Ash,
    //Rogawa,
    //Sunny,
    //Tony,
    //Toyo,
    //AlienNPC,
    //HumanNPC,
    //RobotNPC,

}
public enum ActionToPreform
{
    NoAction = 0,
    //StartMatch3,
    //ChangeDay,
    MoveCharacter = 1,
    //ChangeScene,
    //EndGame,
    //HelpTonyEnding,
    //HelpSunnyEnding,
    RotateCharacter = 2
}
