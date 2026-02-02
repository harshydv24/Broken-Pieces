using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TypewritterEffect : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textMeshPro;
    [SerializeField] string inputText;

    private void Start()
    {

    }

    public void ClearText()
    {
        textMeshPro.text = "";
    }

    public IEnumerator TypeText()
    {
        if (inputText != null || inputText.Length != 0)
        {
            ClearText();
            StartCoroutine(AddText());
            yield return null;
        }
    }

    IEnumerator AddText()
    {
        for (int i = 0; i < inputText.Length; i++)
        {
            yield return new WaitForSeconds(0.1f);
            Debug.Log("#1");
            textMeshPro.text += inputText.Substring(i, 1);
            Debug.Log("#2");
            yield return Task.Yield();
        }
    }

}
