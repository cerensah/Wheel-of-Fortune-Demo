using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Default,
    GameOver,
    GiveUp
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int zone;
    private GameState gameState = GameState.Default;

    public event Action OnGameOver;
    public event Action OnGiveUp;

    private bool hasRevived;

    private int coin = 100;

    [SerializeField] private WheelRotator wheelRotator;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        zone = 1;
        // GameManager listens to VISUAL result only
        wheelRotator.OnSpinFinished += HandleSpinFinished;

        // First wheel setup
        RewardManager.Instance.RandomizeRewards();
    }

    private void OnDisable()
    {
        wheelRotator.OnSpinFinished -= HandleSpinFinished;
    }

    public void TurnWheel()
    {
        // Only generate target and start visual spin
        int randomIdx = RewardManager.Instance.GenerateRandomRewardIdx();

        wheelRotator.StartSpin(randomIdx);
    }

    private void HandleSpinFinished(int rewardIndex)
    {
        // Game flow happens here AFTER animation ends

        RewardManager.Instance.AddReward(rewardIndex);

        if (gameState == GameState.GameOver)
            return;

        zone++;

        RewardManager.Instance.RandomizeRewards();
    }
    public void ChangeGameState(GameState state)
    {
        gameState = state;
        if (gameState == GameState.GameOver)
        {
            OnGameOver?.Invoke();
        } else if(gameState == GameState.GiveUp)
        {
            RefreshGame();
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

    public void SetRevived(bool revived)
    {
        hasRevived = revived;
    }

    public bool GetRevived()
    {
        return hasRevived;
    }

    public void RefreshGame()
    {
        zone = 1;
        gameState = GameState.Default;
        hasRevived = false;

        OnGiveUp?.Invoke();
        RewardManager.Instance.RandomizeRewards();
    }
}
