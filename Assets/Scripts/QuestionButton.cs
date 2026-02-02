using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;
using UnityEngine.UI;

public class QuestionButton : MonoBehaviour
{
    public static QuestionButton Instance {get; private set;}
    public event EventHandler OnQuestionSelected;

    [SerializeField] private TextMeshProUGUI questionTextUI;
    [Header("Effect Elements")]
    [SerializeField] private Transform CorrectIcon;
    [SerializeField] private Color DiscardTextColor;

    private int CurrentQuestionIndex;
    private Player_DialogSystem player_DialogSystem;
    private Suspect_DialogSystem suspect_DialogSystem;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        transform.GetComponent<Button>().interactable = true;
        player_DialogSystem = FindFirstObjectByType<Player_DialogSystem>();
        suspect_DialogSystem = FindFirstObjectByType<Suspect_DialogSystem>();
        CorrectIcon.gameObject.SetActive(false);
    }

    public void SetQuestionText(int questionIndex, string question)
    {
        CurrentQuestionIndex = questionIndex;
        questionTextUI.text = question;
    }

    public void OnClick()
    {
        if(!Suspect_DialogSystem.Instance.GetCanAskQuestions()) return;

        OnQuestionSelected?.Invoke(this, EventArgs.Empty);
        suspect_DialogSystem.IsMaskedBroken = false;
        player_DialogSystem.QuestionSelected(CurrentQuestionIndex);
        UpdateQuestionTextUI();
        transform.GetComponent<Button>().interactable = false;
    }

    private void UpdateQuestionTextUI()
    {
        CanvasGroup correctIconCanvasGroup = CorrectIcon.GetComponent<CanvasGroup>();
        correctIconCanvasGroup.alpha = 0f;
        questionTextUI.color = DiscardTextColor;
        questionTextUI.text = $"<s>{questionTextUI.text}</s>";
        CorrectIcon.gameObject.SetActive(true);
        correctIconCanvasGroup.LeanAlpha(1f, 0.45f);
    }
}
