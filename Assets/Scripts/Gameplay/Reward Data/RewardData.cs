using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * an enum to denote the importance of the item. 
 * lower importance items appear more and will give in higher amounts
 * higher importance items appear less and will give in lower amounts
 */

public enum RewardImportance
{
    Low,
    Medium,
    High
}
[CreateAssetMenu(fileName = "NewRewardData", menuName = "RewardData/NewReward")]
public class RewardData : ScriptableObject
{
    public string item_id;
    public Sprite icon;
    public RewardImportance importance;
}
