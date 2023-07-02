using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    public void LoadGame(int sceneId)
    {
        SceneManager.LoadScene(sceneId);
    }

    public void LoadSceneAsync(int sceneId)
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1) // Loads game scene if this is loading screen
        {
            SceneManager.LoadScene(2);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
