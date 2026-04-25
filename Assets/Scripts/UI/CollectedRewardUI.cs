using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/*
 Attached to reward_prefab
 */

public class CollectedRewardUI : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI amountText;

    public void Init(Sprite icon, int amount)
    {
        image.sprite = icon;
        amountText.text = $"x {amount}";
    }
}
