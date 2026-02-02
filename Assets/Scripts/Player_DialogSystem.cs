using System;
using Unity.VisualScripting;
using UnityEngine;

public class Player_DialogSystem : MonoBehaviour
{
    public event EventHandler<OnQuestionSelectedEventArgs> OnQuestionSelected;
    public class OnQuestionSelectedEventArgs : EventArgs
    {
        public int QIndex;
    }

    [SerializeField] private GameObject questionButtonPrefab;
    [SerializeField] private Transform questionsBox;
    [SerializeField] private string[] QuestionsStage1;
    [SerializeField] private string[] QuestionsStage2;
    [SerializeField] private string[] QuestionsStage3;

    public enum QuestionStages
    {
        Stage1,
        Stage2,
        Stage3
    }

    private QuestionStages currentStage;

    private void Start()
    {
        currentStage = QuestionStages.Stage1;
        Stage1Questions();
    }

    private void Update()
    {
        switch (currentStage)
        {
            case QuestionStages.Stage1:
                break;

            case QuestionStages.Stage2:
                break;

            case QuestionStages.Stage3:
                break;
        }

        //Debug.Log(currentStage);
    }

    private void Stage1Questions()
    {
        for(int i = 0; i < QuestionsStage1.Length; i++)
        {
            GameObject newQuestionButton = Instantiate(questionButtonPrefab, questionsBox);
            QuestionButton questionButton = newQuestionButton.GetComponent<QuestionButton>();
            questionButton.SetQuestionText(i, QuestionsStage1[i]);
        }
    }

    public void QuestionSelected(int questionIndex)
    {
        // Close the questions panel
        CloseQuestionsPanel();
        
        OnQuestionSelected?.Invoke(this, new OnQuestionSelectedEventArgs { 
            QIndex = questionIndex
        });
    }

    public void CloseQuestionsPanel()
    {
        if (questionsBox != null && questionsBox.parent != null)
        {
            questionsBox.parent.gameObject.SetActive(false);
        }
    }

    public void OpenQuestionsPanel()
    {
        if (questionsBox != null && questionsBox.parent != null)
        {
            questionsBox.parent.gameObject.SetActive(true);
        }
    }
}
