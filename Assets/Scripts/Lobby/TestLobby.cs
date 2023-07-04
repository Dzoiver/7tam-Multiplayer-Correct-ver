using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Zenject;
using TMPro;

public class TestLobby : MonoBehaviour
{
    [Inject]
    Menu menu;

    [SerializeField] TextMeshProUGUI playerNameText;
    [SerializeField] LoadingScene loadingScene;
    private Lobby hostLobby;
    private Lobby joinedLobby;
    private float heartbeatTimer;
    private float lobbyUpdateTimer;
    private string playerName;
    string KEY_START_GAME = "StartGame_RelayCode";
    private string foundLobbyId = "";

    public bool IsHost()
    {
        if (hostLobby != null)
            return true;

        return false;
    }
    // Start is called before the first frame update
    private async void Start()
    {
        DontDestroyOnLoad(this);
        await UnityServices.InitializeAsync();
        AuthenticationService.Instance.ClearSessionToken();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        playerName = "Player" + Random.Range(0, 100);
        playerNameText.text += playerName;
        Debug.Log(playerName);
    }

    public Lobby GetCurrentLobby()
    {
        if (joinedLobby != null)
            return joinedLobby;

        if (hostLobby != null)
            return hostLobby;

        return null;
    }

    public string GetLobbyCode()
    {
        if (hostLobby != null)
            return hostLobby.LobbyCode;

        if (joinedLobby != null)
            return joinedLobby.LobbyCode;

        return "No name yet";
    }

    public async void UpdateLobbyCode(string relayCode)
    {
        try { 
        Lobby lobby = await Lobbies.Instance.UpdateLobbyAsync(joinedLobby.Id, new UpdateLobbyOptions
        {
            Data = new Dictionary<string, DataObject>
            {
                { KEY_START_GAME, new DataObject(DataObject.VisibilityOptions.Member, relayCode) }
            }
        });

        joinedLobby = lobby;
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public async void CreateLobby(string lobbyName)
    {
        try
        {
            int maxPlayers = 4;
            CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions
            {
                IsPrivate = false,
                Player = GetPlayer(),
                Data = new Dictionary<string, DataObject> {
                    { KEY_START_GAME, new DataObject(DataObject.VisibilityOptions.Member, "0") }
                }
            };

            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, createLobbyOptions);

            hostLobby = lobby;
            joinedLobby = hostLobby;
            Debug.Log("Created Lobby! " + lobby.Name + " " + lobby.MaxPlayers + " lobby code: " + lobby.LobbyCode);
            loadingScene.LoadGame(1);

        } catch (LobbyServiceException e)
        {
            menu.mesBox.DisplayMessage("Couldn't create a lobby", true);
            Debug.Log(e);
        }
    }

    public async void JoinLobbyByName(string lobbyName)
    {
        try
        {
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions
            {
                Count = 5,
            };
            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync(queryLobbiesOptions);

            Debug.Log("Lobbies without filter found: " + queryResponse.Results.Count);

            FindLobbyWithName(lobbyName);

            if (queryResponse.Results.Count > 0)
            {
                foundLobbyId = queryResponse.Results[0].Id;
            }
            else
            {
                foundLobbyId = "";
                menu.mesBox.DisplayMessage("Couldn't find a lobby", true);
                Debug.Log("Lobby not found");
            }


            JoinLobbyByIdOptions joinLobbyByIdOptions = new JoinLobbyByIdOptions
            {
                Player = GetPlayer()
            };

            if (foundLobbyId != "")
            {
                Debug.Log("Lobby was found, joining...");
                joinedLobby = await Lobbies.Instance.JoinLobbyByIdAsync(foundLobbyId, joinLobbyByIdOptions);

                menu.JoinLobby();

                Debug.Log("Joined Lobby by name " + lobbyName);
            }
            Debug.Log("foundLobbyId = " + foundLobbyId);
        } catch (LobbyServiceException e)
        {
            Debug.Log(e);
            menu.mesBox.DisplayMessage("Couldn't join a lobby", true);
        }
    }

    private async void FindLobbyWithName(string name)
    {
        try
        {
            Debug.Log("Filtering by name: " + name + ".");
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions
            {
                Count = 5,
                Filters = new List<QueryFilter> {
                    new QueryFilter(QueryFilter.FieldOptions.Name, name, QueryFilter.OpOptions.EQ)
                }
            };
            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync(queryLobbiesOptions);

            if (queryResponse.Results.Count > 0)
            {
                foundLobbyId = queryResponse.Results[0].Id;
            }
            else
            {
                foundLobbyId = "";
                menu.mesBox.DisplayMessage("Couldn't find a lobby", true);
                Debug.Log("Lobby not found");
            }
        } catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private Player GetPlayer()
    {
        return new Player
        {
            Data = new Dictionary<string, PlayerDataObject>
                {
                    {
                        "PlayerName", new  PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, playerName)
                    }
                }
        };
    }

    public async void LeaveLobby()
    {
        try
        {
            await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);
            joinedLobby = null;
            hostLobby = null;
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private async void HandleLobbyHeartbeat()
    {
        if (hostLobby != null)
        {
            heartbeatTimer -= Time.deltaTime;
            if (heartbeatTimer < 0f)
            {
                float heartbeatTimerMax = 15;
                heartbeatTimer = heartbeatTimerMax;

                await LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
            }
        }
    }

    public string ReturnPlayersList()
    {
        string playersList = "";
        foreach (Player player in GetCurrentLobby().Players)
        {
            playersList += player.Data["PlayerName"].Value + "\n";
        }
        return playersList;
    }

    private void PrintPlayers(Lobby lobby)
    {
        Debug.Log("Players in lobby " + lobby.Name);
        foreach (Player player in lobby.Players)
        {
            Debug.Log(player.Id + " " + player.Data["PlayerName"].Value);
        }
    }

    public void TryJoin()
    {
        Debug.Log("Joined lobby: " + joinedLobby);
        Debug.Log("Keystart value: " + joinedLobby.Data[KEY_START_GAME].Value);
        if (joinedLobby.Data[KEY_START_GAME].Value != "0")
        {
            // Start Game!
            if (!IsHost())
            {
                TestRelay.instance.JoinRelay(joinedLobby.Data[KEY_START_GAME].Value);
            }

            joinedLobby = null;
        }
    }

    private async void HandleLobbyPollForUpdates()
    {
        if (joinedLobby != null)
        {
            lobbyUpdateTimer -= Time.deltaTime;
            if (lobbyUpdateTimer < 0f)
            {
                try
                {
                    float lobbyUpdateTimerMax = 1.1f;
                    lobbyUpdateTimer = lobbyUpdateTimerMax;

                    Lobby lobby = await LobbyService.Instance.GetLobbyAsync(joinedLobby.Id);

                    joinedLobby = lobby;
                    Debug.Log("Lobby is still alive");
                    if (joinedLobby.Data[KEY_START_GAME].Value != "0")
                    {
                        // Start Game!
                        if (!IsHost())
                        {
                            TestRelay.instance.JoinRelay(joinedLobby.Data[KEY_START_GAME].Value);
                        }
                    }
                }
                catch (LobbyServiceException e)
                {
                    Debug.Log(e);
                }
            }
        }
    }

    private void Update()
    {
        HandleLobbyHeartbeat();
        HandleLobbyPollForUpdates();
    }
}
