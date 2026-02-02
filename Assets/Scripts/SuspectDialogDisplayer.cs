using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SuspectDialogDisplayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ResponceText;
    [SerializeField] private CanvasGroup canvasGroup;
    
    [Header("Animation Settings")]
    [SerializeField] private float fadeInDuration = 0.3f;
    [SerializeField] private float fadeOutDuration = 0.2f;
    [SerializeField] private float wordAppearDuration = 0.08f;
    [SerializeField] private float delayBetweenWords = 0.05f;

    private Coroutine currentTypingCoroutine;
    private bool isTypingComplete = false;

    private void Awake()
    {
        // Ensure we have a CanvasGroup
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
        }
        
        // Start invisible
        canvasGroup.alpha = 0f;
        ResponceText.text = "";
    }

    public void DisplaySuspectResponce(string responce)
    {
        if (currentTypingCoroutine != null)
        {
            StopCoroutine(currentTypingCoroutine);
        }
        isTypingComplete = false;
        currentTypingCoroutine = StartCoroutine(TypeWriterWordByWord(responce));
    }

    private IEnumerator TypeWriterWordByWord(string fullText)
    {
        // Clear the text first
        ResponceText.text = "";
        
        // Fade in the container
        yield return StartCoroutine(FadeIn());
        
        // Split text into words
        string[] words = fullText.Split(' ');
        string displayedText = "";
        
        // Display words one by one with smooth fade effect
        for (int i = 0; i < words.Length; i++)
        {
            // Fade in the current word
            yield return StartCoroutine(FadeInSingleWord(displayedText, words[i]));
            
            // Add word to displayed text
            if (i > 0)
            {
                displayedText += " ";
            }
            displayedText += words[i];
            ResponceText.text = displayedText;
            
            yield return new WaitForSeconds(delayBetweenWords);
        }
        
        isTypingComplete = true;
    }

    private IEnumerator FadeInSingleWord(string previousText, string newWord)
    {
        float elapsed = 0f;
        
        while (elapsed < wordAppearDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / wordAppearDuration);
            
            // Build display text with the new word fading in
            string spacer = string.IsNullOrEmpty(previousText) ? "" : " ";
            
            // Use color alpha instead of <alpha> tag for smoother effect
            Color wordColor = ResponceText.color;
            int alphaValue = Mathf.RoundToInt(alpha * 255);
            string hexAlpha = alphaValue.ToString("X2");
            
            // Get the base color hex (without alpha)
            string baseColorHex = ColorUtility.ToHtmlStringRGB(wordColor);
            
            ResponceText.text = previousText + spacer + "<color=#" + baseColorHex + hexAlpha + ">" + newWord + "</color>";
            
            yield return null;
        }
    }

    private IEnumerator FadeIn()
    {
        float elapsed = 0f;
        canvasGroup.alpha = 0f;
        
        while (elapsed < fadeInDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsed / fadeInDuration);
            yield return null;
        }
        
        canvasGroup.alpha = 1f;
    }

    public IEnumerator FadeOut()
    {
        float elapsed = 0f;
        float startAlpha = canvasGroup.alpha;
        
        while (elapsed < fadeOutDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, elapsed / fadeOutDuration);
            yield return null;
        }
        
        canvasGroup.alpha = 0f;
    }

    public bool IsTypingComplete()
    {
        return isTypingComplete;
    }

    public IEnumerator WaitForTypingComplete()
    {
        while (!isTypingComplete)
        {
            yield return null;
        }
    }
}
