using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessageBox : MonoBehaviour
{
    [SerializeField] GameObject button;
    [SerializeField] TextMeshProUGUI textComponent;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    public void DisplayMessage(string message, bool ShowButton = false)
    {
        Debug.Log("Displaying a message");
        textComponent.text = message;
        gameObject.SetActive(true);

        if (ShowButton)
            button.SetActive(true);
    }

    public void RemoveMessage()
    {
        gameObject.SetActive(false);
        button.SetActive(false);
    }
}
