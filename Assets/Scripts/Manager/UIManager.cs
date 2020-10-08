using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    private void Awake()
    {
        instance = this;
    }
    public Text messageText;
    string[] messageString = new string[5];
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMessage(string m)
    {
        for (int i = 1; i < 5; i++)
        {
            messageString[i - 1] = messageString[i];
        }
        messageString[4] = m + "\n";
        messageText.text = messageString[0] + messageString[1] + messageString[2] + messageString[3] + messageString[4];
    }
}
