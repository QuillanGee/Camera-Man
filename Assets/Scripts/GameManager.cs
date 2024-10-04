using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class GameManager {
    public static GameManager instance;
    public GameState state;
    
    public static event Action<GameState> OnGameStateChanged;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        UpdateState(GameState.Platform);
    }

    public void UpdateState(GameState newState)
    {
        state = newState;

        switch (newState)
        {
            case GameState.Platform:
                break;
            case GameState.FirstPerson:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
        OnGameStateChanged?.Invoke(newState);
    }

    public enum GameState
    {
        Platform,
        FirstPerson
    }
}