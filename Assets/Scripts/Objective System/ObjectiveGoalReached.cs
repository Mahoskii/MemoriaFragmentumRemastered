using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveGoalReached : MonoBehaviour
{
    public static ObjectiveGoalReached Instance;
    [HideInInspector] public bool objectiveActive;
    [HideInInspector] public GameObjective objectiveName;
    [HideInInspector] public string objectiveDescription;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    public void GetObjectiveInfo(GameObjective objectiveName, string objectiveDescription)
    {
        objectiveActive = true;
        this.objectiveName = objectiveName;
        this.objectiveDescription = objectiveDescription;

    }
}
