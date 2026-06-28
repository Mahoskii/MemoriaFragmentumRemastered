using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using System;
using UnityEngine.UI;
using DG.Tweening;

public class DayUI : MonoBehaviour
{
    [SerializeField] private GameObject dayPanel;
    [SerializeField] private Image panelImage;
    [SerializeField] private GameObject dayTextParent;
    [SerializeField] private TextMeshProUGUI dayText;
    private List<string> dayNumbers = new List<string> { "25.2.3056 \n The day of", "07.02.3056 \n 3 weeks before", "10.02.3056 \n 2.5 weeks before", "14.02.3056 \n 2 weeks before", "18.02.3056 \n 1 week before", "25.2.3056 \n 12:17PM \n The day of", "25.2.3056 \n 21:35PM \n The day of", "25.2.3056 \n 23:13PM \n The day of", "25.2.3056 \n 23:47PM \n The day of" };
    private bool updateText;
    private bool clearText;
    public static Action DayPanelToggel;
    public static Action StartScene;
    public static Action TextCleared;
    public static Func<bool> IsDayChanging;
    //void Start()
    //{
    //    ClearText();
    //    OnDayStart();
    //}

    private void OnEnable()
    {
        DayPanelToggel += OnDayStart;
        StartScene += OnSceneStart;
        IsDayChanging += IsDayPanelActive;
        TextCleared += ClearText;
    }
    private void OnDisable()
    {
        DayPanelToggel -= OnDayStart;
        StartScene += OnSceneStart;
        IsDayChanging -= IsDayPanelActive;
        TextCleared -= ClearText;
    }

    void Update()
    {
        if (IsDayPanelActive() && IsDayTextActive() && updateText)
        {
            UpdateDayText(dayNumbers[DayCounter.Instance.currentDay]);
        } 
        else if (!IsDayTextActive() && clearText)
        {
            ClearText();
        }   
    }

    private async void OnDayStart()
    {
        ToggleDayPanel(true);
        await Task.Delay(1500);
        ToggleDayText(true);
        //ActorsLocationChanger.ChangeLocation?.Invoke();
        await Task.Delay(4500);
        Tween textFade = dayText.DOFade(0, 2f).SetEase(Ease.Linear).SetAutoKill(false);
        await textFade.AsyncWaitForCompletion();
        ToggleDayText(false);
        await Task.Delay(1000);
        Tween panelFade = panelImage.DOFade(0, 3f).SetEase(Ease.Linear).SetAutoKill(false);
        await panelFade.AsyncWaitForCompletion();
        ToggleDayPanel(false);
        textFade.Rewind();
        panelFade.Rewind();
        DialogueScenes.StartScene?.Invoke();

    }

    private async void OnSceneStart()
    {
        //Tween preparePanel = panelImage.DOFade(0, 0f).SetAutoKill(false);
        //ToggleDayPanel(true);
        //Tween panelFadeIn = panelImage.DOFade(255, 1f).SetEase(Ease.Linear).SetAutoKill(false);
        //await panelFadeIn.AsyncWaitForCompletion();
        //Tween panelFadeOut = panelImage.DOFade(0, 1f).SetEase(Ease.Linear).SetAutoKill(false);
        //await panelFadeOut.AsyncWaitForCompletion();
        //ToggleDayPanel(false);
        //panelFadeIn.Rewind();
        //panelFadeOut.Rewind();
        //preparePanel.Rewind();
        ToggleDayPanel(true);
        //ActorsLocationChanger.ChangeLocation?.Invoke();
        await Task.Delay(1000);
        ToggleDayPanel(false);
    }
    private void ToggleDayPanel(bool onOrOff)
    {
        dayPanel.SetActive(onOrOff);
    }
    private void ToggleDayText(bool onOrOff)
    {
        dayTextParent.SetActive(onOrOff);
    }

    private bool IsDayPanelActive()
    {
        if (dayPanel.activeSelf)
        {
            return true;
        }
        return false;
    }
    private bool IsDayTextActive()
    {
        if (dayTextParent.activeSelf)
        {
            return true;
        }
        return false;
    }

    private void UpdateDayText(string dayText)
    {
        DialogueUI.Instance.DisplayTextOverTime(dayText, 75, this.dayText);
        updateText = false;
        clearText = true;
    }
    private void ClearText()
    {
        dayText.text = "";
        updateText = true;
        clearText = false;
    }
}
