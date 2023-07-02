using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Zenject;

public class TestLobby : MonoBehaviour
{
    [Inject]
    Menu menu;

    private Lobby hostLobby;
    private float heartbeatTimer;
    private Lobby joinedLobby;
    private string playerName;
    // Start is called before the first frame update
    private async void Start()
    {
        await UnityServices.InitializeAsync();
        AuthenticationService.Instance.ClearSessionToken();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        playerName = "Seifer" + Random.Range(0, 100);
        Debug.Log(playerName);
    }

    public string GetLobbyCode()
    {
        if (hostLobby != null)
            return hostLobby.LobbyCode;

        if (joinedLobby != null)
            return joinedLobby.LobbyCode;

        return "No name yet";
    }

    public async void CreateLobby()
    {
        try
        {
            string lobbyName = "MyLobby";
            int maxPlayers = 4;
            CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions
            {
                IsPrivate = true,
                Player = GetPlayer()
            };

            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, createLobbyOptions);

            hostLobby = lobby;
            Debug.Log("Created Lobby! " + lobby.Name + " " + lobby.MaxPlayers + " lobby code: " + lobby.LobbyCode);

            PrintPlayers(hostLobby);
            menu.OpenLobbyPanel();
        } catch (LobbyServiceException e)
        {
            menu.ReturnToMenu();
            menu.mesBox.DisplayMessage("Couldn't create a lobby", true);
            Debug.Log(e);
        }
    }

    public async void JoinLobby(string lobbyName)
    {
        try
        {
            JoinLobbyByCodeOptions joinLobbyByCodeOptions = new JoinLobbyByCodeOptions
            {
                Player = GetPlayer()
            };

            joinedLobby = await Lobbies.Instance.JoinLobbyByCodeAsync(lobbyName, joinLobbyByCodeOptions); // 
            Debug.Log("Joined Lobby with code " + lobbyName);

            PrintPlayers(joinedLobby);
            menu.OpenLobbyPanel();
        } catch (LobbyServiceException e)
        {
            Debug.Log(e);
            menu.mesBox.DisplayMessage("Couldn't join a lobby", true);
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

    private async void LeaveLobby()
    {
        try
        {
            await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);
        }
        catch
        {
            Debug.Log("something went wrong");
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

    private void PrintPlayers(Lobby lobby)
    {
        Debug.Log("Players in lobby " + lobby.Name);
        foreach (Player player in lobby.Players)
        {
            Debug.Log(player.Id + " " + player.Data["PlayerName"].Value);
        }
    }

    private void Update()
    {
        HandleLobbyHeartbeat();
    }
}
