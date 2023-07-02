using UnityEngine;

public class FPSTarget : MonoBehaviour
{
    [SerializeField] int target = 60;

    void Awake()
    {
        DontDestroyOnLoad(this);
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = target;
    }
}
