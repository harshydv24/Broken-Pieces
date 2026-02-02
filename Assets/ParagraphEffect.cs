using System.Collections;
using TMPro;
using UnityEngine;

public class WordTypewriter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI textUI;

    [Header("Typing Settings")]
    [Tooltip("Words revealed per second")]
    [SerializeField] private float wordsPerSecond = 3f;

    [Tooltip("Extra pause after punctuation")]
    [SerializeField] private float punctuationPause = 0.25f;

    private Coroutine typingRoutine;
    private string fullText;

    void Awake()
    {
        if (textUI == null)
            textUI = GetComponent<TextMeshProUGUI>();
    }

    public void StartTyping(string text)
    {
        if (typingRoutine != null)
            StopCoroutine(typingRoutine);

        fullText = text;
        typingRoutine = StartCoroutine(TypeWords());
    }

    IEnumerator TypeWords()
    {
        textUI.text = "";

        string[] words = fullText.Split(' ');
        float baseDelay = 1f / wordsPerSecond;

        for (int i = 0; i < words.Length; i++)
        {
            textUI.text += words[i] + " ";

            float delay = baseDelay;

            // Smooth natural pauses after punctuation
            if (EndsWithPunctuation(words[i]))
                delay += punctuationPause;

            yield return new WaitForSeconds(delay);
        }
    }

    bool EndsWithPunctuation(string word)
    {
        char lastChar = word[word.Length - 1];
        return lastChar == '.' || lastChar == ',' || lastChar == '?' || lastChar == '!';
    }

    // OPTIONAL: Instantly reveal full text
    public void Skip()
    {
        if (typingRoutine != null)
            StopCoroutine(typingRoutine);

        textUI.text = fullText;
    }
}
