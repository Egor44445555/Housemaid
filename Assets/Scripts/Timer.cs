using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using UnityEngine.InputSystem.Controls;

public class Timer : MonoBehaviour
{
    public Text timerText;
    public float lifeTime = 60;
    StringBuilder builder;
    private int min = 0;
    private int sec = 0;
    [SerializeField] private int delta = 0;

    void Start()
    {
        builder = new StringBuilder(5);
        StartCoroutine(ITimer());
    }

    void Update()
    {
        /*float minutes = Mathf.FloorToInt(lifeTime / 60);
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
        }*/
    }

    IEnumerator ITimer()
    {
        while(true)
        {
            if (sec == 59)
            {
                min++;
                sec = -1;                
            }

            sec += delta;

            if (builder != null)
            {
                builder.Length = 0;
                builder.Append(min.ToString("D2"));
                builder.Append(":");
                builder.Append(sec.ToString("D2"));
                timerText.text = builder.ToString();
            }

            yield return new WaitForSeconds(1);
        }
    }
}
