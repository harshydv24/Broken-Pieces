using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class QuestionsPanelAnimation : MonoBehaviour
{
    [SerializeField] private QuestionsUI questionsUI;
    [SerializeField] private Transform LineTransform;
    private CanvasGroup canvasGroup;
    private Vector3 DefaultScale = new Vector3(0.8f, 0.8f, 1f);

    private void Start()
    {
        canvasGroup.alpha = 0f;
        LineTransform.localScale = new Vector3(0, 0, 0);
        questionsUI.OnDialogDisabled += questionsUI_OnDialogDisabled;
        questionsUI.OnDialogEnabled += questionsUI_OnDialogEnabled;
    }

    private void questionsUI_OnDialogDisabled(object sender, EventArgs e)
    {
        CloseUpAnimation();
    }

    private void questionsUI_OnDialogEnabled(object sender, EventArgs e)
    {
        OpenUpAnimation();
    }

    private void OpenUpAnimation()
    {
        canvasGroup.LeanAlpha(1f, 0.3f);
        transform.LeanScaleX(1f, 0.4f).setEaseOutExpo();
        transform.LeanScaleY(1f, 0.4f).setEaseOutExpo();
        LineTransform.LeanScaleY(1f, 0.5f).setEaseOutExpo();
    }

    private void CloseUpAnimation()
    {
        canvasGroup.LeanAlpha(0f, 0.3f);
        transform.LeanScaleX(DefaultScale.x, 0.4f).setEaseOutExpo();
        transform.LeanScaleY(DefaultScale.y, 0.4f).setEaseOutExpo();
        LineTransform.LeanScaleY(0f, 0.5f).setEaseOutExpo();
    }

}
