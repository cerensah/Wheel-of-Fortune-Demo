using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class WheelRewardsUIManager : MonoBehaviour
{
    [Header("Wheel Related UI")]
    [SerializeField] private Image wheelImage;
    [SerializeField] private Sprite bronzeWheelSprite;
    [SerializeField] private Sprite silverWheelSprite;
    [SerializeField] private Sprite goldWheelSprite;

    [SerializeField] private Image indicatorImage;
    [SerializeField] private Sprite bronzeIndicatorSprite;
    [SerializeField] private Sprite silverIndicatorSprite;
    [SerializeField] private Sprite goldIndicatorSprite;

    [SerializeField] private List<Image> rewardImages;
    [SerializeField] private List<TextMeshProUGUI> rewardAmountText;

    [Header("Previously Collected Rewards UI")]
    [SerializeField] private Transform collectedRewardParent;
    [SerializeField] private Transform animEndPoint;
    [SerializeField] private GameObject collectedRewardPrefab;

    [Header("Current Reward VFX")]
    [SerializeField] private GameObject displayPanel;
    [SerializeField] private GameObject displayImage;
    [SerializeField] private GameObject sparklePrefab;
    [SerializeField] private GameObject rewardSfxPrefab;

    [Header("Zone Count UI")]
    [SerializeField] private TextMeshProUGUI zoneText;


    private void Start()
    {
        if (RewardManager.Instance != null)
        {
            RewardManager.Instance.OnRewardRandomized += RefreshWheelUI;
            RewardManager.Instance.OnRewardPicked += RunRewardVFX;
        }

        GameManager.Instance.OnGiveUp += ClearCollectedUI;
    }

    private void OnDisable()
    {
        if (RewardManager.Instance != null)
        {
            RewardManager.Instance.OnRewardRandomized -= RefreshWheelUI;
            RewardManager.Instance.OnRewardPicked -= RunRewardVFX;
        }

        GameManager.Instance.OnGiveUp -= ClearCollectedUI;
    }

    private void RefreshWheelUI()
    {

        zoneText.text = GameManager.Instance.zone.ToString();

        bool safeZone = (GameManager.Instance.zone % 5 == 0) || (GameManager.Instance.zone == 1);
        zoneText.color = safeZone ? Color.green : Color.white;

        var rewards = RewardManager.Instance.GetCurrentWheelRewards();

        for (int i = 0; i < 8; i++)
        {
            rewardImages[i].sprite = rewards[i].data.icon;
            rewardAmountText[i].text = $"x {rewards[i].amount}";
        }

        if (GameManager.Instance.zone % 30 == 0)
        {
            wheelImage.sprite = goldWheelSprite;
            indicatorImage.sprite = goldIndicatorSprite;
        }
        else if (GameManager.Instance.zone % 5 == 0)
        {
            wheelImage.sprite = silverWheelSprite;
            indicatorImage.sprite = silverIndicatorSprite;
        }
        else
        {
            wheelImage.sprite = bronzeWheelSprite;
            indicatorImage.sprite = bronzeIndicatorSprite;
        }

        // wheel bounce using DOTween -> feedbakc for user
        wheelImage.transform.DOScale(1.1f, 0.15f).SetLoops(2, LoopType.Yoyo);
    }


    private void RunRewardVFX(WheelReward reward)
    {
        StartCoroutine(AddCollectedRewardUI(reward));
    }
    private IEnumerator AddCollectedRewardUI(WheelReward reward)
    {
        displayPanel.SetActive(true);
        displayImage.GetComponent<Image>().sprite = reward.data.icon;

        RewardMoveAnimation();
        PlayRewardSparkleEffect();

        yield return new WaitForSeconds(2f);

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

    private void RewardMoveAnimation()
    {
        GameObject sfx = Instantiate(rewardSfxPrefab, transform);
        Destroy(sfx, 1f);

        RectTransform rect = displayImage.GetComponent<RectTransform>();
        rect.transform.position = displayPanel.transform.position;

        // animate to collected panel
        rect.DOMove(animEndPoint.position, 2f)
            .SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                rect.localScale = Vector3.one;
                displayPanel.SetActive(false);
            });
    }

    private void PlayRewardSparkleEffect()
    {

        for (int i = 0; i < 8; i++)
        {
            GameObject sparkle = Instantiate(
                sparklePrefab,
                displayPanel.transform
            );

            RectTransform rt =
                sparkle.GetComponent<RectTransform>();

            // start at reward center
            rt.position = wheelImage.transform.position;

            // random spread direction
            Vector2 randomOffset = new Vector2(
                UnityEngine.Random.Range(-200f, 200f),
                UnityEngine.Random.Range(-150f, 150f)
            );

            // burst outward
            rt.DOMove(
                (Vector2)rt.position + randomOffset,
                0.5f
            ).SetEase(Ease.OutQuad);

            // scale pop
            rt.DOScale(0f, 0.6f)
                .SetEase(Ease.InBack);

            // fade
            CanvasGroup cg =
                sparkle.GetComponent<CanvasGroup>();

            cg.DOFade(0f, 0.6f);

            Destroy(sparkle, 0.8f);
        }
    }
}
