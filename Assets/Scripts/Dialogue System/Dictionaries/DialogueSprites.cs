using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSprites : MonoBehaviour
{
    public static DialogueSprites Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    [SerializeField]private Sprite[] spriteArray;
    [SerializeField] private CharacterPortraitsTags[] spriteNames;
    public Dictionary<string, Sprite> spriteDictionary;

    private void Start()
    {
        InitializeSpritesDictionary();
    }

    private void InitializeSpritesDictionary()
    {
        spriteDictionary = new Dictionary<string, Sprite>();

        for (int i = 0; i < spriteArray.Length; i++)
        {
            spriteDictionary.Add(spriteNames[i].ToString(), spriteArray[i]);
        }
    }
}
