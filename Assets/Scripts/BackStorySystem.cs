using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackStorySystem : MonoBehaviour
{
    public static BackStorySystem Instance;
    public event EventHandler OnBackStoryEnded;
    public CutSceneSequence cut;

    [SerializeField] private GameObject backstoryScreen;

    private bool isBackStoryEnded = false;
    private bool hasBackStoryStarted = false;
    private CanvasGroup BackStoryScreenCG;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        cut.gameObject.SetActive(false);
        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;
        BackStoryScreenCG = backstoryScreen.GetComponent<CanvasGroup>();
        BackStoryScreenCG.alpha = 0f;
        backstoryScreen.SetActive(false);
    }

    private void GameManager_OnGameStateChanged(object sender, EventArgs e)
    {
        // Only start the backstory sequence once
        if (GameManager.Instance.IsBackStoryStarted() && !hasBackStoryStarted)
        {
            hasBackStoryStarted = true;
            StartCoroutine(StartBackStorySeq());
        }
    }

    private IEnumerator StartBackStorySeq()
    {
        isBackStoryEnded = false;
        Debug.Log("Back Story Started");
        backstoryScreen.SetActive(true);
        BackStoryScreenCG.LeanAlpha(1f, 0.5f);
        // cut.gameObject.SetActive(true);
        // yield return StartCoroutine(cut.startCutScene());
        // cut.gameObject.SetActive(false);
        
        yield return new WaitForSeconds(10f);


        Debug.Log("Back Story Ended");
        BackStoryScreenCG.LeanAlpha(0f, 0.8f).setOnComplete(BackStoryEnded);
        OnBackStoryEnded?.Invoke(this, EventArgs.Empty);
    }

    public void BackStoryEnded()
    {
        backstoryScreen.SetActive(false);
    }
}
