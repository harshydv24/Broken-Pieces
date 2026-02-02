using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeScreenUI : MonoBehaviour
{
    private void Start()
    {
        gameObject.SetActive(true);
        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;
    }

    private void GameManager_OnGameStateChanged(object sender, EventArgs e)
    {
        if (GameManager.Instance.IsHomeScreenActive())
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
