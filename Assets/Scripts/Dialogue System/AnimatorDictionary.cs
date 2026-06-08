using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorDictionary : MonoBehaviour
{
    public static AnimatorDictionary Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    [SerializeField] private Animator[] charAnimArray;
    [SerializeField] private CharacterNames[] charNames;
    public Dictionary<string, Animator> charAnimDictionary;

    private void Start()
    {
        InitializeAnimDictionary();
    }

    private void InitializeAnimDictionary()
    {
        charAnimDictionary = new Dictionary<string, Animator>();

        for (int i = 0; i < charAnimArray.Length; i++)
        {
            charAnimDictionary.Add(charNames[i].ToString(), charAnimArray[i]);
        }
    }
}
