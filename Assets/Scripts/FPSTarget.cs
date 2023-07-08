using UnityEngine;
using UnityEngine.SceneManagement;

public class FPSTarget : MonoBehaviour
{
    [SerializeField] int target = 60;
    private static FPSTarget instance;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this);
        /*
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            Application.targetFrameRate = target / 2;
        }
        */
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = target;
    }
}
