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
    [SerializeField] TextMeshProUGUI winnerText;
    private int coins = 0;
    private int playersConnected = 0;
    private int playersAlive = 0;
    public bool canPlayerMove = false;
    static public GameFlow instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public void ShowWinner(string winnerName)
    {
        winnerPanel.SetActive(true);
        winnerText.text = winnerName + " is the winner! He collected " + coins + " coins!";
    }

    public void StartMatch()
    {
        waitingPanel.SetActive(false);
        canPlayerMove = true;
        Debug.Log("Starting match");
    }

    public void PlayerHasConnected()
    {
        Debug.Log("Player connected");
        playersConnected++;
        playersAlive++;
        if (playersConnected > 1)
        {
            StartMatch();
        }
    }

    public void OnPlayerDeath(string playerName)
    {
        Debug.Log(playerName + " died");
        playersAlive--;
        if (playersAlive == 1)
            ShowWinner(playerName);
    }

    public void GoBackToLobbyScene()
    {
        SceneManager.LoadScene(0);
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
