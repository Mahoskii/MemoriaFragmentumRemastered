using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectiveUI : MonoBehaviour
{
    [SerializeField] private GameObject objectivePanel;
    [SerializeField] private TextMeshProUGUI objectiveText;

    void Update()
    {
        if (ObjectiveGoalReached.Instance.objectiveActive && !IsObjectivePanelActive())
        {
            objectivePanel.SetActive(true);
            SetObjectiveText();
        }
        else if (!ObjectiveGoalReached.Instance.objectiveActive && IsObjectivePanelActive())
        {
            ToggleObjectivePanel(false);
        }
    }

    private void ToggleObjectivePanel(bool onOroff)
    {
        objectivePanel.SetActive(onOroff);
    }
    private bool IsObjectivePanelActive()
    {
        if (objectivePanel.activeSelf)
        {
            return true;
        }
        return false;
    }

    private void SetObjectiveText()
    {
        objectiveText.text = ObjectiveGoalReached.Instance.objectiveDescription;
    }
}
