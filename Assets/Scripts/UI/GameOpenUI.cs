using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOpenUI : MonoBehaviour
{
    [SerializeField] private Button openButton;
    [SerializeField] private GameObject gamePanel;

    private void OnValidate()
    {
        if (openButton == null)
            openButton = GetComponent<Button>();
    }

    private void Start()
    {
        openButton.onClick.RemoveAllListeners();
        openButton.onClick.AddListener(OpenGame);


    }
    private void OpenGame()
    {
        gamePanel.SetActive(true);
    }
}
