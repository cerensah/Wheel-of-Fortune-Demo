using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WheelReward
{
    public RewardData data;
    public int amount;
}
public class RewardManager : MonoBehaviour
{
    public static RewardManager Instance;
    [SerializeField] private List<RewardData> allRewards;
    [SerializeField] private RewardData bomb;
    [SerializeField] private List<WheelReward> currentWheelRewards = new List<WheelReward>();
    [SerializeField] private List<WheelReward> collectedRewards = new List<WheelReward>();

    public event Action OnRewardRandomized;
    public event Action<WheelReward> OnRewardPicked;

    private int zone = 1;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if(GameManager.Instance != null)
        {
            GameManager.Instance.OnWheelRotated += AddReward;
            GameManager.Instance.OnGiveUp += ClearCollectedRewards;
        }

        RandomizeRewards();
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnWheelRotated -= AddReward;
            GameManager.Instance.OnGiveUp -= ClearCollectedRewards;
        }

    }
    public void RandomizeRewards()
    {
        currentWheelRewards.Clear();

        zone = GameManager.Instance.zone;

        bool safeZone = (zone % 5 == 0) || (zone == 1);

        //only add bomb if the level is not a multiple of 5 or we arnt in the first level
        if (!safeZone)
        {
            Debug.Log($"zone is {zone} adding bomb");
            currentWheelRewards.Add(new WheelReward
            {
                data = bomb,
                amount = 1
            });
        }

        while(currentWheelRewards.Count < 8) {
            int randomIdx = UnityEngine.Random.Range(0, allRewards.Count);

            RewardData reward = allRewards[randomIdx];
            currentWheelRewards.Add(new WheelReward
            {
                data = reward,
                amount = reward.stackable? GenerateRewardAmount(allRewards[randomIdx].importance) : 1 //if stackable amount is generated, else just 1
            });
        }

        OnRewardRandomized?.Invoke();
    }

    public int GenerateRandomRewardIdx()
    {
        return UnityEngine.Random.Range(0, GlobalVariables.SLICE_COUNT);
    }


    public void AddReward(int idx)
    {
        var reward = currentWheelRewards[idx];

        if (reward.data.item_id == GlobalVariables.BOMB_ID)
        {
            GameManager.Instance.ChangeGameState(GameState.GameOver);
            return;
        }

        collectedRewards.Add(reward);

        OnRewardPicked?.Invoke(reward);
    }

    public List<WheelReward> GetCurrentWheelRewards()
    {
        return currentWheelRewards;
    }

    public int GenerateRewardAmount(RewardImportance importance)
    {
        switch (importance)
        {
            case (RewardImportance.Low): return UnityEngine.Random.Range(zone * 50 , zone * 150); 
            case (RewardImportance.Medium): return UnityEngine.Random.Range(zone * 1, zone * 5);
            case (RewardImportance.High): return UnityEngine.Random.Range(1, 5);
            default:
                return 1;
        }
    }

    private void ClearCollectedRewards()
    {
        collectedRewards.Clear();
    }
}
