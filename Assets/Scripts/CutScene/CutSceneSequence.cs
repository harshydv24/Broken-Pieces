using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CutSceneSequence : MonoBehaviour
{
    [SerializeField] Shot1 shot1;

    void Start()
    {
        
    }

    

    public IEnumerator startCutScene()
    {
        yield return StartCoroutine(shot1.StartShot());
    }
}
