using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Safe : MonoBehaviour
{
    [SerializeField] public string buttonValue;
    [SerializeField] public GameObject openedSafe;
    [SerializeField] public GameObject safePanel;

    public void ButtonClick()
    {
        if (PlayerPrefs.GetString("SafeCode").Length < 4 && buttonValue != "#")
        {
            FindAnyObjectByType<AudioManager>().InteractionSound("SoundSafeCode", true);

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

        if (buttonValue == "*")
        {
            ClearDisplayCode();
        }
    }

    void CheckCode()
    {
        if (PlayerPrefs.GetString("SafeCode") == "0657")
        {
            FindAnyObjectByType<AudioManager>().InteractionSound("SafeCodeAccessible", true);
            StartCoroutine(OpenSafe());
        } else
        {
            FindAnyObjectByType<AudioManager>().InteractionSound("SafeCodeDenied", true);        
        }
    }

    IEnumerator OpenSafe()
    {
        yield return new WaitForSeconds(1);
        safePanel.SetActive(false);
        openedSafe.SetActive(true);
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
