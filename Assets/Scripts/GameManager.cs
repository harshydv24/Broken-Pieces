using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance{get; private set;}

    public event EventHandler OnGameStateChanged;

    [SerializeField] private BackStorySystem backStorySystem;

    public enum GameState
    {
        HomeScreen,
        BackgroundStory,
        Playing,
        GameOver
    }

    private GameState currentGameState;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // Set initial state properly with event
        SetGameState(GameState.HomeScreen);
        BackStorySystem.Instance.OnBackStoryEnded += BackStorySystem_OnBackStoryEnded;
    }

    private void BackStorySystem_OnBackStoryEnded(object sender, EventArgs e)
    {
        SetGameState(GameState.Playing);
    }

    private void Update()
    {
        // Handle per-frame logic based on current state
        // switch (currentGameState)
        // {
        //     case GameState.HomeScreen:
        //         // HomeScreen logic handled by UI button calling StartGame()
        //         break;
        //     case GameState.BackgroundStory:
        //         // Check if backstory has ended and transition to Playing
        //         if (backStorySystem.BackStoryEnded())
        //         {
        //             SetGameState(GameState.Playing);
        //         }
        //         break;
        //     case GameState.Playing:
        //         // Playing logic (e.g., check for game over conditions)
        //         break;
        //     case GameState.GameOver:
        //         // GameOver logic
        //         break;
        // }
    }


    private void SetGameState(GameState newState)
    {
        if (currentGameState == newState) return;
        
        currentGameState = newState;
        
        // Handle state-specific setup
        switch (currentGameState)
        {
            case GameState.HomeScreen:
                ShowCursor();
                break;
            case GameState.BackgroundStory:
                HideCursor();
                break;
            case GameState.Playing:
                // Cursor state managed by QuestionsUI interaction
                break;
            case GameState.GameOver:
                ShowCursor();
                break;
        }
        
        // Fire the event after state has changed
        OnGameStateChanged?.Invoke(this, EventArgs.Empty);
    }

    private void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void StartGame()
    {
        SetGameState(GameState.BackgroundStory);
    }

    public bool IsBackStoryStarted()
    {
        return currentGameState == GameState.BackgroundStory;
    }

    public bool IsPlayingStarted()
    {
        return currentGameState == GameState.Playing;
    }

    public bool IsHomeScreenActive()
    {
        return currentGameState == GameState.HomeScreen;
    }
}
