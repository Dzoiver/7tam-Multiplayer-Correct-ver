using UnityEngine;
using TMPro;

public class NameScript : MonoBehaviour
{
    TextMeshProUGUI playerUItext;
    // Start is called before the first frame update
    void Start()
    {
        playerUItext = GetComponent<TextMeshProUGUI>();
        if (TestLobby.instance != null)
        {
            playerUItext.text = "Your name: " + TestLobby.instance.GetPlayerName();
        }
    }
}
