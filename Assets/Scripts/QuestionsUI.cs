using System;
using UnityEngine;

public class QuestionsUI : MonoBehaviour
{
    public event EventHandler OnDialogEnabled;
    public event EventHandler OnDialogDisabled;

    [SerializeField] private QuestionsUI questionsUI;
    [SerializeField] private Transform LineTransform;
    [SerializeField] public GameObject QuestionsPanel;

    private CanvasGroup canvasGroup;
    private Vector3 DefaultScale = new Vector3(0.8f, 0.8f, 1f);

    private bool IsQuestionUIActive = false;

    private void Start()
    {
        canvasGroup = QuestionsPanel.GetComponent<CanvasGroup>();
        QuestionsPanel.SetActive(false);
        canvasGroup.alpha = 0f;
        LineTransform.localScale = new Vector3(1, 0, 0);
    }

    private void OnEnable()
    {
        if (QuestionButton.Instance != null)
        {
            QuestionButton.Instance.OnQuestionSelected += QuestionButton_OnQuestionSelected;
        }
    }

    private void OnDisable()
    {
        if (QuestionButton.Instance != null)
        {
            QuestionButton.Instance.OnQuestionSelected -= QuestionButton_OnQuestionSelected;
        }
    }

    private void QuestionButton_OnQuestionSelected(object sender, EventArgs e)
    {
        CloseUpAnimation();
        // QuestionsPanel.SetActive(false);
    }

    void Update()
    {
        if(!GameManager.Instance.IsPlayingStarted()) return;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (IsQuestionUIActive)
            {
                // QuestionsPanel.SetActive(false);
                CloseUpAnimation();
                OnDialogDisabled?.Invoke(this, EventArgs.Empty);
                IsQuestionUIActive = false;
            }
            else
            {
                // QuestionsPanel.SetActive(true);
                OpenUpAnimation();
                OnDialogEnabled?.Invoke(this, EventArgs.Empty);
                IsQuestionUIActive = true;
            }
        }
    }

    private void OpenUpAnimation()
    {
        QuestionsPanel.SetActive(true);
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
        LineTransform.LeanScaleY(0f, 0.5f).setEaseOutExpo().setOnComplete(Hide);
    }

    private void Hide()
    {
        QuestionsPanel.SetActive(false);
    }

}
