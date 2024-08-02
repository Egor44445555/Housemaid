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

        timerText.text = minutes.ToString() + ":" + seconds;

        if (lifeTime > 1)
        {
            lifeTime -= Time.deltaTime;
        }
    }
}
