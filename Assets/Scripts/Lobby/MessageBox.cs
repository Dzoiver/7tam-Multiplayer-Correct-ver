using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessageBox : MonoBehaviour
{
    [SerializeField] GameObject button;
    [SerializeField] TextMeshProUGUI textComponent;

    public void DisplayMessage(string message, bool ShowButton = false)
    {
        button.SetActive(false);
        textComponent.text = message;
        gameObject.SetActive(true);

        if (ShowButton)
            button.SetActive(true);

    }

    public void SetName()
    {
        textComponent.text = "Enter lobby name";
        gameObject.SetActive(true);
        button.SetActive(true);
    }

    public void RemoveMessage()
    {
        button.SetActive(false);
        gameObject.SetActive(false);
    }
}
