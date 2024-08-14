using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class Timer : MonoBehaviour
{
    public Text timerText;
    public float lifeTime = 60;
    StringBuilder builder;

    void Start()
    {
        builder = new StringBuilder(5);
    }

    void Update()
    {
        float minutes = Mathf.FloorToInt(lifeTime / 60);
        float seconds = Mathf.FloorToInt(lifeTime % 60);
        string zeroMinutes = minutes < 10 ? "0" : "";
        string zeroSeconds = seconds < 10 ? "0" : "";

        if (builder != null)
        {
            builder.Length = 0;
            builder.Append(zeroMinutes + minutes.ToString());
            builder.Append(":");
            builder.Append(zeroSeconds + seconds);
            timerText.text = builder.ToString();
        }        

        if (lifeTime > 1)
        {
            lifeTime -= Time.deltaTime;
        }
    }
}
