using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterResponseUI : MonoBehaviour
{
    [SerializeField] private GameObject CounterResponseUIVisual;
    [SerializeField] private GameObject CounterResponseButton;

    private void Start()
    {
        CounterResponseButton.SetActive(false);
        CounterResponseUIVisual.SetActive(false);
        Suspect_DialogSystem.Instance.CheckForResponce += Suspect_DialogSystem_CheckForResponce;
        ResponceCheckingSystem.Instance.HideResponceVerifySystem += ResponceCheckingSystem_HideResponceVerifySystem;
    }

    private void ResponceCheckingSystem_HideResponceVerifySystem(object sender, EventArgs e)
    {
        CounterResponseUIVisual.SetActive(false);
    }

    private void Suspect_DialogSystem_CheckForResponce(object sender, EventArgs e)
    {
        CounterResponseButton.SetActive(true);
    }

    public void Hide()
    {
        CounterResponseUIVisual.SetActive(!CounterResponseUIVisual.activeSelf);
        CounterResponseButton.SetActive(false);
    }


}
