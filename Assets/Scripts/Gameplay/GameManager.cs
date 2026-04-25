using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Default,
    GameOver,
    Revived
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int zone;
    private GameState gameState = GameState.Default;

    public event Action<int> OnWheelRotated;
    public event Action OnGameOver;

    private int coin;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        zone = 1; 
    }
    public void TurnWheel()
    {
        int randomIdx = RewardManager.Instance.GenerateRandomRewardIdx();

        OnWheelRotated?.Invoke(randomIdx);

        zone++;

        RewardManager.Instance.RandomizeRewards();
    }

    public void ChangeGameState(GameState state)
    {
        gameState = state;
        if (gameState == GameState.GameOver)
        {
            OnGameOver?.Invoke();
        }
    }

    public GameState GetCurrentState()
    {
        return gameState;
    }

    public bool HasEnoughCoins(int amount)
    {
        return coin >= amount;
    }

    public void RefreshGame()
    {
        zone = 1;
        gameState = GameState.Default;
        RewardManager.Instance.RandomizeRewards();
    }
}
