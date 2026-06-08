using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDialogue
{
    void WhatToDisplay(int day, int sceneNumber);
    void NextDialogue(int moveTo);
    void ResetNextDialogue();
}
