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
    [SerializeField] TextMeshProUGUI playersList;
    [SerializeField] Button startButton;
    [SerializeField] LoadingScene loadingScene;


    [SerializeField] TMP_InputField lobbyNameHost;
    public void CreateLobby()
    {
        mesBox.DisplayMessage("Creating a lobby");
        lobby.CreateLobby(lobbyNameHost.text);
        // mesBox.DisplayMessage("Creating lobby...");
    }

    public void JoinLobby()
    {
        loadingScene.LoadGame(1);
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

    public void SetLobbyName()
    {
        mainMenu.SetActive(false);
        mesBox.SetName();
    }

    public void ConnectToLobby()
    {
        TMP_InputField inputText = inputField.GetComponent<TMP_InputField>();
        if (inputText.text.Length > 0)
        {
            Debug.Log("Trying to join by name: " + inputText.text + ".");
            mesBox.DisplayMessage("Joining a lobby...");
            lobby.JoinLobbyByName(inputText.text);
        }
        else
        {
            Debug.Log("Name can't be empty");
            mesBox.DisplayMessage("Enter lobby name!", true);
        }
    }
}
