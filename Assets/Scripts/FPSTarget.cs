using UnityEngine;

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
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = target;
    }
}
