using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameFlow : MonoBehaviour
{
    [SerializeField] GameObject winnerPanel;
    [SerializeField] GameObject waitingPanel;
    [SerializeField] TextMeshProUGUI winnerText;
    private int playersConnected = 0;
    private int playersAlive = 0;
    public bool canPlayerMove = false;
    static public GameFlow instance;
    private Dictionary<TopDownCharacterController, int> playerList = new Dictionary<TopDownCharacterController, int>();

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public void AddPlayerToList(TopDownCharacterController player, int id)
    {
        playerList.Add(player, id);
        PlayerHasConnected();
    }

    public void ShowWinner()
    {
        winnerPanel.SetActive(true);
        TopDownCharacterController player = playerList.First().Key;
        string playerName = player.Name;
        int coins = player.CoinCount;
        winnerText.text = playerName + " is the winner! He collected " + coins + " coins!";
    }

    public void StartMatch()
    {
        waitingPanel.SetActive(false);
        canPlayerMove = true;
        Debug.Log("Starting match");
    }

    private void PlayerHasConnected()
    {
        Debug.Log("Player connected");
        playersConnected++;
        playersAlive++;
        if (playersConnected > 1)
        {
            StartMatch();
        }
    }

    public void OnPlayerDeath(TopDownCharacterController player)
    {
        Debug.Log(player.Name + " died");
        playersAlive--;
        playerList.Remove(player);
        if (playersAlive == 1)
            ShowWinner();
    }

    public void GoBackToLobbyScene()
    {
        // Disconnect from relay and close the lobby
        Debug.Log("Trying to leave the lobby");
        TestLobby.instance.LeaveLobby();
        Debug.Log("Trying to leave the relay");
        TestRelay.instance.DisconnectFromRelay();
        SceneManager.LoadScene(0);
    }
}
