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
    public string id;

    public void Init(Sprite icon, int amount, string id)
    {
        image.sprite = icon;
        amountText.text = $"x {amount}";
        this.id = id;
    }

    public void ChangeAmount(int amount)
    {
        amountText.text = $"x {amount}";
    }
}
