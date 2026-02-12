using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BackStorySystem : MonoBehaviour
{
    public static BackStorySystem Instance;
    public event EventHandler OnBackStoryEnded;

    [SerializeField] private GameObject backstoryScreen;
    [SerializeField] private Image StoryImageDisplayer;
    [SerializeField] private TextMeshProUGUI StoryTextDisplayer;
    [SerializeField] private Sprite[] StoryImages;
    [SerializeField] private string[] StoryTexts;

    private bool isBackStoryEnded = false;
    private bool hasBackStoryStarted = false;
    private CanvasGroup BackStoryScreenCG;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
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
        StartCoroutine(PlayStoryComponents());
        
        yield return new WaitForSeconds(25f);

        Debug.Log("Back Story Ended");
        BackStoryScreenCG.LeanAlpha(0f, 0.8f).setOnComplete(BackStoryEnded);
        OnBackStoryEnded?.Invoke(this, EventArgs.Empty);
    }

    public void BackStoryEnded()
    {
        backstoryScreen.SetActive(false);
    }

    private IEnumerator PlayStoryComponents()
    {
        for(int i = 0; i < StoryImages.Length; i++)
        {
            StoryImageDisplayer.sprite = StoryImages[i];
            StoryTextDisplayer.text = StoryTexts[i];
            yield return new WaitForSeconds(5);
        }
    }
}
