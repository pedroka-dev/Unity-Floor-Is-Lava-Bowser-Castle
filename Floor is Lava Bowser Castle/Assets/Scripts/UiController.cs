using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class UiController : MonoBehaviour
{
    public PlayerController playerController;

    private Label timeLbl;
    private Label coinLbl;
    private Label victoryLbl;

    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        timeLbl = root.Q<Label>("TimeLabel");
        coinLbl = root.Q<Label>("CoinLabel");
        victoryLbl = root.Q<Label>("VictoryLabel");
    }

    private void Update()
    {
        timeLbl.text = TimeSpan.FromSeconds(playerController.runTime).ToString(@"mm\:ss\.f");
        coinLbl.text = "$" + playerController.collectedCoins.ToString();
        if (playerController.isGameWon)
        {
            victoryLbl.visible = true;
        }
        if (!playerController.allowPlayerMovement) 
        {
            victoryLbl.visible = false;
            timeLbl.visible = false;
            coinLbl.visible = false;
        }
    }
}
