using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.Experimental.GlobalIllumination;

public class MaskShatterController : MonoBehaviour
{
    [SerializeField] public List<Rigidbody> crackPieces; 
    [SerializeField] private string suspectName = "Suspect";
    // [SerializeField] public Animator CellingLightAnimator;

    private List<Rigidbody> remainingPieces = new List<Rigidbody>();
    private bool IsGameOver = false;

    public static event Action<string> OnGameOver;

    // [SerializeField] private Light flickerLight;
    // [SerializeField] private float flickerSpeed = 2f;
    // [SerializeField] private float intensityMultiplier = 1f;
    // float randomOffset;

    void Start()
    {
        IsGameOver = false;
        remainingPieces.Clear();

        foreach (var rb in crackPieces)
        {
            if (rb == null)
            {
                Debug.LogError("A Crack Piece Rigidbody is NULL!");
                continue;
            }

            rb.isKinematic = true;
            rb.useGravity = false;
            remainingPieces.Add(rb);
        }

        // randomOffset = UnityEngine.Random.Range(0f, 100f);
    }

    // void Update()
    // {
    //     float noise = Mathf.PerlinNoise(Time.time * flickerSpeed, randomOffset);
    //     flickerLight.intensity = noise * intensityMultiplier;
    // }

    public void BreakRandomPiece()
    {
        if (remainingPieces.Count == 0) return;

        int index = UnityEngine.Random.Range(0, remainingPieces.Count);
        Rigidbody piece = remainingPieces[index];

        // Detach from mask
        piece.transform.SetParent(null);

        // Enable physics
        piece.isKinematic = false;
        piece.useGravity = true;

        // Small explosion for realism
        piece.AddExplosionForce(
            UnityEngine.Random.Range(2f, 4f),
            piece.transform.position + Vector3.back * 0.2f,
            1f
        );

        remainingPieces.RemoveAt(index);

        MaskBreakingVFX();

        if (remainingPieces.Count == 0)
        {
            IsGameOver = true;
            Debug.Log("===========================================");
            Debug.Log("GAME OVER - You successfully found the criminal!");
            Debug.Log("The criminal is: " + suspectName);
            Debug.Log("===========================================");
            OnGameOver?.Invoke(suspectName);
            return;
        }
    }

    private void MaskBreakingVFX()
    {
        // Light flicker + little bit swing.
        // sound effect
        // little bit camera shake.
    }

    public bool GetIsGameOver()
    {
        return IsGameOver;
    }

    public int GetRemainingPiecesCount()
    {
        return remainingPieces.Count;
    }
}