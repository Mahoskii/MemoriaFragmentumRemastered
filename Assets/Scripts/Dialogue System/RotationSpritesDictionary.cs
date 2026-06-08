using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationSpritesDictionary : MonoBehaviour
{
    [Header("Rotation sprites array")]
    [SerializeField] private Sprite[] RotationSprites;
    [Header("Rotation sprites dictionary")]
    public Dictionary<(string Names, string Direction), Sprite> CharRotationDictionary;

    [SerializeField] private CharacterNames[] charNames;
    [SerializeField] private FacingPositions[] charPosition;

    public static RotationSpritesDictionary Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    private void Start()
    {
        InitializeRotationDictionary();
    }

    private void InitializeRotationDictionary()
    {
        CharRotationDictionary = new Dictionary<(string Names, string Direction), Sprite>();

        for(int i = 0, j = 0, k = 0; i < RotationSprites.Length; i++)
        {
            CharRotationDictionary.Add((Names: charNames[j].ToString(), Direction: charPosition[k].ToString()), RotationSprites[i]);
            if(k < charPosition.Length)
            {
                k++;
                if(k == charPosition.Length && j < charNames.Length)
                {
                    k = 0;
                    j++;
                }
            }
        }
    }

}
