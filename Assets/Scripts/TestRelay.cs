using UnityEngine;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using System.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.Authentication;

public class TestRelay : MonoBehaviour
{
    public static TestRelay instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this);
    }

    public bool IsAuthorizing()
    {
        if (UnityServices.State == ServicesInitializationState.Uninitialized)
        {
            return false;
        }
        return true;
    }

    private async void Start()
    {
        if (TestLobby.instance == null)
        {
            await UnityServices.InitializeAsync();
            AuthenticationService.Instance.ClearSessionToken();
            AuthenticationService.Instance.SignedIn += () =>
            {
                Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
            };
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }

    public async void CreateRealyViaButton()
    {
        await CreateRelay();
    }
    public async Task<string> CreateRelay()
    {
        try
        {
            Debug.Log("Starting creating relay");
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);

            string relayCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartHost();

            Debug.Log("Relay code: " + relayCode);
            return relayCode;
        } catch (RelayServiceException e)
        {
            Debug.Log(e);
            return null;
        }
    }

    public async void JoinRelay(string joinCode)
    {
        try
        {
            Debug.Log("Joining Relay with " + joinCode);
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

            RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartClient();
        } catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
    }
}
