using UnityEngine;
using Zenject;
using TMPro;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [Inject]
    TestLobby lobby;

    [Inject][HideInInspector]
    public MessageBox mesBox;

    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject hostedPanel;
    [SerializeField] GameObject joinPanel;
    [SerializeField] GameObject inputField;
    [SerializeField] GameObject lobbyNameText;
    [SerializeField] TextMeshProUGUI playersList;
    [SerializeField] Button startButton;
    public void CreateLobby()
    {
        lobby.CreateLobby();
        mesBox.DisplayMessage("Creating lobby...");
    }

    public void OpenLobbyPanel()
    {
        mainMenu.SetActive(false);
        joinPanel.SetActive(false);
        hostedPanel.SetActive(true);
        lobbyNameText.GetComponent<TextMeshProUGUI>().text = lobby.GetLobbyCode();
        UpdatePlayersList();
    }

    public void UpdatePlayersList()
    {
        playersList.text = lobby.ReturnPlayersList();
    }

    public void OpenJoinPanel()
    {
        mainMenu.SetActive(false);
        joinPanel.SetActive(true);
    }

    public void ReturnToMenu()
    {
        joinPanel.SetActive(false);
        hostedPanel.SetActive(false);
        mainMenu.SetActive(true);
        startButton.interactable = false;

        lobby.LeaveLobby();
    }

    public void PlayButtonSetInteractable(bool isInteractable)
    {
        if (isInteractable)
            startButton.interactable = true;
        else
            startButton.interactable = false;
    }

    public void ConnectToLobby()
    {
        TMP_InputField inputText = inputField.GetComponent<TMP_InputField>();
        if (inputText.text.Length == 6)
        {
            Debug.Log("Trying to join by code: " + inputText.text);
            mesBox.DisplayMessage("Joining a lobby...");
            lobby.JoinLobby(inputText.text.ToUpper());
        }
        else
        {
            Debug.Log("The code length must be 6 symbols");
            mesBox.DisplayMessage("The code length must be 6 symbols!", true);
            // Display some message on the screen
        }

    }
}
