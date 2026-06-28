using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DayCounter : MonoBehaviour
{
    public static DayCounter Instance;

    [Header("Day")]
    [HideInInspector] public int currentDay;
    public static Action DoneOnDayEnd;

    //public ScriptableDayData dayInfo = DayCounter.Instance.DaySceneList[DayCounter.Instance.currentDay];

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.N))
    //    {
    //        ChangeDay();
    //    }
    //}

    public void ChangeDay()
    {
        if (!ObjectiveGoalReached.Instance.objectiveActive)
        {
            currentDay++;
            DayUI.DayPanelToggel?.Invoke();
            DoneOnDayEnd?.Invoke();
        }
    }
}
