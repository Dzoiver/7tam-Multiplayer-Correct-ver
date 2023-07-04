using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class LoadingScene : MonoBehaviour
{
    [SerializeField] TestLobby lobby;
    
    private static LoadingScene instance;
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
    public async void LoadGame(int sceneId)
    {
        SceneManager.LoadScene(sceneId);

        if (sceneId == 1)
            LoadGame(2);

        if (sceneId == 2)
        {
            if (lobby.IsHost())
            {
                Debug.Log("Trying to create relay");
                string relayCode = await TestRelay.instance.CreateRelay();

                lobby.UpdateLobbyCode(relayCode);
            }
            else
            {
                Debug.Log("Trying to join relay");
                lobby.TryJoin();
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1) // Loads game scene if this is loading screen
        {
            SceneManager.LoadScene(2);
        }
    }
}
