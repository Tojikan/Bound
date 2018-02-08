using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleTimer : MonoBehaviour
{
    public float loopTime;
    public int loopLength;
    public float timeScale;
    public int time;

    void Start()
    {
        if (loopLength % timeScale != 0)
        {
            Debug.Log("TimeScale does not match!");
        }
        loopLength = (int)(loopTime / timeScale);

        StartCoroutine("Timer", loopLength);
    }

    IEnumerator Timer(float loop)
    {
        time = 0;
        while(true)
        {
            yield return new WaitForSeconds(timeScale);
            time += 1;

            if (time >= loopLength)
            {
                time = 0;
            }
        }
    }

}
