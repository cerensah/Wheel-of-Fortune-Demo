using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WheelRewardsUIManager : MonoBehaviour
{
    [Header("Wheel Related UI")]
    [SerializeField] private List<Image> rewardImages;
    [SerializeField] private List<TextMeshProUGUI> rewardAmountText;

    [Header("Previously Collected Rewards UI")]
    [SerializeField] private Transform collectedRewardParent;
    [SerializeField] private GameObject collectedRewardPrefab;

    private void Start()
    {
        if (RewardManager.Instance != null)
        {
            RewardManager.Instance.OnRewardRandomized += RefreshWheelUI;
            RewardManager.Instance.OnRewardPicked += AddCollectedRewardUI;
        }

        GameManager.Instance.OnGiveUp += ClearCollectedUI;
    }

    private void OnDisable()
    {
        if (RewardManager.Instance != null)
        {
            RewardManager.Instance.OnRewardRandomized -= RefreshWheelUI;
            RewardManager.Instance.OnRewardPicked -= AddCollectedRewardUI;
        }

        GameManager.Instance.OnGiveUp -= ClearCollectedUI;
    }

    private void RefreshWheelUI()
    {
        var rewards = RewardManager.Instance.GetCurrentWheelRewards();

        for (int i = 0; i < 8; i++)
        {
            rewardImages[i].sprite = rewards[i].data.icon;
            rewardAmountText[i].text = $"x {rewards[i].amount}";
        }
    }

    private void AddCollectedRewardUI(WheelReward reward)
    {
        GameObject rewardObj = Instantiate(collectedRewardPrefab, collectedRewardParent);
        rewardObj.GetComponent<CollectedRewardUI>().Init(reward.data.icon, reward.amount);
    }

    private void ClearCollectedUI()
    {
        foreach(Transform child in collectedRewardParent)
        {
            Destroy(child.gameObject);
        }
    }
}
