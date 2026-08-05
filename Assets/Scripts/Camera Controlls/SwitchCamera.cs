using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwitchCamera : MonoBehaviour
{
    [SerializeField] private GameObject mainCam, match3Cam;
    public static Action MainCamActive;
    public static Action Match3CamActive;

    private void OnEnable()
    {
        MainCamActive += ActivateMainCam;
        Match3CamActive += ActivateMatch3Cam;
    }
    private void OnDisable()
    {
        MainCamActive -= ActivateMainCam;
        Match3CamActive -= ActivateMatch3Cam;
    }

    private void Start()
    {
        match3Cam.SetActive(false);
    }

    private void ActivateMainCam()
    {
        match3Cam.SetActive(false);
        mainCam.SetActive(true);
    }

    private void ActivateMatch3Cam()
    {
        match3Cam.SetActive(true);
        mainCam.SetActive(false);
    }

}
