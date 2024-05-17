using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class UiController : MonoBehaviour
{
    public PlayerController playerController;

    private Label timeLbl;
    private Label coinLbl;

    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        timeLbl = root.Q<Label>("TimeLabel");
        coinLbl = root.Q<Label>("CoinLabel");
    }

    private void Update()
    {
        timeLbl.text = TimeSpan.FromSeconds(playerController.runTime).ToString(@"mm\:ss\.f");
        coinLbl.text = "$" + playerController.collectedCoins.ToString();
    }
}
