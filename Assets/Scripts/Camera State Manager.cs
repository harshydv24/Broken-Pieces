using System;
using UnityEngine;

public class CameraStateManager : MonoBehaviour
{
    [SerializeField] private QuestionsUI questionsUI;
    [SerializeField] private Player_CameraController player_CameraController;
    // [SerializeField] private GameObject HomeScreenCam;

    [SerializeField] private Vector3 HomeScreenCamPos;

    private bool IsCursorActive = false;

    private void Start()
    {
        Camera.main.transform.localPosition = HomeScreenCamPos;
        questionsUI.OnDialogEnabled += questionsUI_OnDialogEnabled;
        questionsUI.OnDialogDisabled += questionsUI_OnDialogDisabled;
        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (IsCursorActive)
            {
                HideCursor();
                player_CameraController.canLook = true;
                IsCursorActive = false;
            }
            else
            {
                ShowCursor();
                player_CameraController.canLook = false;
                IsCursorActive = true;
            }
        }
    }

    private void GameManager_OnGameStateChanged(object sender, EventArgs e)
    {
        if (GameManager.Instance.IsPlayingStarted())
        {
            UpdateCameraPos();
        }
    }

    private void questionsUI_OnDialogDisabled(object sender, EventArgs e)
    {
        player_CameraController.canLook = true;
        HideCursor();
    }

    private void questionsUI_OnDialogEnabled(object sender, EventArgs e)
    {
        player_CameraController.canLook = false;
        ShowCursor();
    }

    private void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    private void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void UpdateCameraPos()
    {
        Camera.main.transform.LeanMoveLocal(Vector3.zero, 0.1f).setEaseOutExpo();
    }
}
