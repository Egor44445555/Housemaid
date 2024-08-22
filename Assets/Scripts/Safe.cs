using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Safe : MonoBehaviour
{
    [SerializeField] public string buttonValue;
    string codeText = "";

    public void ButtonClick()
    {
        if (PlayerPrefs.GetString("SafeCode").Length < 4 && buttonValue != "#")
        {
            foreach (Safe item in FindObjectsByType<Safe>(FindObjectsSortMode.None))
            {
                if (item.buttonValue == "DisplayCode")
                {
                    PlayerPrefs.SetString("SafeCode", PlayerPrefs.GetString("SafeCode") + buttonValue);
                    PlayerPrefs.Save();
                    item.GetComponent<Text>().text = PlayerPrefs.GetString("SafeCode", PlayerPrefs.GetString("SafeCode") + buttonValue);
                }
            }
        }

        if (buttonValue == "#")
        {
            CheckCode();
            ClearDisplayCode();
        }
    }

    void CheckCode()
    {
        if (PlayerPrefs.GetString("SafeCode") == "0657")
        {
            print("Open safe");
        }
    }

    void ClearDisplayCode()
    {
        foreach (Safe item in FindObjectsByType<Safe>(FindObjectsSortMode.None))
        {
            if (item.buttonValue == "DisplayCode")
            {
                PlayerPrefs.SetString("SafeCode", "");
                PlayerPrefs.Save();
                item.GetComponent<Text>().text = PlayerPrefs.GetString("SafeCode", "");
            }
        }
    }

    void Start()
    {
        PlayerPrefs.SetString("SafeCode", "");
        PlayerPrefs.Save();
    }
}
