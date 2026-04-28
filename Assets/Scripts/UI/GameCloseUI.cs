using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCloseUI : MonoBehaviour
{
    [SerializeField] private Button quitButton;
    [SerializeField] private GameObject gamePanel;

    [SerializeField] private WheelRotator wheelRotator;

    private void OnValidate()
    {
        if (quitButton == null)
            quitButton = GetComponent<Button>();
    }

    private void Start()
    {
        quitButton.onClick.RemoveAllListeners();
        quitButton.onClick.AddListener(CloseGame);

        wheelRotator.OnSpinStart += ButtonNotInteractable;
        wheelRotator.OnSpinFinished += ButtonInteractable;

    }

    private void OnDisable()
    {
        wheelRotator.OnSpinStart -= ButtonNotInteractable; 
        wheelRotator.OnSpinFinished -= ButtonInteractable;
    }
    private void ButtonInteractable(int idx)
    {
        quitButton.interactable = true;
    }

    private void ButtonNotInteractable()
    {
        //when spin starts if safe zone -> allow quiting
        if (GameManager.Instance.IsSafeZone())
        {
            quitButton.interactable = true;
            return;
        }

        //if not cannot quit
        quitButton.interactable = false;
    }
    private void CloseGame()
    {
        gamePanel.SetActive(false);
        GameManager.Instance.RefreshGame();
    }
}
