using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timerText;
    public float lifeTime = 60F;
    private float gameTime;

    void Update()
    {
        timerText.text = lifeTime.ToString();
        gameTime += 1 * Time.deltaTime;

        if (gameTime >=1)
        {
            lifeTime -= 1;
            gameTime = 0;
        }
    }
}
