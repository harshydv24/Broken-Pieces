using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Suspect/Responses")]
public class SuspectResponcesSO : ScriptableObject
{
    [Serializable]
    public class ResponseOptions
    {
        public string[] options = new string[3];
    }

    public ResponseOptions[] responses;

    public string GetResponse(int questionIndex, int optionIndex)
    {
        if (questionIndex < 0 || questionIndex >= responses.Length)
        {
            Debug.LogError($"Question index {questionIndex} out of range!");
            return "";
        }
        if (optionIndex < 0 || optionIndex >= responses[questionIndex].options.Length)
        {
            Debug.LogError($"Option index {optionIndex} out of range!");
            return "";
        }
        return responses[questionIndex].options[optionIndex];
    }
}
