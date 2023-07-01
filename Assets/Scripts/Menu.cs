using UnityEngine;
using Zenject;
using TMPro;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [Inject]
    TestLobby lobby;

    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject hostedPanel;
    [SerializeField] GameObject joinPanel;
    [SerializeField] GameObject inputField;
    [SerializeField] GameObject lobbyNameText;
    public void OpenLobbyPanel()
    {
        mainMenu.SetActive(false);
        hostedPanel.SetActive(true);
        lobby.CreateLobby();
        lobbyNameText.GetComponent<TextMeshProUGUI>().text = lobby.GetLobbyCode();
    }

    public void OpenJoinPanel()
    {
        mainMenu.SetActive(false);
        joinPanel.SetActive(true);
    }

    public void CloseLobbyPanel()
    {
        hostedPanel.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void CloseJoinPanel()
    {
        mainMenu.SetActive(true);
        joinPanel.SetActive(false);
    }

    public void ConnectToLobby()
    {
        TextMeshProUGUI inputText = inputField.GetComponent<TextMeshProUGUI>();
        Debug.Log(inputText.text);
        lobby.JoinLobby(inputText.text);
    }
}
