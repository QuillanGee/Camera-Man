using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager {
    public static GameManager instance;
    public GameState state;

    void Awake()
    {
        instance = this;
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
    }

    public enum GameState
    {
        Platform,
        FirstPerson
    }
}