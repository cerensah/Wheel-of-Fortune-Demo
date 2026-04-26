using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUIManager : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Button giveUpButton;
    [SerializeField] private Button reviveButton;

    //game manager is execcuted after this script so better subscribe in Start than onEnable
    //think of a better solution??
    private void Start()
    {
        GameManager.Instance.OnGameOver += InitPanel;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameOver -= InitPanel;
    }
    private void InitPanel()
    {
        panel.gameObject.SetActive(true);

        giveUpButton.onClick.RemoveAllListeners();
        giveUpButton.onClick.AddListener(OnGiveUpClicked);

        bool hasRevived = GameManager.Instance.GetRevived();
        reviveButton.gameObject.SetActive(!hasRevived); //only show revive if havent revived before
        reviveButton.onClick.RemoveAllListeners();
        reviveButton.onClick.AddListener(OnReviveClicked);
    }

    private void OnGiveUpClicked()
    {
        GameManager.Instance.ChangeGameState(GameState.GiveUp);
        GameManager.Instance.RefreshGame();
        panel.gameObject.SetActive(false);
    }

    private void OnReviveClicked()
    {
        if(GameManager.Instance.GetRevived() != true && 
            GameManager.Instance.HasEnoughCoins(GlobalVariables.reviveCost))
        {
            GameManager.Instance.SetRevived(true);
        }

        panel.gameObject.SetActive(false);
    }
}
