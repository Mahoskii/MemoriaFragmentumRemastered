using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GetPlayerName : MonoBehaviour
{
    public static GetPlayerName Instance;
    [SerializeField] private TMP_InputField inputField;
    [HideInInspector] public string playerName;
    [SerializeField] private GameObject namePanel;
    [HideInInspector] public bool isActive;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        isActive = true;
    }
    public void GetName()
    {
        if (inputField.text.ToCharArray().Length > 0)
        {
            playerName = inputField.text;

        }
        else
        {
            playerName = "Bart Ender";
        }
        namePanel.SetActive(false);
        DayUI.TextCleared?.Invoke();
        DayUI.DayPanelToggel?.Invoke();
        isActive = false;
    }
}
