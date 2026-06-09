using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformDictionary : MonoBehaviour
{
    public static TransformDictionary Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    [SerializeField] private Transform[] charTransformArray;
    [SerializeField] private CharacterNames[] charNames;
    public Dictionary<string, Transform> charTransformDictionary;

    private void Start()
    {
        InitializeTransformDictionary();
    }

    private void InitializeTransformDictionary()
    {
        charTransformDictionary = new Dictionary<string, Transform>();

        for (int i = 0; i < charTransformArray.Length; i++)
        {
            charTransformDictionary.Add(charNames[i].ToString(), charTransformArray[i]);
        }
    }
}
