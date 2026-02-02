using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suspect_DialogSystem : MonoBehaviour
{
    public static Suspect_DialogSystem Instance{get; private set;}

    public event EventHandler CheckForResponce;

    [SerializeField] private SuspectResponcesSO suspect1ResponcesSO;
    [SerializeField] private SuspectResponcesSO suspect2ResponcesSO;
    [SerializeField] private SuspectResponcesSO suspect3ResponcesSO;
    [SerializeField] private Player_DialogSystem player_DialogSystem;
    [SerializeField] private GameObject Suspect1Mask;
    [SerializeField] private GameObject Suspect2Mask;
    [SerializeField] private GameObject Suspect3Mask;
    [SerializeField] private GameObject SuspectResponceUI;
    [SerializeField] private Transform Suspect1UIPosition;
    [SerializeField] private Transform Suspect2UIPosition;
    [SerializeField] private Transform Suspect3UIPosition;

    [SerializeField] private ResponceCheckingSystem responceCheckingSystem;

    [Header("Animation Settings")]
    [SerializeField] private float delayBetweenSuspects = 0.05f;

    private int Suspect1Answer;
    private int Suspect2Answer;
    private int Suspect3Answer;

    public bool IsMaskedBroken = false;
    private bool CanAskQuestions = true;
    private bool isFirstQuestion = true;

    // Track currently displayed response UIs
    private List<GameObject> currentResponseUIs = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        player_DialogSystem.OnQuestionSelected += OnQuestionSelected;
        responceCheckingSystem.CheckResponce += OnCheckResponce;
    }

    private void OnCheckResponce(object sender, ResponceCheckingSystem.CheckResponceEventArgs e)
    {
        CheckResponce(e.suspectID);
    }

    private void CheckResponce(int suspectID)
    {
        if(IsMaskedBroken)
        {
            Debug.Log("Mask already broken, no further checks.");
            return;
        }

        if(suspectID == 1 && (Suspect1Answer == 1 || Suspect1Answer == 2))
        {
            Debug.Log("Suspect 1 is lying, break 1 pice");
            // OnSuspect1MaskBroke?.Invoke(this, EventArgs.Empty);
            Suspect1Mask.GetComponent<MaskShatterController>().BreakRandomPiece();
            IsMaskedBroken = true;
        }
        else if(suspectID == 2 && (Suspect2Answer == 1 || Suspect2Answer == 2))
        {
            Debug.Log("Suspect 2 is lying, break 1 pice");
            // OnSuspect2MaskBroke?.Invoke(this, EventArgs.Empty);
            Suspect2Mask.GetComponent<MaskShatterController>().BreakRandomPiece();
            IsMaskedBroken = true;
        }
        else if(suspectID == 3 && (Suspect3Answer == 1 || Suspect3Answer == 2))
        {
            Debug.Log("Suspect 3 is lying, break 1 pice");
            // OnSuspect3MaskBroke?.Invoke(this, EventArgs.Empty);
            Suspect3Mask.GetComponent<MaskShatterController>().BreakRandomPiece();
            IsMaskedBroken = true;
        }
    }

    private void OnQuestionSelected(object sender, Player_DialogSystem.OnQuestionSelectedEventArgs e)
    {
        StartCoroutine(GenrateRespond(e.QIndex));
    }

    IEnumerator GenrateRespond(int questionIndex)
    {
        CanAskQuestions = false;

        // Fade out existing responses if this is not the first question
        if (!isFirstQuestion && currentResponseUIs.Count > 0)
        {
            yield return StartCoroutine(FadeOutAllResponses());
        }
        isFirstQuestion = false;

        // Clear the list and old objects
        ClearAllOldResponses();

        // Display suspect responses one by one
        yield return StartCoroutine(DisplaySuspect1Dialog(questionIndex));
        yield return new WaitForSeconds(delayBetweenSuspects);
        
        yield return StartCoroutine(DisplaySuspect2Dialog(questionIndex));
        yield return new WaitForSeconds(delayBetweenSuspects);
        
        yield return StartCoroutine(DisplaySuspect3Dialog(questionIndex));
        yield return new WaitForSeconds(1f);

        CanAskQuestions = true;
        CheckForResponce?.Invoke(this, EventArgs.Empty);
    }

    private IEnumerator FadeOutAllResponses()
    {
        List<Coroutine> fadeCoroutines = new List<Coroutine>();
        
        foreach (GameObject responseUI in currentResponseUIs)
        {
            if (responseUI != null)
            {
                SuspectDialogDisplayer displayer = responseUI.GetComponent<SuspectDialogDisplayer>();
                if (displayer != null)
                {
                    fadeCoroutines.Add(StartCoroutine(displayer.FadeOut()));
                }
            }
        }

        // Wait for all fade outs to complete
        foreach (Coroutine coroutine in fadeCoroutines)
        {
            yield return coroutine;
        }
    }

    private void ClearAllOldResponses()
    {
        // Destroy old UI objects
        foreach (GameObject responseUI in currentResponseUIs)
        {
            if (responseUI != null)
            {
                Destroy(responseUI);
            }
        }
        currentResponseUIs.Clear();

        // Also clear any remaining children
        ClearOldResponses(Suspect1UIPosition);
        ClearOldResponses(Suspect2UIPosition);
        ClearOldResponses(Suspect3UIPosition);
    }

    private IEnumerator DisplaySuspect1Dialog(int questionIndex)
    {
        Suspect1Answer = UnityEngine.Random.Range(0, 3);
        Debug.Log("Suspect 1 Response: " + suspect1ResponcesSO.GetResponse(questionIndex, Suspect1Answer));
        SuspectDialogDisplayer displayer = DisplayResponce(1, questionIndex);
        if (displayer != null)
        {
            yield return StartCoroutine(displayer.WaitForTypingComplete());
        }
    }

    private IEnumerator DisplaySuspect2Dialog(int questionIndex)
    {
        Suspect2Answer = UnityEngine.Random.Range(0, 3);
        Debug.Log("Suspect 2 Response: " + suspect2ResponcesSO.GetResponse(questionIndex, Suspect2Answer));
        SuspectDialogDisplayer displayer = DisplayResponce(2, questionIndex);
        if (displayer != null)
        {
            yield return StartCoroutine(displayer.WaitForTypingComplete());
        }
    }

    private IEnumerator DisplaySuspect3Dialog(int questionIndex)
    {
        Suspect3Answer = UnityEngine.Random.Range(0, 3);
        Debug.Log("Suspect 3 Response: " + suspect3ResponcesSO.GetResponse(questionIndex, Suspect3Answer));
        SuspectDialogDisplayer displayer = DisplayResponce(3, questionIndex);
        if (displayer != null)
        {
            yield return StartCoroutine(displayer.WaitForTypingComplete());
        }
    }

    private SuspectDialogDisplayer DisplayResponce(int SuspectID, int questionIndex)
    {
        GameObject responceUI = null;
        SuspectDialogDisplayer displayer = null;

        if(SuspectID == 1)
        {
            responceUI = Instantiate(SuspectResponceUI, Suspect1UIPosition.position, Suspect1UIPosition.rotation, Suspect1UIPosition);
            displayer = responceUI.GetComponent<SuspectDialogDisplayer>();
            displayer.DisplaySuspectResponce(suspect1ResponcesSO.GetResponse(questionIndex, Suspect1Answer));
        }
        else if(SuspectID == 2)
        {
            responceUI = Instantiate(SuspectResponceUI, Suspect2UIPosition.position, Suspect2UIPosition.rotation, Suspect2UIPosition);
            displayer = responceUI.GetComponent<SuspectDialogDisplayer>();
            displayer.DisplaySuspectResponce(suspect2ResponcesSO.GetResponse(questionIndex, Suspect2Answer));
        }
        else if(SuspectID == 3)
        {
            responceUI = Instantiate(SuspectResponceUI, Suspect3UIPosition.position, Suspect3UIPosition.rotation, Suspect3UIPosition);
            displayer = responceUI.GetComponent<SuspectDialogDisplayer>();
            displayer.DisplaySuspectResponce(suspect3ResponcesSO.GetResponse(questionIndex, Suspect3Answer));
        }

        // Track the new response UI
        if (responceUI != null)
        {
            currentResponseUIs.Add(responceUI);
        }

        return displayer;
    }

    private void ClearOldResponses(Transform parent)
    {
        foreach(Transform child in parent)
        {
            if(child.gameObject == SuspectResponceUI) continue;
            Destroy(child.gameObject);
        }
    }

    public bool GetCanAskQuestions()
    {
        return CanAskQuestions;
    }
}
