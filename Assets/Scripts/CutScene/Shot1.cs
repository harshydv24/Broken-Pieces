using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Shot1 : MonoBehaviour
{
    [SerializeField] TypewritterEffect factText;
    [SerializeField] Image clueStackImage;
    [SerializeField] List<Image> clueList = new List<Image>();

    private void Awake()
    {
        setAlphaColorZero(clueStackImage);
    }

    void setAlphaColorZero(Image image)
    {
        Color color = image.color;
        color.a = 0;
        image.color = color;
    }





    public IEnumerator StartShot()
    {
        factText.ClearText();
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(factText.TypeText());
        yield return new WaitForSeconds(2);

        yield return StartCoroutine(lerpAlphaColorValue(clueStackImage, 1, 4));

        yield return StartCoroutine(ShowCutscenes());

    }

    IEnumerator ShowCutscenes()
    {
        foreach (Image image in clueList)
        {
            yield return StartCoroutine(LerpScale(image.transform, 3.29f, 0.01f));
            yield return StartCoroutine(lerpAlphaColorValue(image, 1, 2));
            //yield return new WaitForSeconds(1);
            //yield return new WaitForSeconds(1);
            yield return StartCoroutine(LerpScale(image.transform, 1, 2));
        }
    }

    IEnumerator lerpAlphaColorValue(Image image, int endInt, float duration)
    {
        float t = 0f;
        Color c = image.color;
        float startInt = c.a;

        while (t < duration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(startInt, endInt, t / duration);
            image.color = c;
            yield return null;
        }

        c.a = endInt;
        image.color = c;

        yield return Task.Yield();
    }

    IEnumerator LerpScale(Transform target, float endValue, float duration)
    {
        float t = 0f;
        Vector3 startScale = target.localScale;
        Vector3 endScale = Vector3.one * endValue;

        while (t < duration)
        {
            t += Time.deltaTime;
            target.localScale = Vector3.Lerp(startScale, endScale, t / duration);
            yield return null;
        }

        target.localScale = endScale;
    }



}
