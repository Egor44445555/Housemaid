using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timerText;
    public float lifeTime = 60;

    void Update()
    {
        float minutes = Mathf.FloorToInt(lifeTime / 60);
        float seconds = Mathf.FloorToInt(lifeTime % 60);
        string zeroMinutes = minutes < 10 ? "0" : "";
        string zeroSeconds = seconds < 10 ? "0" : "";

        timerText.text = zeroMinutes + minutes.ToString() + ":" + zeroSeconds + seconds;

        if (lifeTime > 1)
        {
            lifeTime -= Time.deltaTime;
        }
    }
}
