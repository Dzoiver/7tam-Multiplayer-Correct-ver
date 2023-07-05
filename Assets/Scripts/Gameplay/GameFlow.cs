using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameFlow : MonoBehaviour
{
    [SerializeField] GameObject winnerPanel;
    [SerializeField] GameObject waitingPanel;
    TextMeshProUGUI winnerText;
    private int coins = 0;
    private string winnerName = "Player";
    public void ShowWinner()
    {
        winnerText.text = winnerName + " is the winner! He collected " + coins + " coins!";
        winnerPanel.SetActive(true);
    }

    public void StartMatch()
    {
        waitingPanel.SetActive(false);
    }

    public void GoBackToLobbyScene()
    {
        SceneManager.LoadScene(0);
    }
    // Start is called before the first frame update
    void Start()
    {
        winnerText = winnerPanel.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
