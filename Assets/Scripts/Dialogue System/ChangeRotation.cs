using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeRotation : MonoBehaviour
{
    public static ChangeRotation Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    public void ChangeCharRotation(CharacterRotate[] rotations)
    {
       foreach(CharacterRotate rotate in rotations)
       {
            //Look Into Tuples!
            ActorSpriteDictionary.Instance.actorSpriteDictionary[rotate.charToRotate.ToString()].sprite = RotationSpritesDictionary.Instance.CharRotationDictionary[(rotate.charToRotate.ToString(), rotate.position.ToString())];
       }
    }
}
