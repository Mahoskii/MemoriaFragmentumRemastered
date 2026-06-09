using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorSpriteDictionary : MonoBehaviour
{
    public static ActorSpriteDictionary Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    [SerializeField] private SpriteRenderer[] spriteHolderArray;
    [SerializeField] private CharacterNames[] charNames;
    public Dictionary<string, SpriteRenderer> actorSpriteDictionary;

    private void Start()
    {
        InitializeActorSpriteDictionary();
    }

    private void InitializeActorSpriteDictionary()
    {
        actorSpriteDictionary = new Dictionary<string, SpriteRenderer>();

        for (int i = 0; i < spriteHolderArray.Length; i++)
        {
            actorSpriteDictionary.Add(charNames[i].ToString(), spriteHolderArray[i]);
        }
    }
}
